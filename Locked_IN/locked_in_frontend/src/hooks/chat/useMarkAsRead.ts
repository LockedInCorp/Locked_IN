import { useMutation, useQueryClient } from "@tanstack/react-query"
import { markChatAsRead } from "@/api/api"
import type { UserChatDto } from "@/api/types"

export function useMarkAsRead() {
    const queryClient = useQueryClient()

    return useMutation({
        mutationFn: (chatId: number) => markChatAsRead(chatId),
        onMutate: async (chatId) => {
            // Cancel any outgoing refetches (so they don't overwrite our optimistic update)
            await queryClient.cancelQueries({ queryKey: ["userChats"] })

            // Snapshot the previous value
            const previousChats = queryClient.getQueryData<UserChatDto[]>(["userChats"])

            // Optimistically update to the new value
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
            // Always refetch after error or success to ensure we are in sync with server
            queryClient.invalidateQueries({ queryKey: ["userChats"] })
        },
    })
}
