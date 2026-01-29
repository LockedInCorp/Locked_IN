import {apiClient} from "@/lib/apiClient.ts";
export interface UserChatsResponce {
    success: boolean
    message: string
    data?: Array<{
        id: number
        chatName: string
        lastMessageUsername?: string
        lastMessageContent?: string
        lastMessageTime?: Date
        unreadMessagesCount?: number
        chatIconURL?: string
    }>
}
export const getUserChats = async (): Promise<UserChatsResponce> => {
    try {
        const response = await apiClient.get<UserChatsResponce>(`/game-profile/favorites`)
        return response.data
    } catch (error: any) {
        const errorData = error.response?.data || {
            success: false,
            message: 'Failed to fetch user chats'
        }
        throw new Error(errorData.message || 'Failed to fetch user chats')
    }
}