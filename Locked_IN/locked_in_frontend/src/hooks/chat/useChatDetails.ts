import { useState, useEffect, useCallback } from "react"
import { getChatDetails, getChatMessages } from "@/api/api"
import type { GetChatDetails, GetMessageDto } from "@/api/types"
import { useChatHub } from "@/hooks/signalr/useChatHub"
import { useMarkAsRead } from "@/hooks/chat/useMarkAsRead"
import { useQueryClient } from "@tanstack/react-query"

export function useChatDetails(chatId: number | null) {
    const [chatDetails, setChatDetails] = useState<GetChatDetails | null>(null)
    const [messages, setMessages] = useState<GetMessageDto[]>([])
    const [isLoading, setIsLoading] = useState(false)
    const [isFetchingMore, setIsFetchingMore] = useState(false)
    const [error, setError] = useState<string | null>(null)
    const [page, setPage] = useState(1)
    const [hasMore, setHasMore] = useState(true)

    const queryClient = useQueryClient()
    const { mutate: markAsRead } = useMarkAsRead()

    const onReceiveMessage = useCallback((message: GetMessageDto) => {
        if (message.chatId === chatId) {
            setMessages(prev => {
                if (prev.some(m => m.id === message.id)) return prev;
                return [...prev, message];
            });
            markAsRead(chatId);
        }
        queryClient.invalidateQueries({ queryKey: ["userChats"] });
    }, [chatId, queryClient, markAsRead]);

    const onMessageEdited = useCallback((message: GetMessageDto) => {
        if (message.chatId === chatId) {
            setMessages(prev => prev.map(m => m.id === message.id ? message : m));
        }
    }, [chatId]);

    const onMessageDeleted = useCallback((messageId: number) => {
        setMessages(prev => prev.filter(m => m.id !== messageId));
    }, []);

    useChatHub({
        onReceiveMessage,
        onMessageEdited,
        onMessageDeleted
    }, !!chatId);

    const fetchMessages = useCallback(async (pageNum: number, isInitial: boolean = false) => {
        if (!chatId) return
        if (isFetchingMore && !isInitial) return
        
        if (isInitial) setIsLoading(true)
        else setIsFetchingMore(true)

        try {
            const result = await getChatMessages(chatId.toString(), pageNum)
            const items = result?.items || []
            if (isInitial) {
                setMessages(items)
            } else {
                setMessages(prev => [...items, ...prev])
            }
            setHasMore(pageNum < (result?.totalPages || 0))
            setPage(pageNum)

            if (pageNum === 1) {
                markAsRead(chatId)
            }
        } catch (e) {
            console.error(e)
        } finally {
            setIsLoading(false)
            setIsFetchingMore(false)
        }
    }, [chatId, markAsRead])

    const fetchChatDetails = useCallback(async () => {
        if (chatId === null || chatId === undefined) {
            return
        }

        setIsLoading(true)
        setError(null)

        try {
            const details = await getChatDetails(chatId)
            setChatDetails(details)
            // When details are fetched, we also start with page 1 of messages
            await fetchMessages(1, true)
        } catch (err) {
            console.error(`Error loading chat details for chat ${chatId}:`, err)
            const errorMessage = err instanceof Error ? err.message : 'Failed to load chat details'
            setError(errorMessage)
            setChatDetails(null)
        } finally {
            setIsLoading(false)
        }
    }, [chatId, fetchMessages])

    useEffect(() => {
        if (chatId !== null && chatId !== undefined) {
            fetchChatDetails()
        } else {
            setChatDetails(null)
            setMessages([])
            setError(null)
            setPage(1)
            setHasMore(true)
        }
    }, [chatId, fetchChatDetails])

    const fetchMore = useCallback(() => {
        if (hasMore && !isFetchingMore) {
            fetchMessages(page + 1)
        }
    }, [hasMore, isFetchingMore, page, fetchMessages])

    return {
        chatDetails,
        messages,
        isLoading,
        isFetchingMore,
        hasMore,
        page,
        error,
        refetch: fetchChatDetails,
        fetchMore
    }
}
