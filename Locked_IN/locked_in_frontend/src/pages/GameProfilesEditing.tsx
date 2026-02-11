"use client"

import { useState, useEffect } from "react"
import { useNavigate } from "react-router-dom"
import { useAuthStore } from "@/stores/authStore"
import { useProfileStore } from "@/stores/profileStore"
import GameProfilesEditing from "@/custom_components/profile/GameProfilesEditing"
import {
    searchGamesByName,
    createGameProfile,
    updateGameProfile,
    deleteGameProfile,
    getExperienceTags,
    getPreferenceTags,
    getUserGameProfiles
} from "@/api/api"
import type { GameProfile } from "@/custom_components/profile/ProfileFields"

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

    useEffect(() => {
        const fetchData = async () => {
            try {
                const [gamesData, expData, prefData] = await Promise.all([
                    searchGamesByName(""),
                    getExperienceTags(),
                    getPreferenceTags()
                ])
                setGamesList(gamesData || []);
                setExpTags(expData || []);
                setPrefTags(prefData || []);
            } catch (err) {
                console.error("Failed to load metadata", err)
            }
        }
        fetchData()
    }, [])

    useEffect(() => {
        if (!user?.id) {
            navigate("/login")
            return
        }

        const mappedProfiles: GameProfile[] = profileData.gameProfiles.map(gp => ({
            id: gp.id || (gp as any).gameProfileId,
            gameId: gp.gameId,
            gameName: gp.gameName || `Game`,
            preferences: gp.preferences || [],
            experience: gp.experience ? String(gp.experience) : "",
            inGameNickname: gp.inGameNickname || "",
            ranking: gp.ranking || "",
            role: gp.role || ""
        }))

        setGameProfiles(mappedProfiles)
        setOriginalProfiles(mappedProfiles)
    }, [user, profileData, navigate])

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

                let preferenceTagIds: number[] = [];

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

                const rankValue = profile.ranking ? parseInt(profile.ranking) : null;
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
                ranking: gp.rank ? String(gp.rank) : "",

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