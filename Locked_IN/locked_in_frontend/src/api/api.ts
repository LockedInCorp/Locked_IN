import { apiClient } from '@/api/apiClient.ts';
import * as Types from './types';

// Auth
export async function registerUser(data: Types.RegisterRequest): Promise<Types.UserProfileDto> {
  const formData = new FormData();
  formData.append('username', data.username);
  formData.append('email', data.email);
  formData.append('password', data.password);
  
  if (data.avatar) {
    formData.append('avatar', data.avatar);
  }

  try {
    const response = await apiClient.post<Types.UserProfileDto>('/user/register', formData, {
      headers: {
        'Content-Type': 'multipart/form-data',
      },
    });
    return response.data;
  } catch (error: any) {
    const errorData = error.response?.data;
    throw new Error(errorData?.message || 'Registration failed');
  }
}

export async function loginUser(data: Types.LoginRequest): Promise<Types.UserProfileDto> {
  try {
    const response = await apiClient.post<Types.UserProfileDto>('/user/login', {
      username: data.username,
      password: data.password,
    });
    return response.data;
  } catch (error: any) {
    const errorData = error.response?.data;
    throw new Error(errorData?.message || 'Login failed');
  }
}

export async function logoutUser(): Promise<void> {
  try {
    await apiClient.post('/user/logout');
  } catch (error: any) {
    const errorData = error.response?.data;
    throw new Error(errorData?.message || 'Logout failed');
  }
}

// Profile
export const getUserProfile = async (userId: number): Promise<Types.UserProfileResponse> => {
    try {
        const response = await apiClient.get<Types.UserProfileResponse>(`/user/${userId}`)
        return response.data
    } catch (error: any) {
        throw new Error(error.response?.data?.message || 'Failed to fetch profile')
    }
}

export const updateUserProfile = async (
    data: Types.UpdateProfileRequest
): Promise<Types.UserProfileResponse> => {
    try {
        const formData = new FormData()
        formData.append('username', data.username)
        formData.append('email', data.email)
        
        if (data.avatar instanceof File) {
            formData.append('avatarfile', data.avatar)
        }

        const response = await apiClient.put<Types.UserProfileResponse>(`/user/profile`, formData, {
            headers: {
                'Content-Type': 'multipart/form-data',
            },
        })
        return response.data
    } catch (error: any) {
        throw new Error(error.response?.data?.message || 'Failed to update profile')
    }
}

export const updateUserAvailability = async (
    availability: Record<string, string[]>
): Promise<Types.UserProfileResponse> => {
    try {
        const response = await apiClient.put<Types.UserProfileResponse>(`/user/availability`, { availability })
        return response.data
    } catch (error: any) {
        throw new Error(error.response?.data?.message || 'Failed to update availability')
    }
}

// Games
export const searchGamesByName = async (searchTerm: string): Promise<Types.GameDto[]> => {
    try {
        const response = await apiClient.get<Types.GameDto[]>(`/Game/search?searchTerm=${encodeURIComponent(searchTerm)}`)
        return response.data
    } catch (error: any) {
        const errorData = error.response?.data || { 
            message: 'Failed to search games' 
        }
        throw new Error(errorData.message || 'Failed to search games')
    }
}

export const addGameProfile = async (userId: number, gameId: number): Promise<Types.GameProfileResponse> => {
    try {
        const response = await apiClient.post<Types.GameProfileResponse>(`/game-profile/unfavorite/${userId}/${gameId}`)
        return response.data
    } catch (error: any) {
        const errorData = error.response?.data || { 
            success: false,
            message: 'Failed to add game profile' 
        }
        throw new Error(errorData.message || 'Failed to add game profile')
    }
}

export const getUserGameProfiles = async (userId: number): Promise<Types.GameProfileResponse> => {
    try {
        const response = await apiClient.get<Types.GameProfileResponse>(`/game-profile/favorites/${userId}`)
        return response.data
    } catch (error: any) {
        const errorData = error.response?.data || { 
            success: false, 
            message: 'Failed to fetch game profiles' 
        }
        throw new Error(errorData.message || 'Failed to fetch game profiles')
    }
}


