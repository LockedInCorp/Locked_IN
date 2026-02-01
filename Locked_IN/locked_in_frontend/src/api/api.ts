import { apiClient } from '@/lib/apiClient';
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
    userId: number,
    availability: Record<string, string[]>
): Promise<Types.UserProfileResponse> => {
    try {
        const response = await apiClient.put<Types.UserProfileResponse>(`/user/availability/${userId}`, { availability })
        return response.data
    } catch (error: any) {
        throw new Error(error.response?.data?.message || 'Failed to update availability')
    }
}

// Games
export const searchGamesByName = async (searchTerm: string): Promise<Types.GameDto[]> => {
    try {
        const response = await apiClient.get<Types.GameDto[]>(`/game/search?searchTerm=${encodeURIComponent(searchTerm)}`)
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
export const getAllTags = async (): Promise<Types.TagsResponse> => {
    try {
        const response = await apiClient.get<Types.TagsResponse>('/tag')
        return response.data
    } catch (error: any) {
        const errorData = error.response?.data || { 
            success: false, 
            message: 'Failed to fetch tags' 
        }
        throw new Error(errorData.message || 'Failed to fetch tags')
    }
}

// Chat
export const getUserChats = async (): Promise<Types.UserChatDto[]> => {
    try {
        const response = await apiClient.get<Types.UserChatDto[]>(`/chat/user-chats`)
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
        const response = await apiClient.get<Types.GroupDetailsDto>(`/chat/${chatId}/group-details`)
        return response.data
    } catch (error: any) {
        throw new Error(error.response?.data?.message || 'Failed to fetch group details')
    }
}
