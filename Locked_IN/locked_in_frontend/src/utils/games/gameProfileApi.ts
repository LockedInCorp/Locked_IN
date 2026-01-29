import { apiClient } from '@/lib/apiClient';

export interface GameDto {
    id: number
    name: string
}

export interface GameProfileResponse {
    success: boolean
    message: string
    data?: {
        id: number
        userId: number
        gameId: number
        gameName: string
        isFavorite: boolean
        rank?: string
    }
}

export const searchGamesByName = async (searchTerm: string): Promise<GameDto[]> => {
    try {
        const response = await apiClient.get<GameDto[]>(`/game/search?searchTerm=${encodeURIComponent(searchTerm)}`)
        return response.data
    } catch (error: any) {
        const errorData = error.response?.data || { 
            message: 'Failed to search games' 
        }
        throw new Error(errorData.message || 'Failed to search games')
    }
}

export const addGameProfile = async (userId: number, gameId: number): Promise<GameProfileResponse> => {
    try {
        const response = await apiClient.post<GameProfileResponse>(`/game-profile/unfavorite/${userId}/${gameId}`)
        return response.data
    } catch (error: any) {
        const errorData = error.response?.data || { 
            success: false,
            message: 'Failed to add game profile' 
        }
        throw new Error(errorData.message || 'Failed to add game profile')
    }
}