// Tags
export const getExperienceTags = async (): Promise<Types.ExperienceTag[]> => {
    try {
        const response = await apiClient.get<Types.ExperienceTag[]>('/ExperienceTag')
        return response.data
    } catch (error: any) {
        const errorData = error.response?.data || { 
            message: 'Failed to fetch experience tags' 
        }
        throw new Error(errorData.message || 'Failed to fetch experience tags')
    }
}

export const getPreferenceTags = async (): Promise<Types.PreferenceTag[]> => {
    try {
        const response = await apiClient.get<Types.PreferenceTag[]>('/PreferanceTag')
        return response.data
    } catch (error: any) {
        const errorData = error.response?.data || { 
            message: 'Failed to fetch preference tags' 
        }
        throw new Error(errorData.message || 'Failed to fetch preference tags')
    }
}

// Chat
export const getUserChats = async (): Promise<Types.UserChatDto[]> => {
    try {
        const response = await apiClient.get<Types.UserChatDto[]>(`/chat/user`)
        return response.data
    } catch (error: any) {
        throw new Error(error.response?.data?.message || 'Failed to fetch user chats')
    }
}

export const getChatMessages = async (chatId: string): Promise<Types.ChatMessageDto[]> => {
    try {
        const response = await apiClient.get<Types.ChatMessageDto[]>(`/chat/${chatId}/messages`)
        return response.data
    } catch (error: any) {
        throw new Error(error.response?.data?.message || 'Failed to fetch messages')
    }
}

export const getGroupDetails = async (chatId: string): Promise<Types.GroupDetailsDto> => {
    try {
        const response = await apiClient.get<Types.GroupDetailsDto>(`/chat/${chatId}`)
        return response.data
    } catch (error: any) {
        throw new Error(error.response?.data?.message || 'Failed to fetch group details')
    }
}

export const getCommunicationServices = async (): Promise<Types.CommunicationService[]> => {
    try {
        const response = await apiClient.get<Types.CommunicationService[]>('/CommunicationService')
        return response.data
    } catch (error: any) {
        throw new Error(error.response?.data?.message || 'Failed to fetch communication services')
    }
}

export const searchTeamsAdvanced = async (data: Types.TeamSearchRequest): Promise<Types.PagedResult<Types.TeamSearchResult>> => {
    try {
        const response = await apiClient.post<Types.PagedResult<Types.TeamSearchResult>>('/Team/search/advanced', data)
        return response.data
    } catch (error: any) {
        throw new Error(error.response?.data?.message || 'Failed to search teams')
    }
}

export const requestToJoinTeam = async (teamId: number, userId: number): Promise<void> => {
    try {
        await apiClient.post(`/teams/${teamId}/join`, { userId })
    } catch (error: any) {
        throw new Error(error.response?.data?.message || 'Failed to join team')
    }
}

export const cancelJoinRequest = async (teamId: number, userId: number): Promise<void> => {
    try {
        await apiClient.delete(`/teams/${teamId}/cancel-join`, { data: { userId } })
    } catch (error: any) {
        throw new Error(error.response?.data?.message || 'Failed to cancel join request')
    }
}

const createTeam = async (data: Types.CreateGroupRequest): Promise<void> => {
    try {
        const formData = new FormData()
        formData.append('Name', data.name)
        formData.append('GameId', data.gameId.toString())
        formData.append('MaxMembers', data.maxMembers.toString())
        formData.append('IsPrivate', data.isPrivate.toString())
        formData.append('AutoAccept', data.autoAccept.toString())
        formData.append('Experience', data.experience.toString())
        
        if (data.minCompetitiveScore !== undefined) {
            formData.append('MinCompetitiveScore', data.minCompetitiveScore.toString())
        }
        if (data.communicationService !== undefined) {
            formData.append('CommunicationService', data.communicationService.toString())
        }
        if (data.communicationServiceLink) {
            formData.append('CommunicationServiceLink', data.communicationServiceLink)
        }
        if (data.description) {
            formData.append('Description', data.description)
        }
        if (data.previewImage) {
            formData.append('PreviewImage', data.previewImage)
        }
        
        data.tags.forEach(tagId => {
            formData.append('Tags', tagId.toString())
        })

        await apiClient.post('/Team', formData, {
            headers: {
                'Content-Type': 'multipart/form-data',
            },
        })
    } catch (error: any) {
        throw new Error(error.response?.data?.message || 'Failed to create team')
    }
}
export default createTeam
