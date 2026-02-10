import { useState, useEffect, useCallback } from "react"
import { getGroupDetails } from "@/api/api"
import type { GroupDetailsDto, GroupMemberDto } from "@/api/types"
import { useChatHub } from "@/hooks/signalr/useChatHub"

export function useGroupDetails(teamId: string | number | null | undefined) {
    const [group, setGroup] = useState<GroupDetailsDto | null>(null)
    const [isLoading, setIsLoading] = useState(false)
    const [error, setError] = useState<string | null>(null)

    const onUserJoined = useCallback((user: GroupMemberDto) => {
        setGroup(prev => {
            if (!prev) return prev;
            if (prev.members.some(m => m.id === user.id)) return prev;
            return {
                ...prev,
                members: [...prev.members, user],
                currentMemberCount: prev.currentMemberCount + 1
            };
        });
    }, []);

    const onUserLeft = useCallback((userId: number) => {
        setGroup(prev => {
            if (!prev) return prev;
            if (!prev.members.some(m => m.id === userId)) return prev;
            return {
                ...prev,
                members: prev.members.filter(m => m.id !== userId),
                currentMemberCount: Math.max(0, prev.currentMemberCount - 1)
            };
        });
    }, []);

    useChatHub({
        onUserJoined,
        onUserLeft
    }, !!teamId);

    const fetchGroupDetails = useCallback(async () => {
        if (!teamId) {
            setGroup(null)
            return
        }

        setIsLoading(true)
        setError(null)

        try {
            const data = await getGroupDetails(teamId.toString())
            setGroup(data)
        } catch (e) {
            console.error("Failed to fetch group info:", e)
            setError(e instanceof Error ? e.message : "Failed to fetch group info")
            setGroup(null)
        } finally {
            setIsLoading(false)
        }
    }, [teamId])

    useEffect(() => {
        fetchGroupDetails()
    }, [teamId, fetchGroupDetails])

    return {
        group,
        isLoading,
        error,
        refetch: fetchGroupDetails
    }
}
