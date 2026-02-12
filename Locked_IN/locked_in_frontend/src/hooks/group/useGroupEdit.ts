import { useState, useEffect, useCallback } from "react"
import { useNavigate } from "react-router-dom"
import { useQueryClient } from "@tanstack/react-query"
import { getGroupDetails, updateTeam } from "@/api/api"
import { useGroupEditStore } from "@/stores/groupEditStore"

export function useGroupEdit(teamId: string | undefined) {
    const navigate = useNavigate()
    const queryClient = useQueryClient()
    const [isLoading, setIsLoading] = useState(false)
    const [isSaving, setIsSaving] = useState(false)
    const [error, setError] = useState<string | null>(null)

    const {
        groupName,
        gameId,
        groupSize,
        isPrivate,
        autoAccept,
        previewImage,
        selectedTags,
        experience,
        competitiveScore,
        communicationService,
        communicationLink,
        description,
        loadTeamData,
        resetForm
    } = useGroupEditStore()

    const loadTeam = useCallback(async () => {
        if (!teamId) return

        setIsLoading(true)
        setError(null)

        try {
            const data = await getGroupDetails(teamId)
            loadTeamData(data)
        } catch (e) {
            const msg = e instanceof Error ? e.message : "Failed to load team"
            setError(msg)
            console.error("Failed to load team:", e)
        } finally {
            setIsLoading(false)
        }
    }, [teamId, loadTeamData])

    useEffect(() => {
        if (teamId) {
            loadTeam()
        }

        return () => {
            resetForm()
        }
    }, [teamId, loadTeam, resetForm])

    const handleSave = useCallback(async () => {
        if (!teamId || !gameId || !groupName || !groupSize || !experience) {
            setError("Please fill in all required fields (Group Name, Game, Group Size, and Experience Level)")
            return
        }
        if (groupName.length > 20) {
            setError("Group name cannot exceed 20 characters")
            return
        }

        setIsSaving(true)
        setError(null)

        try {
            const updated = await updateTeam(parseInt(teamId, 10), {
                name: groupName,
                gameId,
                maxMembers: parseInt(groupSize, 10) || 0,
                isPrivate,
                autoAccept,
                experience,
                tags: selectedTags,
                minCompetitiveScore: parseInt(competitiveScore, 10) || 0,
                communicationService,
                communicationServiceLink: communicationLink,
                description,
                previewImage: previewImage || undefined
            })
            queryClient.invalidateQueries({ queryKey: ["userChats"] })
            if (updated.chatId != null) {
                navigate(`/my-groups/${updated.chatId}`)
            } else {
                navigate("/my-groups")
            }
        } catch (e) {
            const msg = e instanceof Error ? e.message : "Failed to update team"
            setError(msg)
        } finally {
            setIsSaving(false)
        }
    }, [
        teamId,
        gameId,
        groupName,
        groupSize,
        isPrivate,
        experience,
        autoAccept,
        selectedTags,
        competitiveScore,
        communicationService,
        communicationLink,
        description,
        previewImage,
        navigate,
        queryClient
    ])

    return {
        isLoading,
        isSaving,
        error,
        handleSave,
        loadTeam
    }
}
