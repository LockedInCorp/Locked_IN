import { useMutation, useQueryClient } from "@tanstack/react-query"
import { markChatAsRead } from "@/api/api"
import type { UserChatDto } from "@/api/types"

export function useMarkAsRead() {
    const queryClient = useQueryClient()

    return useMutation({
        mutationFn: (chatId: number) => markChatAsRead(chatId),
        onMutate: async (chatId) => {
            await queryClient.cancelQueries({ queryKey: ["userChats"] })

            const previousChats = queryClient.getQueryData<UserChatDto[]>(["userChats"])

            queryClient.setQueriesData<UserChatDto[]>({ queryKey: ["userChats"] }, (old) => {
                if (!old) return old
                return old.map((chat) => 
                    chat.id === chatId 
                        ? { ...chat, unreadMessageCount: 0 } 
                        : chat
                )
            })

            return { previousChats }
        },
        onError: (_err, _chatId, context) => {
            if (context?.previousChats) {
                queryClient.setQueryData(["userChats"], context.previousChats)
            }
        },
        onSettled: () => {
            queryClient.invalidateQueries({ queryKey: ["userChats"] })
        },
    })
}
