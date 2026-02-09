import { useState, useEffect, useCallback } from "react"
import { getTeamJoinRequests } from "@/api/api"
import type {JoinRequestDto} from "@/api/types"

export function useJoinRequests(teamId: number | null | undefined) {
    const [requests, setRequests] = useState<JoinRequestDto[]>([])
    const [isLoading, setIsLoading] = useState(false)
    const [error, setError] = useState<string | null>(null)

    const fetchJoinRequests = useCallback(async () => {
        if (!teamId) {
            setRequests([])
            return
        }

        setIsLoading(true)
        setError(null)

        try {
            const data = await getTeamJoinRequests(teamId)
            setRequests(data)
        } catch (e) {
            console.error("Failed to fetch join requests:", e)
            setError(e instanceof Error ? e.message : "Failed to fetch join requests")
            setRequests([])
        } finally {
            setIsLoading(false)
        }
    }, [teamId])

    useEffect(() => {
        fetchJoinRequests()
    }, [teamId, fetchJoinRequests])

    return {
        requests,
        isLoading,
        error,
        refetch: fetchJoinRequests
    }
}
