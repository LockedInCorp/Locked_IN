import { useState, useEffect, useCallback } from "react"
import { getGroupDetails } from "@/api/api"
import type { GroupDetailsDto } from "@/api/types"

export function useGroupDetails(teamId: string | number | null | undefined) {
    const [group, setGroup] = useState<GroupDetailsDto | null>(null)
    const [isLoading, setIsLoading] = useState(false)
    const [error, setError] = useState<string | null>(null)

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
