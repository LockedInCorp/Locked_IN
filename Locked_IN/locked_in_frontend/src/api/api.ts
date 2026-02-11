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

export const createGameProfile = async (data: Types.CreateGameProfileRequest): Promise<Types.GameProfileResponse> => {
    try {
        const response = await apiClient.post<Types.GameProfileResponse>('/game-profile', data);
        return response.data;
    } catch (error: any) {
        throw new Error(error.response?.data || 'Failed to create game profile');
    }
}

export const updateGameProfile = async (profileId: number, data: Types.UpdateGameProfileRequest): Promise<Types.GameProfileResponse> => {
    try {
        const response = await apiClient.put<Types.GameProfileResponse>(`/game-profile/${profileId}`, data);
        return response.data;
    } catch (error: any) {
        throw new Error(error.response?.data || 'Failed to update game profile');
    }
}

export const deleteGameProfile = async (profileId: number): Promise<void> => {
    try {
        await apiClient.delete(`/game-profile/${profileId}`);
    } catch (error: any) {
        throw new Error(error.response?.data || 'Failed to delete game profile');
    }
}

export const getUserGameProfiles = async (userId: number): Promise<Types.GameProfile[]> => {
    try {
        const response = await apiClient.get<Types.GameProfile[]>(`/game-profile/favorites/${userId}`)
        return response.data
    } catch (error: any) {
        console.error("Failed to fetch game profiles", error);
        return [];
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
export const getUserChats = async (searchTerm?: string): Promise<Types.UserChatDto[]> => {
    try {
        const response = await apiClient.get<Types.UserChatDto[]>(`/chat/user`, {
            params: { searchTerm }
        })
        return response.data
    } catch (error: any) {
        throw new Error(error.response?.data?.message || 'Failed to fetch user chats')
    }
}

export const getChatMessages = async (chatId: string, page: number = 1, pageSize: number = 20): Promise<Types.PagedResult<Types.GetMessageDto>> => {
    try {
        const response = await apiClient.get<Types.PagedResult<Types.GetMessageDto>>(`/Message/${chatId}`, {
            params: { pageNumber: page, pageSize }
        })
        return response.data
    } catch (error: any) {
        throw new Error(error.response?.data?.message || 'Failed to fetch messages')
    }
}

export const getGroupDetails = async (teamId: string): Promise<Types.GroupDetailsDto> => {
    try {
        const response = await apiClient.get<Types.GroupDetailsDto>(`/Team/${teamId}`)
        return response.data
    } catch (error: any) {
        throw new Error(error.response?.data?.message || 'Failed to fetch group details')
    }
}

export const getChatDetails = async (id: number): Promise<Types.GetChatDetails> => {
    try {
        const response = await apiClient.get<Types.GetChatDetails>(`/chat/${id}`)
        return response.data
    } catch (error: any) {
        throw new Error(error.response?.data?.message || 'Failed to fetch chat details')
    }
}

export const sendMessage = async (data: Types.SendMessageRequest): Promise<void> => {
    try {
        const formData = new FormData();
        formData.append('chatId', data.chatId.toString());
        formData.append('content', data.content ?? '');
        if (data.attachmentFile instanceof File) {
            formData.append('attachmentFile', data.attachmentFile);
        }
        await apiClient.post(`/Message`, formData, {
            headers: {
                'Content-Type': 'multipart/form-data',
            },
        });
    } catch (error: any) {
        throw new Error(error.response?.data?.message || 'Failed to send message');
    }
}

export const editMessage = async (messageId: number, content: string): Promise<void> => {
    try {
        await apiClient.put(`/Message`, { messageId, content })
    } catch (error: any) {
        throw new Error(error.response?.data?.message || 'Failed to edit message')
    }
}

export const deleteMessage = async (messageId: number): Promise<void> => {
    try {
        await apiClient.delete(`/Message/${messageId}`)
    } catch (error: any) {
        throw new Error(error.response?.data?.message || 'Failed to delete message')
    }
}

export const markChatAsRead = async (chatId: number): Promise<void> => {
    try {
        await apiClient.post(`/chat/${chatId}/read`)
    } catch (error: any) {
        throw new Error(error.response?.data?.message || 'Failed to mark chat as read')
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

export const getTeamJoinRequests = async (teamId: number): Promise<Types.JoinRequestDto[]> => {
    try {
        const response = await apiClient.get<Types.JoinRequestDto[]>(`/teams/${teamId}/join-requests`)
        return response.data
    } catch (error: any) {
        throw new Error(error.response?.data?.message || 'Failed to fetch join requests')
    }
}

export const acceptJoinRequest = async (teamId: number, userId: number): Promise<void> => {
    try {
        await apiClient.put(`/teams/${teamId}/users/${userId}/accept`)
    } catch (error: any) {
        throw new Error(error.response?.data?.message || 'Failed to accept join request')
    }
}

export const declineJoinRequest = async (teamId: number, userId: number): Promise<void> => {
    try {
        await apiClient.put(`/teams/${teamId}/users/${userId}/decline`)
    } catch (error: any) {
        throw new Error(error.response?.data?.message || 'Failed to decline join request')
    }
}

export const leaveTeam = async (teamId: number): Promise<void> => {
    try {
        await apiClient.post(`/teams/${teamId}/leave`)
    } catch (error: any) {
        throw new Error(error.response?.data?.message || 'Failed to leave team')
    }
}

// TODO: Add backend API endpoint for removing a member from a team (e.g. DELETE /teams/{teamId}/members/{userId})
export const removeTeamMember = async (teamId: number, userId: number): Promise<void> => {
    try {
        await apiClient.delete(`/teams/${teamId}/members/${userId}`)
    } catch (error: any) {
        throw new Error(error.response?.data?.message || 'Failed to remove member from team')
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

// Friendship
export const getFriendshipStatus = async (targetUserId: number): Promise<string> => {
    try {
        const response = await apiClient.get<{ status: string }>(`/friendship/status/${targetUserId}`)
        return response.data.status || 'None'
    } catch (error: any) {
        const errorData = error.response?.data || { 
            message: 'Failed to fetch friendship status' 
        }
        throw new Error(errorData.message || 'Failed to fetch friendship status')
    }
}

export const sendFriendRequest = async (receiverId: number): Promise<void> => {
    try {
        await apiClient.post(`/friendship/request`, { receiverId })
    } catch (error: any) {
        const errorData = error.response?.data || { 
            message: 'Failed to send friend request' 
        }
        throw new Error(errorData.message || 'Failed to send friend request')
    }
}

export const cancelFriendRequest = async (friendshipId: number): Promise<void> => {
    try {
        await apiClient.delete(`/friendship/requests/${friendshipId}/cancel`)
    } catch (error: any) {
        const errorData = error.response?.data || { 
            message: 'Failed to cancel friend request' 
        }
        throw new Error(errorData.message || 'Failed to cancel friend request')
    }
}

export const deleteFriendship = async (friendId: number): Promise<void> => {
    try {
        await apiClient.delete(`/friendship/remove/${friendId}`)
    } catch (error: any) {
        const errorData = error.response?.data || { 
            message: 'Failed to delete friendship' 
        }
        throw new Error(errorData.message || 'Failed to delete friendship')
    }
}
