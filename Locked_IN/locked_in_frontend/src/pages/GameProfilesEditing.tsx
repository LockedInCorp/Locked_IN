"use client"

import { useState, useEffect } from "react"
import { useNavigate } from "react-router-dom"
import { useAuthStore } from "@/stores/authStore"
import { useProfileStore } from "@/stores/profileStore"
import GameProfilesEditPanel from "@/components/profile/GameProfilesEditPanel"
import {
    searchGamesByName,
    createGameProfile,
    updateGameProfile,
    deleteGameProfile,
    getExperienceTags,
    getPreferenceTags,
    getUserGameProfiles
} from "@/api/api"
import type { GameProfile } from "@/api/types"

export default function GameProfilesEditingPage() {
    const navigate = useNavigate()
    const { user } = useAuthStore()
    const { profileData, setProfileData } = useProfileStore()

    const [gameProfiles, setGameProfiles] = useState<GameProfile[]>([])
    const [originalProfiles, setOriginalProfiles] = useState<GameProfile[]>([])

    const [isSaving, setIsSaving] = useState(false)
    const [error, setError] = useState<string | null>(null)

    const [gamesList, setGamesList] = useState<{id: number, name: string}[]>([])
    const [expTags, setExpTags] = useState<{id: number, name: string}[]>([])
    const [prefTags, setPrefTags] = useState<{id: number, name: string}[]>([])
    const [tagsLoading, setTagsLoading] = useState(true)
    const [tagsError, setTagsError] = useState<string | null>(null)

    useEffect(() => {
        const fetchData = async () => {
            try {
                setTagsLoading(true)
                setTagsError(null)
                const [gamesData, expData, prefData] = await Promise.all([
                    searchGamesByName(""),
                    getExperienceTags(),
                    getPreferenceTags()
                ])
                setGamesList(gamesData || [])
                setExpTags(expData || [])
                setPrefTags(prefData || [])
            } catch (err) {
                const msg = err instanceof Error ? err.message : "Failed to load metadata"
                setTagsError(msg)
                console.error("Failed to load metadata", err)
            } finally {
                setTagsLoading(false)
            }
        }
        fetchData()
    }, [])

    useEffect(() => {
        if (!user?.id) {
            navigate("/login")
            return
        }

        const idToPrefName = Object.fromEntries(prefTags.map(p => [p.id, p.name]))
        const idToExpName = Object.fromEntries(expTags.map(e => [e.id, e.name]))

        const mappedProfiles: GameProfile[] = profileData.gameProfiles.map(gp => {
            const prefIds = (gp.preferences ?? []).filter((p): p is number => typeof p === 'number')
            const prefNames = prefIds
                .map(id => idToPrefName[id])
                .filter((n): n is string => n != null)
            const expId = typeof gp.experience === 'number' ? gp.experience : (typeof gp.experience === 'string' && /^\d+$/.test(gp.experience) ? parseInt(gp.experience, 10) : gp.experienceTagId)
            const expName = (expId != null && idToExpName[expId]) ? idToExpName[expId] : (typeof gp.experience === 'string' ? gp.experience : "")

            return {
                id: gp.id || (gp as any).gameProfileId,
                gameId: gp.gameId,
                gameName: gp.gameName || `Game`,
                preferences: prefNames.length > 0 ? prefNames : (gp.preferences ?? []),
                experience: expName || (gp.experience ? String(gp.experience) : ""),
                inGameNickname: gp.inGameNickname || "",
                rank: gp.rank ?? "",
                role: gp.role || ""
            }
        })

        setGameProfiles(mappedProfiles)
        setOriginalProfiles(mappedProfiles)
    }, [user, profileData, navigate, expTags, prefTags])

    const handleGameProfilesChange = (profiles: GameProfile[]) => {
        setGameProfiles(profiles)
    }

    const handleSave = async () => {
        if (!user?.id) return

        setIsSaving(true)
        setError(null)

        try {
            const profilesToDelete = originalProfiles.filter(orig =>
                !gameProfiles.some(curr => curr.gameName === orig.gameName)
            );

            for (const profile of profilesToDelete) {
                if (profile.id) {
                    await deleteGameProfile(profile.id);
                }
            }

            for (const profile of gameProfiles) {
                let gameId = profile.gameId;

                if (!gameId) {
                    const gameObj = gamesList.find(g => g.name.toLowerCase() === profile.gameName.toLowerCase())
                    if (gameObj) gameId = gameObj.id;
                }

                if (!gameId) continue;

                const expTagObj = expTags.find(t => t.name === profile.experience)
                const experienceId = expTagObj ? expTagObj.id : (expTags[0]?.id || 1)

                const preferenceTagIds: number[] = [];

                if (profile.preferences && profile.preferences.length > 0) {
                    profile.preferences.forEach(pref => {
                        if (typeof pref === 'number') {
                            preferenceTagIds.push(pref);
                        } else {
                            const prefObj = prefTags.find(p => p.name === String(pref));
                            if (prefObj) preferenceTagIds.push(prefObj.id);
                        }
                    });
                }

                const mainGameExpId = preferenceTagIds.length > 0 ? preferenceTagIds[0] : (prefTags[0]?.id || 1);

                const rankValue = profile.rank ? parseInt(profile.rank) : null;
                const safeRank = (rankValue === null || isNaN(rankValue)) ? null : rankValue;

                const payload = {
                    experienceTagId: experienceId,
                    gameExpId: mainGameExpId,
                    preferenceTagIds: preferenceTagIds,
                    rank: safeRank,
                    isFavorite: true,
                    role: profile.role || "",
                    inGameNickname: profile.inGameNickname || ""
                };

                if (profile.id) {
                    await updateGameProfile(profile.id, payload);
                } else {
                    await createGameProfile({
                        gameId: gameId,
                        ...payload
                    });
                }
            }

            const updatedProfilesResponse = await getUserGameProfiles(Number(user.id))
            const rawData = updatedProfilesResponse;
            const profilesArray = Array.isArray(rawData) ? rawData : []

            const newGameProfiles = profilesArray.map((gp: any) => ({
                id: gp.id,
                gameId: gp.gameId,
                gameName: gp.gameName || gp.game?.name || "Unknown",
                isFavorite: gp.isFavorite,
                rank: gp.rank ? String(gp.rank) : "",

                preferences: gp.preferences && gp.preferences.length > 0
                    ? gp.preferences
                    : (gp.gameExpId ? [gp.gameExpId] : []),

                experience: gp.experienceTagId ?
                    (expTags.find(t => t.id === gp.experienceTagId)?.name || "") : "",
                inGameNickname: gp.inGameNickname || "",
                role: gp.role || ""
            }))

            setProfileData({
                ...profileData,
                gameProfiles: newGameProfiles
            })

            navigate("/profile")
        } catch (error: any) {
            console.error(error);
            const errorMessage = error.response?.data || error.message || "Failed to save game profiles."
            setError(typeof errorMessage === 'string' ? errorMessage : JSON.stringify(errorMessage))
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

                        <GameProfilesEditPanel
                            gameProfiles={gameProfiles}
                            onGameProfilesChange={handleGameProfilesChange}
                            onSave={handleSave}
                            onBack={() => navigate(-1)}
                            isLoading={isSaving}
                            availableGames={gamesList.map(g => g.name)}
                            gamePreferences={prefTags.map(p => p.name)}
                            experienceLevels={expTags.map(e => e.name)}
                            tagsLoading={tagsLoading}
                            tagsError={tagsError}
                        />
                    </div>
                </div>
            </div>
        </div>
    )
}