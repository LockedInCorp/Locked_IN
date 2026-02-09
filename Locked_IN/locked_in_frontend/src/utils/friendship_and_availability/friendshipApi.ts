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

export const getFriendsList = async (userId: number): Promise<FriendshipResponse> => {
    try {
        const response = await apiClient.get<FriendshipDto[]>(`/friendship/list/${userId}`)
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

export const getPendingRequests = async (userId: number): Promise<PendingRequestsResponse> => {
    try {
        const response = await apiClient.get<PendingFriendshipRequestDto[]>(`/friendship/pending-requests/${userId}`)
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

export { 
    getFriendshipStatus,
    sendFriendRequest,
    cancelFriendRequest,
    deleteFriendship
} from '@/api/api'

export const getOutgoingPendingRequests = async (userId: number): Promise<PendingRequestsResponse> => {
    try {
        const friendsResponse = await getFriendsList(userId)
        const pendingResponse = await getPendingRequests(userId)
        
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
