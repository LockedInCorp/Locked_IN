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
        const response = await apiClient.get<FriendshipResponse>(`/friendship/list/${userId}`)
        return response.data
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
        const response = await apiClient.get<PendingRequestsResponse>(`/friendship/pending-requests/${userId}`)
        return response.data
    } catch (error: any) {
        const errorData = error.response?.data || { 
            success: false, 
            message: 'Failed to fetch pending requests' 
        }
        throw new Error(errorData.message || 'Failed to fetch pending requests')
    }
}
