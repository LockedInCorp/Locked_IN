const API_BASE_URL = 'http://localhost:5122/api'

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
    const response = await fetch(`${API_BASE_URL}/game/search?searchTerm=${encodeURIComponent(searchTerm)}`, {
        method: 'GET',
        credentials: 'include',
    })

    if (!response.ok) {
        const errorData = await response.json().catch(() => ({ 
            message: 'Failed to search games' 
        }))
        throw new Error(errorData.message || 'Failed to search games')
    }

    return response.json()
}

export const addGameProfile = async (userId: number, gameId: number): Promise<GameProfileResponse> => {
    const response = await fetch(`${API_BASE_URL}/game-profile/unfavorite/${userId}/${gameId}`, {
        method: 'POST',
        credentials: 'include',
    })

    if (!response.ok) {
        const errorData = await response.json().catch(() => ({ 
            success: false,
            message: 'Failed to add game profile' 
        }))
        throw new Error(errorData.message || 'Failed to add game profile')
    }

    return response.json()
}
