"use client"

import { ChevronDown, ChevronUp, Trash2, Check } from "lucide-react"
import { Label } from "@/components/ui/label"
import { Input } from "@/components/ui/input"
import { Button } from "@/components/ui/button"
import { RadioGroup, RadioGroupItem } from "@/components/ui/radio-group"
import { useNavigate } from "react-router-dom"
import type { GameProfile } from "./ProfileFields"
import { useProfileStore } from "@/stores/profileStore"
import { useEffect, useState } from "react"
import { getExperienceTags, getPreferenceTags } from "@/api/api"

type ProfileFieldsEditProps = {
    nickname: string
    gameProfiles: GameProfile[]
    onNicknameChange: (value: string) => void
    onGameProfilesChange: (profiles: GameProfile[]) => void
}

export default function ProfileFieldsEdit({
                                              nickname,
                                              gameProfiles,
                                              onNicknameChange,
                                              onGameProfilesChange
                                          }: ProfileFieldsEditProps) {
    const navigate = useNavigate()
    const {
        expandedGames,
        toggleExpandedGame,
        setExpandedGames
    } = useProfileStore()

    const [gamePreferencesDict, setGamePreferencesDict] = useState<Record<number, string>>({})
    const [experienceLevelsDict, setExperienceLevelsDict] = useState<Record<number, string>>({})

    useEffect(() => {
        const fetchTags = async () => {
            try {
                const [prefs, exps] = await Promise.all([
                    getPreferenceTags(),
                    getExperienceTags()
                ])
                setGamePreferencesDict(prefs.reduce((acc, p) => ({ ...acc, [p.id]: p.experiencelevel }), {}))
                setExperienceLevelsDict(exps.reduce((acc, e) => ({ ...acc, [e.id]: e.experiencelevel }), {}))
            } catch (error) {
                console.error("Failed to fetch tags", error)
            }
        }
        fetchTags()
    }, [])

    const handleRemoveGame = (gameId: number) => {
        onGameProfilesChange(gameProfiles.filter(profile => profile.gameId !== gameId))
        if (expandedGames.has(gameId)) {
            const newExpanded = new Set(expandedGames)
            newExpanded.delete(gameId)
            setExpandedGames(newExpanded)
        }
    }

    const handleUpdateGameProfile = (gameId: number, updates: Partial<GameProfile>) => {
        onGameProfilesChange(
            gameProfiles.map(profile =>
                profile.gameId === gameId ? { ...profile, ...updates } : profile
            )
        )
    }

    const togglePreference = (gameId: number, preferenceId: number) => {
        const profile = gameProfiles.find(p => p.gameId === gameId)
        if (!profile) return

        const newPreferences = profile.preferences.includes(preferenceId)
            ? profile.preferences.filter(p => p !== preferenceId)
            : [...profile.preferences, preferenceId]

        handleUpdateGameProfile(gameId, { preferences: newPreferences })
    }

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

            {/* Game Profiles */}
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

                {/* Game profiles list */}
                {gameProfiles.length > 0 && (
                    <div className="space-y-2 mt-3">
                        {gameProfiles.map((profile) => {
                            const isExpanded = expandedGames.has(profile.gameId!)
                            const gameName = profile.gameName || `Game #${profile.gameId}`

                            return (
                                <div
                                    key={profile.id || profile.gameId}
                                    className="rounded-lg border border-border bg-card overflow-hidden"
                                >
                                    {/* Collapsed Header */}
                                    <div className="flex items-center justify-between px-4 py-3 bg-muted/50">
                                        <button
                                            onClick={() => toggleExpandedGame(profile.gameId!)}
                                            className="flex-1 flex items-center justify-between cursor-pointer"
                                        >
                                            <span className="text-sm font-semibold text-foreground">{gameName}</span>
                                            {isExpanded ? (
                                                <ChevronUp className="h-4 w-4 text-muted-foreground" />
                                            ) : (
                                                <ChevronDown className="h-4 w-4 text-muted-foreground" />
                                            )}
                                        </button>
                                        <button
                                            type="button"
                                            onClick={() => handleRemoveGame(profile.gameId!)}
                                            className="ml-2 p-1.5 hover:bg-destructive/20 rounded transition-colors cursor-pointer"
                                            aria-label={`Remove ${gameName}`}
                                        >
                                            <Trash2 className="h-4 w-4 text-muted-foreground hover:text-destructive" />
                                        </button>
                                    </div>

                                    {/* Expanded Content */}
                                    {isExpanded && (
                                        <div className="px-4 py-4 space-y-4 bg-card">
                                            {/* Gameplay Preferences */}
                                            <div className="space-y-3">
                                                <Label className="text-sm text-muted-foreground">Choose your gameplay preferences</Label>
                                                <div className="flex flex-wrap gap-2">
                                                    {Object.entries(gamePreferencesDict).map(([idStr, name]) => {
                                                        const id = Number(idStr)
                                                        const isSelected = profile.preferences.includes(id)
                                                        return (
                                                            <button
                                                                key={id}
                                                                type="button"
                                                                onClick={() => togglePreference(profile.gameId!, id)}
                                                                className={`flex items-center rounded-md px-4 py-2 text-sm font-medium transition-all cursor-pointer ${
                                                                    isSelected
                                                                        ? "bg-primary text-primary-foreground border-2 border-primary shadow-sm"
                                                                        : "bg-card text-muted-foreground border-2 border-transparent hover:bg-muted hover:text-foreground hover:border-border"
                                                                }`}
                                                            >
                                                                {isSelected && <Check className="h-3 w-3 mr-1" />}
                                                                {name}
                                                            </button>
                                                        )
                                                    })}
                                                </div>
                                            </div>

                                            {/* Experience */}
                                            <div className="space-y-3">
                                                <Label className="text-sm text-muted-foreground">Choose your game experience</Label>
                                                <RadioGroup
                                                    value={profile.experience.toString()}
                                                    onValueChange={(value) => handleUpdateGameProfile(profile.gameId!, { experience: Number(value) })}
                                                >
                                                    {Object.entries(experienceLevelsDict).map(([idStr, name]) => (
                                                        <div key={idStr} className="flex items-center gap-2">
                                                            <RadioGroupItem value={idStr} id={`${profile.gameId}-${idStr}`} className="border-border" />
                                                            <Label htmlFor={`${profile.gameId}-${idStr}`} className="cursor-pointer text-sm text-foreground">
                                                                {name}
                                                            </Label>
                                                        </div>
                                                    ))}
                                                </RadioGroup>
                                            </div>

                                            {/* In-game Nickname */}
                                            <div className="space-y-2">
                                                <Label htmlFor={`${profile.gameId}-nickname`} className="text-sm text-muted-foreground">Enter your in-game nickname</Label>
                                                <Input
                                                    id={`${profile.gameId}-nickname`}
                                                    type="text"
                                                    value={profile.inGameNickname}
                                                    onChange={(e) => handleUpdateGameProfile(profile.gameId!, { inGameNickname: e.target.value })}
                                                    placeholder="Enter your in-game nickname"
                                                    className="border-border bg-card text-foreground placeholder:text-muted-foreground"
                                                />
                                            </div>

                                            {/* Ranking */}
                                            <div className="space-y-2">
                                                <Label htmlFor={`${profile.gameId}-ranking`} className="text-sm text-muted-foreground">Enter your game ranking (optional)</Label>
                                                <Input
                                                    id={`${profile.gameId}-ranking`}
                                                    type="text"
                                                    value={profile.ranking || ""}
                                                    onChange={(e) => handleUpdateGameProfile(profile.gameId!, { ranking: e.target.value })}
                                                    placeholder="0"
                                                    className="border-border bg-card text-foreground placeholder:text-muted-foreground"
                                                />
                                            </div>

                                            {/* Role */}
                                            <div className="space-y-2">
                                                <Label htmlFor={`${profile.gameId}-role`} className="text-sm text-muted-foreground">Enter your in-game role (optional)</Label>
                                                <Input
                                                    id={`${profile.gameId}-role`}
                                                    type="text"
                                                    value={profile.role || ""}
                                                    onChange={(e) => handleUpdateGameProfile(profile.gameId!, { role: e.target.value })}
                                                    placeholder="e.g. Tank, Damager"
                                                    className="border-border bg-card text-foreground placeholder:text-muted-foreground"
                                                />
                                            </div>
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