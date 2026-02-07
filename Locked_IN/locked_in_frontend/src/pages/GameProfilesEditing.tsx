"use client"

import { useState, useEffect } from "react"
import { useNavigate } from "react-router-dom"
import { useAuthStore } from "@/stores/authStore"
import { useProfileStore } from "@/stores/profileStore"
import GameProfilesEditing from "@/custom_components/profile/GameProfilesEditing"
import { searchGamesByName, addGameProfile } from "@/api/api"
import type { GameProfile } from "@/custom_components/profile/ProfileFields"

export default function GameProfilesEditingPage() {
    const navigate = useNavigate()
    const { user } = useAuthStore()
    const { profileData, updateProfileData } = useProfileStore()
    
    const [gameProfiles, setGameProfiles] = useState<GameProfile[]>([])
    const [isSaving, setIsSaving] = useState(false)
    const [error, setError] = useState<string | null>(null)

    useEffect(() => {
        if (!user?.id) {
            navigate("/login")
            return
        }

        setGameProfiles(profileData.gameProfiles.map(gp => ({
            gameName: `Game #${gp.gameId}`,
            preferences: [],
            experience: "",
            inGameNickname: gp.inGameNickname || "",
            ranking: gp.ranking || "",
            role: gp.role || ""
        })))
    }, [user, profileData, navigate])

    const handleGameProfilesChange = (profiles: GameProfile[]) => {
        setGameProfiles(profiles)
    }

    const handleSave = async () => {
        if (!user?.id) {
            setError("User not logged in")
            return
        }

        setIsSaving(true)
        setError(null)

        try {
            const allGames = await searchGamesByName('')
            
            for (const profile of gameProfiles) {
                const game = allGames.find(g => g.name.toLowerCase() === profile.gameName.toLowerCase())
                
                if (!game) {
                    console.warn(`Game "${profile.gameName}" not found in database, skipping...`)
                    continue
                }

                try {
                    await addGameProfile(Number(user.id), game.id)
                } catch (error) {
                    console.error(`Failed to add game profile for "${profile.gameName}":`, error)
                }
            }

            navigate("/profile")
        } catch (error) {
            const errorMessage = error instanceof Error ? error.message : "Failed to save game profiles. Please try again."
            setError(errorMessage)
        } finally {
            setIsSaving(false)
        }
    }

    return (
        <div className="relative w-full min-h-full overflow-y-auto bg-background">
            <div className="relative z-10 min-h-full flex items-center justify-center px-6 py-12">
                <div className="w-full max-w-3xl">
                    <div className="rounded-lg border border-border bg-card p-8 shadow-lg">
                        {error && (
                            <div className="mb-4 p-3 rounded-md bg-destructive/10 border border-destructive text-destructive text-sm">
                                {error}
                            </div>
                        )}

                        <GameProfilesEditing
                            gameProfiles={gameProfiles}
                            onGameProfilesChange={handleGameProfilesChange}
                            onSave={handleSave}
                            isLoading={isSaving}
                        />
                    </div>
                </div>
            </div>
        </div>
    )
}
