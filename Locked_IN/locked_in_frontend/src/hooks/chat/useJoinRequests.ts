import { useState, useEffect, useCallback } from "react"
import { getTeamJoinRequests } from "@/api/api"
import type {JoinRequestDto, TeamJoinResponceDto} from "@/api/types"
import { useJoinRequestHub } from "@/hooks/signalr/useJoinRequestHub"

export function useJoinRequests(teamId: number | null | undefined, enabled: boolean = true) {
    const [requests, setRequests] = useState<JoinRequestDto[]>([])
    const [isLoading, setIsLoading] = useState(false)
    const [error, setError] = useState<string | null>(null)

    const handleNewJoinRequest = useCallback((data: TeamJoinResponceDto) => {
        if (data.teamId === teamId) {
            setRequests(prev => {
                if (prev.some(r => r.userId === data.userId)) return prev;
                
                const newRequest: JoinRequestDto = {
                    teamId: data.teamId,
                    userId: data.userId,
                    username: data.username,
                    avatarUrl: data.avatarUrl,
                    requestTimestamp: data.requestTimestamp
                };
                return [newRequest, ...prev];
            });
        }
    }, [teamId]);

    const handleCanceledJoinRequest = useCallback((userId: number, canceledTeamId: number) => {
        if (canceledTeamId === teamId) {
            setRequests(prev => prev.filter(r => r.userId !== userId));
        }
    }, [teamId]);

    useJoinRequestHub(handleNewJoinRequest, handleCanceledJoinRequest, enabled && !!teamId);

    const fetchJoinRequests = useCallback(async () => {
        if (!teamId || !enabled) {
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
    }, [teamId, enabled])

    useEffect(() => {
        if (enabled) {
            fetchJoinRequests()
        } else {
            setRequests([])
        }
    }, [teamId, fetchJoinRequests, enabled])

    return {
        requests,
        isLoading,
        error,
        refetch: fetchJoinRequests
    }
}
