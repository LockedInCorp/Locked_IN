"use client"

import { ChevronDown, ChevronUp } from "lucide-react"
import { Label } from "@/lib/components/ui/label"
import { Check } from "lucide-react"
import { useProfileStore } from "@/stores/profileStore"
import { useEffect, useState } from "react"
import { getExperienceTags, getPreferenceTags } from "@/api/api"
import type { GameProfile } from "@/api/types"

type ProfileFieldsProps = {
    nickname: string
    gameProfiles: GameProfile[]
}

export default function ProfileFields({
                                          nickname,
                                          gameProfiles
                                      }: ProfileFieldsProps) {
    const { expandedGames, toggleExpandedGame } = useProfileStore()
    const [prefsDict, setPrefsDict] = useState<Record<number, string>>({})
    const [expDict, setExpDict] = useState<Record<number, string>>({})

    useEffect(() => {
        const fetchData = async () => {
            try {
                const [prefs, exps] = await Promise.all([
                    getPreferenceTags(),
                    getExperienceTags()
                ])

                const prefsObj = prefs.reduce((acc, p) => ({ ...acc, [p.id]: p.name }), {})
                const expsObj = exps.reduce((acc, e) => ({ ...acc, [e.id]: e.name }), {})

                setPrefsDict(prefsObj)
                setExpDict(expsObj)
            } catch (error) {
                console.error("Failed to fetch tags", error)
            }
        }
        fetchData()
    }, [])

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
                            const uniqueKey = profile.id || profile.gameId || Math.random();
                            const toggleKey = profile.gameId || 0;
                            const isExpanded = expandedGames.has(toggleKey)

                            const gameName = profile.gameName || `Game #${profile.gameId}`

                            return (
                                <div
                                    key={uniqueKey}
                                    className="rounded-lg border border-border bg-card overflow-hidden"
                                >
                                    {/* Collapsed Header */}
                                    <button
                                        onClick={() => toggleExpandedGame(toggleKey)}
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
                                            {(profile.preferences?.length ?? 0) > 0 && (
                                                <div className="space-y-2">
                                                    <Label className="text-sm text-muted-foreground">Gameplay Preferences</Label>
                                                    <div className="flex flex-wrap gap-2">
                                                        {(profile.preferences ?? []).map((prefId) => (
                                                            <div
                                                                key={prefId}
                                                                className="flex items-center rounded-md px-3 py-1.5 text-sm font-medium bg-primary text-primary-foreground border-2 border-primary shadow-sm"
                                                            >
                                                                <Check className="h-3 w-3 mr-1" />
                                                                {typeof prefId === 'number' ? (prefsDict[prefId] || `Pref #${prefId}`) : prefId}
                                                            </div>
                                                        ))}
                                                    </div>
                                                </div>
                                            )}

                                            {/* Experience */}
                                            {(profile.experience ?? profile.experienceTagId) && (
                                                <div className="space-y-2">
                                                    <Label className="text-sm text-muted-foreground">Experience</Label>
                                                    <p className="text-sm font-medium text-foreground capitalize">
                                                        {typeof profile.experience === 'number'
                                                            ? (expDict[profile.experience] || `Level #${profile.experience}`)
                                                            : (profile.experience ?? (profile.experienceTagId ? (expDict[profile.experienceTagId] || `Level #${profile.experienceTagId}`) : ""))}
                                                    </p>
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
                                            {profile.rank && (
                                                <div className="space-y-2">
                                                    <Label className="text-sm text-muted-foreground">Ranking</Label>
                                                    <p className="text-sm font-medium text-foreground">{profile.rank}</p>
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