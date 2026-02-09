import { useState, useEffect, useCallback } from "react"
import { getChatDetails } from "@/api/api"
import type { GetChatDetails } from "@/api/types"

export function useChatDetails(chatId: number | null) {
    const [chatDetails, setChatDetails] = useState<GetChatDetails | null>(null)
    const [isLoading, setIsLoading] = useState(false)
    const [error, setError] = useState<string | null>(null)

    const fetchChatDetails = useCallback(async () => {
        if (chatId === null || chatId === undefined) {
            return
        }

        setIsLoading(true)
        setError(null)

        try {
            const details = await getChatDetails(chatId)
            setChatDetails(details)
        } catch (err) {
            console.error(`Error loading chat details for chat ${chatId}:`, err)
            const errorMessage = err instanceof Error ? err.message : 'Failed to load chat details'
            setError(errorMessage)
            setChatDetails(null)
        } finally {
            setIsLoading(false)
        }
    }, [chatId])

    useEffect(() => {
        if (chatId !== null && chatId !== undefined) {
            fetchChatDetails()
        } else {
            setChatDetails(null)
            setError(null)
        }
    }, [chatId, fetchChatDetails])

    return {
        chatDetails,
        isLoading,
        error,
        refetch: fetchChatDetails
    }
}
