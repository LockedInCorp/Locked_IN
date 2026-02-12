"use client"

import { ChevronDown, ChevronUp } from "lucide-react"
import { Label } from "@/lib/components/ui/label"
import { Input } from "@/lib/components/ui/input"
import { Button } from "@/lib/components/ui/button"
import { useNavigate } from "react-router-dom"
import type { GameProfile } from "@/api/types"
import { useProfileStore } from "@/stores/profileStore"
import { useEffect, useState } from "react"
import { getExperienceTags, getPreferenceTags } from "@/api/api"
import { Check } from "lucide-react"

type ProfileFieldsEditProps = {
    nickname: string
    gameProfiles: GameProfile[]
    onNicknameChange: (value: string) => void
}

export default function ProfileFieldsEdit({
                                              nickname,
                                              gameProfiles,
                                              onNicknameChange
                                          }: ProfileFieldsEditProps) {
    const navigate = useNavigate()
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
                setPrefsDict(prefs.reduce((acc, p) => ({ ...acc, [p.id]: p.name }), {}))
                setExpDict(exps.reduce((acc, e) => ({ ...acc, [e.id]: e.name }), {}))
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
                <Label htmlFor="nickname" className="text-sm text-muted-foreground">Nickname</Label>
                <Input
                    id="nickname"
                    value={nickname}
                    onChange={(e) => onNicknameChange(e.target.value)}
                    className="border-border bg-card text-foreground placeholder:text-muted-foreground"
                />
            </div>

            {/* Game Profiles - static display */}
            <div className="space-y-3">
                <div className="flex items-center justify-between">
                    <Label className="text-sm text-muted-foreground">Game Profiles</Label>
                    <Button
                        type="button"
                        variant="outline"
                        onClick={() => navigate("/profile/game-profiles")}
                        className="border-border bg-card text-foreground hover:bg-muted cursor-pointer"
                    >
                        Change game profiles
                    </Button>
                </div>

                {gameProfiles.length === 0 ? (
                    <p className="text-sm text-muted-foreground">No games added yet</p>
                ) : (
                    <div className="space-y-2 mt-3">
                        {gameProfiles.map((profile) => {
                            const uniqueKey = profile.id || profile.gameId || Math.random()
                            const toggleKey = profile.gameId ?? 0
                            const isExpanded = expandedGames.has(toggleKey)
                            const gameName = profile.gameName || `Game #${profile.gameId}`

                            return (
                                <div
                                    key={uniqueKey}
                                    className="rounded-lg border border-border bg-card overflow-hidden"
                                >
                                    <button
                                        type="button"
                                        onClick={() => toggleExpandedGame(toggleKey)}
                                        className="w-full flex items-center justify-between px-4 py-3 bg-muted/50 hover:bg-muted transition-colors cursor-pointer"
                                    >
                                        <span className="text-sm font-semibold text-foreground">{gameName}</span>
                                        {isExpanded ? (
                                            <ChevronUp className="h-4 w-4 text-muted-foreground" />
                                        ) : (
                                            <ChevronDown className="h-4 w-4 text-muted-foreground" />
                                        )}
                                    </button>

                                    {isExpanded && (
                                        <div className="px-4 py-4 space-y-4 bg-card">
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

                                            {(profile.experience ?? profile.experienceTagId) && (
                                                <div className="space-y-2">
                                                    <Label className="text-sm text-muted-foreground">Experience</Label>
                                                    <p className="text-sm font-medium text-foreground">
                                                        {typeof profile.experience === 'number'
                                                            ? (expDict[profile.experience] || `Level #${profile.experience}`)
                                                            : (profile.experience ?? (profile.experienceTagId ? (expDict[profile.experienceTagId] || `Level #${profile.experienceTagId}`) : ""))}
                                                    </p>
                                                </div>
                                            )}

                                            {profile.inGameNickname && (
                                                <div className="space-y-2">
                                                    <Label className="text-sm text-muted-foreground">In-game Nickname</Label>
                                                    <p className="text-sm font-medium text-foreground">{profile.inGameNickname}</p>
                                                </div>
                                            )}

                                            {profile.rank && (
                                                <div className="space-y-2">
                                                    <Label className="text-sm text-muted-foreground">Ranking</Label>
                                                    <p className="text-sm font-medium text-foreground">{profile.rank}</p>
                                                </div>
                                            )}

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
                        })}
                    </div>
                )}
            </div>
        </div>
    )
}
