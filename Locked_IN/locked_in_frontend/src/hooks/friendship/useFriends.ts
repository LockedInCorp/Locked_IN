import { useState, useEffect, useCallback } from "react"
import { useAuthStore } from "@/stores/authStore"
import { getFriendsList, getPendingRequests } from "@/utils/friendship_and_availability/friendshipApi"
import { getUserProfile } from "@/api/api"
import type { FriendshipDto, PendingFriendshipRequestDto } from "@/utils/friendship_and_availability/friendshipApi"

export interface FriendWithAvailability extends FriendshipDto {
    availability?: Record<string, string[]>
    avatarUrl?: string
}

export function useFriends() {
    const { user } = useAuthStore()
    const [friends, setFriends] = useState<FriendWithAvailability[]>([])
    const [pendingRequests, setPendingRequests] = useState<PendingFriendshipRequestDto[]>([])
    const [userAvailability, setUserAvailability] = useState<Record<string, string[]>>({})
    const [isLoading, setIsLoading] = useState(false)
    const [error, setError] = useState<string | null>(null)

    const loadFriends = useCallback(async () => {
        if (!user?.id) {
            setError("User not logged in")
            setIsLoading(false)
            return
        }

        setIsLoading(true)
        setError(null)

        try {
            const userId = parseInt(user.id)
            
            const [friendsResponse, pendingResponse, userProfile] = await Promise.all([
                getFriendsList(userId),
                getPendingRequests(userId).catch(() => ({ success: false, message: '', data: [] })),
                getUserProfile(userId)
            ])

            if (!friendsResponse.success) {
                throw new Error(friendsResponse.message || 'Failed to load friends')
            }

            setFriends(friendsResponse.data || [])
            setPendingRequests(pendingResponse.data || [])
            
            setUserAvailability(userProfile?.availability || {})
        } catch (err) {
            console.error('Error loading friends/availability:', err)
            const errorMessage = err instanceof Error ? err.message : 'Failed to load friends'
            setError(errorMessage)
        } finally {
            setIsLoading(false)
        }
    }, [user?.id])

    const loadFriendAvailability = useCallback(async (friendId: number) => {
        try {
            const profile = await getUserProfile(friendId)
            if (profile?.availability) {
                setFriends(prev => prev.map(friend => 
                    friend.friendId === friendId 
                        ? { ...friend, availability: profile.availability }
                        : friend
                ))
            }
        } catch (err) {
            console.error(`Failed to load availability for friend ${friendId}:`, err)
        }
    }, [])

    useEffect(() => {
        if (user?.id) {
            loadFriends()
        }
    }, [user?.id, loadFriends])

    return {
        friends,
        pendingRequests,
        userAvailability,
        isLoading,
        error,
        loadFriends,
        loadFriendAvailability,
        setUserAvailability
    }
}
