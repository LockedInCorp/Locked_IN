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
import { getFriendsList, getPendingRequests } from "@/utils/friendship_and_availability/friendshipApi"
import type { GameProfile } from "@/api/types"

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
        if (!user?.id || !friendId) return

        setIsLoading(true)
        setError(null)

        try {
            const currentUserId = parseInt(user.id)

            const [profile, gameProfilesData, status] = await Promise.all([
                getUserProfile(friendId),
                getUserGameProfiles(friendId),
                getFriendshipStatus(friendId)
            ])

            if (!profile) throw new Error('Failed to load friend profile')

            let avatarUrl = getImageUrl(extractAvatarPath(profile as any))
            if (!avatarUrl) avatarUrl = profile.avatarURL || "/assets/diverse-user-avatars.png"

            const rawProfiles: any[] = Array.isArray(gameProfilesData)
                ? gameProfilesData
                : Array.isArray((gameProfilesData as any)?.data)
                    ? (gameProfilesData as any).data
                    : (gameProfilesData as any)?.data != null
                        ? [(gameProfilesData as any).data]
                        : []
            const gameProfiles: GameProfile[] = rawProfiles.map((gp: any) => ({
                id: gp.id,
                gameId: gp.gameId,
                gameName: gp.gameName || gp.name || "Unknown Game",
                preferences: gp.preferences ?? [],
                experienceTagId: gp.experienceTagId,
                experience: gp.experienceTagId,
                inGameNickname: gp.inGameNickname ?? "",
                rank: gp.rank ?? "",
                role: gp.role ?? ""
            }));

            setProfileData({
                nickname: profile.username,
                gameProfiles,
                avatarUrl,
                avatarFallback: profile.username.charAt(0).toUpperCase() || "U"
            })

            setFriendshipStatus(status as FriendshipStatus)

            if (status === "Pending" || status === "Accepted") {
                const [friendsResponse, pendingResponse] = await Promise.all([
                    getFriendsList().catch(() => ({ data: [] })),
                    getPendingRequests().catch(() => ({ data: [] }))
                ])

                const friends = (friendsResponse as any).data || []
                const pending = (pendingResponse as any).data || []

                if (status === "Accepted") {
                    const friend = friends.find((f: any) => f.friendId === friendId)
                    if (friend) setFriendshipId(friend.friendshipId)
                } else if (status === "Pending") {
                    const incomingReq = pending.find((p: any) => p.requesterId === friendId)
                    if (incomingReq) {
                        setFriendshipId(incomingReq.friendshipId)
                        setIsOutgoingRequest(false)
                    } else {
                        try {
                            const friendPendingResponse = await getPendingRequests()
                            const friendPending = (friendPendingResponse as any).data || []
                            const outgoingReq = friendPending.find((p: any) => p.requesterId === currentUserId)
                            if (outgoingReq) {
                                setFriendshipId(outgoingReq.friendshipId)
                                setIsOutgoingRequest(true)
                            }
                        } catch (e) { console.warn(e) }
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
        try {
            await sendFriendRequest(friendId)
            setFriendshipStatus("Pending")
            setIsOutgoingRequest(true)
            await loadFriendProfile()
        } catch (err) { setError("Failed to send request") }
        finally { setIsActionLoading(false) }
    }, [user?.id, friendId, isActionLoading, loadFriendProfile])

    const handleCancelRequest = useCallback(async () => {
        if (!user?.id || !friendshipId || isActionLoading) return
        setIsActionLoading(true)
        try {
            await cancelFriendRequest(friendshipId)
            setFriendshipStatus("None")
            setFriendshipId(null)
            setIsOutgoingRequest(false)
        } catch (err) { setError("Failed to cancel request") }
        finally { setIsActionLoading(false) }
    }, [user?.id, friendshipId, isActionLoading])

    const handleDeleteFriend = useCallback(async () => {
        if (!user?.id || !friendshipId || isActionLoading) return
        setIsActionLoading(true)
        try {
            await deleteFriendship(friendId)
            setFriendshipStatus("None")
            setFriendshipId(null)
        } catch (err) { setError("Failed to delete friendship") }
        finally { setIsActionLoading(false) }
    }, [user?.id, friendshipId, isActionLoading])

    useEffect(() => {
        if (user?.id && friendId) loadFriendProfile()
    }, [user?.id, friendId, loadFriendProfile])

    return {
        isLoading,
        isActionLoading,
        error,
        friendshipStatus,
        friendshipId,
        isOutgoingRequest,
        profileData,
        handleAddFriend,
        handleCancelRequest,
        handleDeleteFriend,
        loadFriendProfile
    }
}