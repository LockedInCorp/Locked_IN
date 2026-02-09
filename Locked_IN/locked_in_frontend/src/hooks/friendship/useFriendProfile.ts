import { useState, useEffect, useCallback } from "react"
import { useAuthStore } from "@/stores/authStore"
import { 
    getUserProfile, 
    getUserGameProfiles,
    getFriendshipStatus, 
    sendFriendRequest, 
    cancelFriendRequest, 
    deleteFriendship
} from "@/api/api"
import { extractAvatarPath, getImageUrl } from "@/utils/imageUtils"
import { 
    getFriendsList,
    getPendingRequests
} from "@/utils/friendship_and_availability/friendshipApi"
import type { GameProfile } from "@/stores/authStore"

export type FriendshipStatus = "None" | "Pending" | "Accepted" | "Blocked"

export function useFriendProfile(friendId: number) {
    const { user } = useAuthStore()
    const [isLoading, setIsLoading] = useState(false)
    const [isActionLoading, setIsActionLoading] = useState(false)
    const [error, setError] = useState<string | null>(null)
    const [friendshipStatus, setFriendshipStatus] = useState<FriendshipStatus>("None")
    const [friendshipId, setFriendshipId] = useState<number | null>(null)
    const [isOutgoingRequest, setIsOutgoingRequest] = useState(false)
    const [profileData, setProfileData] = useState<{
        nickname: string
        gameProfiles: GameProfile[]
        avatarUrl?: string
        avatarFallback: string
    } | null>(null)

    const loadFriendProfile = useCallback(async () => {
        if (!user?.id || !friendId) {
            setError("User not logged in or friend ID missing")
            setIsLoading(false)
            return
        }

        setIsLoading(true)
        setError(null)

        try {
            const currentUserId = parseInt(user.id)
            
            const [profile, gameProfilesResponse, status] = await Promise.all([
                getUserProfile(friendId),
                getUserGameProfiles(friendId).catch(() => ({ success: false, message: '', data: [] })),
                getFriendshipStatus(friendId)
            ])

            if (!profile) {
                throw new Error('Failed to load friend profile')
            }
            
            let avatarUrl = getImageUrl(extractAvatarPath(profile as any))
            if (!avatarUrl) {
                avatarUrl = profile.avatarURL || "/assets/diverse-user-avatars.png"
            }
            
            const rawData = gameProfilesResponse.data
            const profilesArray = Array.isArray(rawData) ? rawData : (rawData ? [rawData] : [])
            
            const gameProfiles: GameProfile[] = profilesArray.map(gp => ({
                gameName: gp.gameName,
                preferences: [],
                experience: "",
                inGameNickname: "",
                ranking: gp.rank || "",
                role: ""
            }))

            setProfileData({
                nickname: profile.username,
                gameProfiles,
                avatarUrl: avatarUrl,
                avatarFallback: profile.username.charAt(0).toUpperCase() || "U"
            })

            setFriendshipStatus(status as FriendshipStatus)

            if (status === "Pending" || status === "Accepted") {
                const [friendsResponse, pendingResponse] = await Promise.all([
                    getFriendsList().catch(() => ({ success: false, message: '', data: [] })),
                    getPendingRequests().catch(() => ({ success: false, message: '', data: [] }))
                ])

                const friends = friendsResponse.data || []
                const pending = pendingResponse.data || []

                if (status === "Accepted") {
                    const friend = friends.find(f => f.friendId === friendId)
                    if (friend) {
                        setFriendshipId(friend.friendshipId)
                    }
                } else if (status === "Pending") {
                    const incomingReq = pending.find(p => p.requesterId === friendId)
                    if (incomingReq) {
                        setFriendshipId(incomingReq.friendshipId)
                        setIsOutgoingRequest(false)
                    } else {
                        try {
                            const friendPendingResponse = await getPendingRequests()
                            const friendPending = friendPendingResponse.data || []
                            const outgoingReq = friendPending.find(p => p.requesterId === currentUserId)
                            if (outgoingReq) {
                                setFriendshipId(outgoingReq.friendshipId)
                                setIsOutgoingRequest(true)
                            }
                        } catch (err) {
                            console.warn('Could not fetch outgoing request details:', err)
                        }
                    }
                }
            }
        } catch (err) {
            const errorMessage = err instanceof Error ? err.message : 'Failed to load friend profile'
            setError(errorMessage)
        } finally {
            setIsLoading(false)
        }
    }, [user?.id, friendId])

    const handleAddFriend = useCallback(async () => {
        if (!user?.id || isActionLoading) return

        setIsActionLoading(true)
        setError(null)

        try {
            await sendFriendRequest(friendId)
            setFriendshipStatus("Pending")
            setIsOutgoingRequest(true)
            await loadFriendProfile()
        } catch (err) {
            const errorMessage = err instanceof Error ? err.message : 'Failed to send friend request'
            setError(errorMessage)
        } finally {
            setIsActionLoading(false)
        }
    }, [user?.id, friendId, isActionLoading, loadFriendProfile])

    const handleCancelRequest = useCallback(async () => {
        if (!user?.id || !friendshipId || isActionLoading) return

        setIsActionLoading(true)
        setError(null)

        try {
            await cancelFriendRequest(friendshipId)
            setFriendshipStatus("None")
            setFriendshipId(null)
            setIsOutgoingRequest(false)
        } catch (err) {
            const errorMessage = err instanceof Error ? err.message : 'Failed to cancel friend request'
            setError(errorMessage)
        } finally {
            setIsActionLoading(false)
        }
    }, [user?.id, friendshipId, isActionLoading])

    const handleDeleteFriend = useCallback(async () => {
        if (!user?.id || !friendshipId || isActionLoading) return

        setIsActionLoading(true)
        setError(null)

        try {
            await deleteFriendship(friendId)
            setFriendshipStatus("None")
            setFriendshipId(null)
        } catch (err) {
            const errorMessage = err instanceof Error ? err.message : 'Failed to delete friendship'
            setError(errorMessage)
        } finally {
            setIsActionLoading(false)
        }
    }, [user?.id, friendshipId, isActionLoading])

    useEffect(() => {
        if (user?.id && friendId) {
            loadFriendProfile()
        }
    }, [user?.id, friendId, loadFriendProfile])

    return {
        isLoading,
        isActionLoading,
        error,
        friendshipStatus,
        friendshipId,
        isOutgoingRequest,
        profileData,
        loadFriendProfile,
        handleAddFriend,
        handleCancelRequest,
        handleDeleteFriend
    }
}
