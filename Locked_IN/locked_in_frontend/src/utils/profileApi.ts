const API_BASE_URL = 'http://localhost:5122/api'

export interface UserProfileResponse {
    success: boolean
    message: string
    data?: {
        id: number
        email: string
        username: string
        avatar?: File | string | null
        AvatarURL?: string
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
    const response = await fetch(`${API_BASE_URL}/user/${userId}`, {
        method: 'GET',
        credentials: 'include',
    })

    if (!response.ok) {
        const errorData = await response.json().catch(() => ({ 
            success: false, 
            message: 'Failed to fetch profile' 
        }))
        throw new Error(errorData.message || 'Failed to fetch profile')
    }

    const data = await response.json()
    
    return data
}

export const updateUserProfile = async (
    userId: number, 
    data: UpdateProfileRequest
): Promise<UserProfileResponse> => {
    const response = await fetch(`${API_BASE_URL}/user/profile/${userId}`, {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(data),
        credentials: 'include',
    })

    if (!response.ok) {
        const errorData = await response.json().catch(() => ({ 
            success: false, 
            message: 'Failed to update profile' 
        }))
        throw new Error(errorData.message || 'Failed to update profile')
    }

    return response.json()
}

export const updateUserAvailability = async (
    userId: number,
    availability: Record<string, string[]>
): Promise<UserProfileResponse> => {
    const response = await fetch(`${API_BASE_URL}/user/availability/${userId}`, {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({ availability }),
        credentials: 'include',
    })

    if (!response.ok) {
        const errorData = await response.json().catch(() => ({ 
            success: false, 
            message: 'Failed to update availability' 
        }))
        throw new Error(errorData.message || 'Failed to update availability')
    }

    return response.json()
}

export const getUserGameProfiles = async (userId: number): Promise<GameProfileResponse> => {
    const response = await fetch(`${API_BASE_URL}/game-profile/favorites/${userId}`, {
        method: 'GET',
        credentials: 'include',
    })

    if (!response.ok) {
        const errorData = await response.json().catch(() => ({ 
            success: false, 
            message: 'Failed to fetch game profiles' 
        }))
        throw new Error(errorData.message || 'Failed to fetch game profiles')
    }

    return response.json()
}

export const getAllTags = async (): Promise<TagsResponse> => {
    const response = await fetch(`${API_BASE_URL}/tag`, {
        method: 'GET',
        credentials: 'include',
    })

    if (!response.ok) {
        const errorData = await response.json().catch(() => ({ 
            success: false, 
            message: 'Failed to fetch tags' 
        }))
        throw new Error(errorData.message || 'Failed to fetch tags')
    }

    return response.json()
}
