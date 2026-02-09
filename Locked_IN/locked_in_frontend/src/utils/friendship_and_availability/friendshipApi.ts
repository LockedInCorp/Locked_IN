import { apiClient } from '@/api/apiClient';

export interface FriendshipDto {
    friendshipId: number
    friendId: number
    friendUsername: string
    status: string
    since: string
}

export interface PendingFriendshipRequestDto {
    friendshipId: number
    requesterId: number
    requesterUsername: string
    requestTimestamp: string
}

export interface FriendshipResponse {
    success: boolean
    message: string
    data?: FriendshipDto[]
}

export interface PendingRequestsResponse {
    success: boolean
    message: string
    data?: PendingFriendshipRequestDto[]
}

export const getFriendsList = async (): Promise<FriendshipResponse> => {
    try {
        const response = await apiClient.get<FriendshipDto[]>(`/friendship/list`)
        return {
            success: true,
            message: '',
            data: response.data || []
        }
    } catch (error: any) {
        const errorData = error.response?.data || { 
            success: false, 
            message: 'Failed to fetch friends list' 
        }
        throw new Error(errorData.message || 'Failed to fetch friends list')
    }
}

export const getPendingRequests = async (): Promise<PendingRequestsResponse> => {
    try {
        const response = await apiClient.get<PendingFriendshipRequestDto[]>(`/friendship/pending-requests`)
        return {
            success: true,
            message: '',
            data: response.data || []
        }
    } catch (error: any) {
        const errorData = error.response?.data || { 
            success: false, 
            message: 'Failed to fetch pending requests' 
        }
        throw new Error(errorData.message || 'Failed to fetch pending requests')
    }
}

export const acceptFriendRequest = async (friendshipId: number): Promise<void> => {
    try {
        await apiClient.put(`/friendship/requests/${friendshipId}/accept`)
    } catch (error: any) {
        const errorData = error.response?.data || { 
            message: 'Failed to accept friend request' 
        }
        throw new Error(errorData.message || 'Failed to accept friend request')
    }
}

export const declineFriendRequest = async (friendshipId: number): Promise<void> => {
    try {
        await apiClient.put(`/friendship/requests/${friendshipId}/decline`)
    } catch (error: any) {
        const errorData = error.response?.data || { 
            message: 'Failed to decline friend request' 
        }
        throw new Error(errorData.message || 'Failed to decline friend request')
    }
}

export { 
    getFriendshipStatus,
    sendFriendRequest,
    cancelFriendRequest,
    deleteFriendship
} from '@/api/api'

export const getOutgoingPendingRequests = async (userId: number): Promise<PendingRequestsResponse> => {
    try {
        const friendsResponse = await getFriendsList()
        const pendingResponse = await getPendingRequests()
        
        const allFriendships = [
            ...(friendsResponse.data || []),
            ...(pendingResponse.data || [])
        ]
        
        const outgoing: PendingFriendshipRequestDto[] = []
        
        for (const friendship of allFriendships) {
            if ('requesterId' in friendship && friendship.requesterId === userId) {
                outgoing.push(friendship as PendingFriendshipRequestDto)
            }
        }
        
        return {
            success: true,
            message: '',
            data: outgoing
        }
    } catch (error: any) {
        const errorData = error.response?.data || { 
            success: false, 
            message: 'Failed to fetch outgoing requests' 
        }
        throw new Error(errorData.message || 'Failed to fetch outgoing requests')
    }
}
