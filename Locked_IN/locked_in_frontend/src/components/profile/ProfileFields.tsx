"use client"

import { ChevronDown, ChevronUp } from "lucide-react"
import { Label } from "@/lib/components/ui/label"
import { Check } from "lucide-react"
import { useProfileStore } from "@/stores/profileStore"
import { useEffect, useState } from "react"
import { getExperienceTags, getPreferenceTags, searchGamesByName } from "@/api/api"

export type GameProfile = {
    gameId: number
    preferences: number[]
    experience: number
    inGameNickname: string
    ranking?: string
    role?: string
}

type ProfileFieldsProps = {
    nickname: string
    gameProfiles: GameProfile[]
}

export default function ProfileFields({
    nickname,
    gameProfiles
}: ProfileFieldsProps) {
    const { expandedGames, toggleExpandedGame } = useProfileStore()
    const [gamesDict, setGamesDict] = useState<Record<number, string>>({})
    const [prefsDict, setPrefsDict] = useState<Record<number, string>>({})
    const [expDict, setExpDict] = useState<Record<number, string>>({})

    useEffect(() => {
        const fetchData = async () => {
            try {
                const [prefs, exps] = await Promise.all([
                    getPreferenceTags(),
                    getExperienceTags()
                ])
                
                const prefsObj = prefs.reduce((acc, p) => ({ ...acc, [p.id]: p.experiencelevel }), {})
                const expsObj = exps.reduce((acc, e) => ({ ...acc, [e.id]: e.experiencelevel }), {})
                
                setPrefsDict(prefsObj)
                setExpDict(expsObj)

                // Fetch game names for the profiles we have
                const uniqueGameIds = Array.from(new Set(gameProfiles.map(p => p.gameId)))
                // Note: The API doesn't seem to have a getGameById, only search. 
                // We might need to handle this differently if we don't have game names initially.
                // For now, let's assume we can at least show the IDs if names aren't loaded, 
                // or try to find them if they were cached.
                // If the game profiles already came with names from the backend (Dota 2 example), 
                // we might want to include gameName in GameProfile type too for convenience.
            } catch (error) {
                console.error("Failed to fetch tags", error)
            }
        }
        fetchData()
    }, [gameProfiles])

    return (
        <div className="space-y-6">
            {/* Nickname */}
            <div className="space-y-2">
                <Label className="text-sm text-muted-foreground">Nickname</Label>
                <p className="text-base font-semibold text-foreground">{nickname}</p>
            </div>

            {/* Game Profiles */}
            <div className="space-y-3">
                <Label className="text-sm text-muted-foreground">Game Profiles</Label>
                <div className="space-y-2">
                    {gameProfiles.length === 0 ? (
                        <p className="text-sm text-muted-foreground">No games added yet</p>
                    ) : (
                        gameProfiles.map((profile) => {
                            const isExpanded = expandedGames.has(profile.gameId)
                            const gameName = gamesDict[profile.gameId] || `Game #${profile.gameId}`
                            return (
                                <div
                                    key={profile.gameId}
                                    className="rounded-lg border border-border bg-card overflow-hidden"
                                >
                                    {/* Collapsed Header */}
                                    <button
                                        onClick={() => toggleExpandedGame(profile.gameId)}
                                        className="w-full flex items-center justify-between px-4 py-3 bg-muted/50 hover:bg-muted transition-colors cursor-pointer"
                                    >
                                        <span className="text-sm font-semibold text-foreground">{gameName}</span>
                                        <div className="flex items-center gap-2">
                                            {isExpanded ? (
                                                <ChevronUp className="h-4 w-4 text-muted-foreground" />
                                            ) : (
                                                <ChevronDown className="h-4 w-4 text-muted-foreground" />
                                            )}
                                        </div>
                                    </button>

                                    {/* Expanded Content */}
                                    {isExpanded && (
                                        <div className="px-4 py-4 space-y-4 bg-card">
                                            {/* Gameplay Preferences */}
                                            {profile.preferences.length > 0 && (
                                                <div className="space-y-2">
                                                    <Label className="text-sm text-muted-foreground">Gameplay Preferences</Label>
                                                    <div className="flex flex-wrap gap-2">
                                                        {profile.preferences.map((prefId) => (
                                                            <div
                                                                key={prefId}
                                                                className="flex items-center rounded-md px-3 py-1.5 text-sm font-medium bg-primary text-primary-foreground border-2 border-primary shadow-sm"
                                                            >
                                                                <Check className="h-3 w-3 mr-1" />
                                                                {prefsDict[prefId] || `Pref #${prefId}`}
                                                            </div>
                                                        ))}
                                                    </div>
                                                </div>
                                            )}

                                            {/* Experience */}
                                            {profile.experience && (
                                                <div className="space-y-2">
                                                    <Label className="text-sm text-muted-foreground">Experience</Label>
                                                    <p className="text-sm font-medium text-foreground capitalize">{expDict[profile.experience] || `Level #${profile.experience}`}</p>
                                                </div>
                                            )}

                                            {/* In-game Nickname */}
                                            {profile.inGameNickname && (
                                                <div className="space-y-2">
                                                    <Label className="text-sm text-muted-foreground">In-game Nickname</Label>
                                                    <p className="text-sm font-medium text-foreground">{profile.inGameNickname}</p>
                                                </div>
                                            )}

                                            {/* Ranking */}
                                            {profile.ranking && (
                                                <div className="space-y-2">
                                                    <Label className="text-sm text-muted-foreground">Ranking</Label>
                                                    <p className="text-sm font-medium text-foreground">{profile.ranking}</p>
                                                </div>
                                            )}

                                            {/* Role */}
                                            {profile.role && (
                                                <div className="space-y-2">
                                                    <Label className="text-sm text-muted-foreground">Role</Label>
                                                    <p className="text-sm font-medium text-foreground">{profile.role}</p>
                                                </div>
                                            )}
                                        </div>
                                    )}
                                </div>
                            )
                        })
                    )}
                </div>
            </div>
        </div>
    )
}

