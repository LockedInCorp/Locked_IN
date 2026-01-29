import { apiClient } from '@/lib/apiClient';

export interface UserProfileResponse {
    success: boolean
    message: string
    data?: {
        id: number
        email: string
        username: string
        avatar?: File | string | null
        avatarURL?: string
        availability?: Record<string, string[]>
    }
}

export interface UpdateProfileRequest {
    username: string
    email: string
    avatarUrl?: string
}

export interface UpdateAvailabilityRequest {
    availability: Record<string, string[]>
}

export interface GameProfileResponse {
    success: boolean
    message: string
    data?: Array<{
        id: number
        userId: number
        gameId: number
        gameName: string
        isFavorite: boolean
        rank?: string
    }>
}

export interface TagsResponse {
    success: boolean
    message: string
    data?: {
        games: Array<{
            id: number
            name: string
        }>
        experienceTags: Array<{
            id: number
            experiencelevel: string
        }>
        preferenceTags: Array<{
            id: number
            name: string
        }>
        gameExperiences: Array<{
            id: number
            experience: string
        }>
        gameplayPreferences: Array<{
            id: number
            preference: string
        }>
    }
}

export const getUserProfile = async (userId: number): Promise<UserProfileResponse> => {
    try {
        const response = await apiClient.get<UserProfileResponse>(`/user/${userId}`)
        return response.data
    } catch (error: any) {
        const errorData = error.response?.data || { 
            success: false, 
            message: 'Failed to fetch profile' 
        }
        throw new Error(errorData.message || 'Failed to fetch profile')
    }
}

export const updateUserProfile = async (
    userId: number, 
    data: UpdateProfileRequest
): Promise<UserProfileResponse> => {
    try {
        const response = await apiClient.put<UserProfileResponse>(`/user/profile/${userId}`, data)
        return response.data
    } catch (error: any) {
        const errorData = error.response?.data || { 
            success: false, 
            message: 'Failed to update profile' 
        }
        throw new Error(errorData.message || 'Failed to update profile')
    }
}

export const updateUserAvailability = async (
    availability: Record<string, string[]>
): Promise<UserProfileResponse> => {
    try {
        const response = await apiClient.put<UserProfileResponse>(`/user/availability`, { availability })
        return response.data
    } catch (error: any) {
        const errorData = error.response?.data || { 
            success: false, 
            message: 'Failed to update availability' 
        }
        throw new Error(errorData.message || 'Failed to update availability')
    }
}

export const getUserGameProfiles = async (userId: number): Promise<GameProfileResponse> => {
    try {
        const response = await apiClient.get<GameProfileResponse>(`/game-profile/favorites/${userId}`)
        return response.data
    } catch (error: any) {
        const errorData = error.response?.data || { 
            success: false, 
            message: 'Failed to fetch game profiles' 
        }
        throw new Error(errorData.message || 'Failed to fetch game profiles')
    }
}

export const getAllTags = async (): Promise<TagsResponse> => {
    try {
        const response = await apiClient.get<TagsResponse>('/tag')
        return response.data
    } catch (error: any) {
        const errorData = error.response?.data || { 
            success: false, 
            message: 'Failed to fetch tags' 
        }
        throw new Error(errorData.message || 'Failed to fetch tags')
    }
}
