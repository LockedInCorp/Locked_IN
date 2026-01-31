import { apiClient } from '@/lib/apiClient';

export interface UserProfileResponse {
    id: number
    email: string
    username: string
    avatarURL?: string
    availability?: Record<string, string[]>
}

export interface UpdateProfileRequest {
    username: string
    email: string
    avatar?: File | null;
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
        throw new Error(error.response?.data?.message || 'Failed to fetch profile')
    }
}

export const updateUserProfile = async (
    data: UpdateProfileRequest
): Promise<UserProfileResponse> => {
    try {
        const formData = new FormData()
        formData.append('username', data.username)
        formData.append('email', data.email)
        
        if (data.avatar instanceof File) {
            formData.append('avatarfile', data.avatar)
        }

        const response = await apiClient.put<UserProfileResponse>(`/user/profile`, formData, {
            headers: {
                'Content-Type': 'multipart/form-data',
            },
        })
        return response.data
    } catch (error: any) {
        throw new Error(error.response?.data?.message || 'Failed to update profile')
    }
}

//
// TODO: Connect availability from the backend
//
export const updateUserAvailability = async (
    userId: number,
    availability: Record<string, string[]>
): Promise<UserProfileResponse> => {
    try {
        const response = await apiClient.put<UserProfileResponse>(`/user/availability/${userId}`, { availability })
        return response.data
    } catch (error: any) {
        throw new Error(error.response?.data?.message || 'Failed to update availability')
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
