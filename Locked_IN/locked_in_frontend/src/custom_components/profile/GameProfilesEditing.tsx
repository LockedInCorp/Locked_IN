"use client"

import { useState, useEffect } from "react"
import { ChevronDown, ChevronUp, Trash2, Check } from "lucide-react"
import { Label } from "@/components/ui/label"
import { Input } from "@/components/ui/input"
import { Button } from "@/components/ui/button"
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select"
import { RadioGroup, RadioGroupItem } from "@/components/ui/radio-group"
import type { GameProfile } from "./ProfileFields"
import { useGameProfilesStore } from "@/stores/gameProfilesStore"
import { searchGamesByName, getPreferenceTags, getExperienceTags } from "@/api/api"

type GameProfilesEditingProps = {
    gameProfiles: GameProfile[]
    onGameProfilesChange: (profiles: GameProfile[]) => void
    onSave: () => void
    isLoading?: boolean
    errors?: {
        [gameName: string]: {
            inGameNickname?: string
            experience?: string
        }
    }
}

export default function GameProfilesEditing({
    gameProfiles,
    onGameProfilesChange,
    onSave,
    isLoading = false,
    errors = {}
}: GameProfilesEditingProps) {
    const {
        expandedGames,
        selectedGame,
        toggleExpandedGame,
        setExpandedGames,
        setSelectedGame,
        setCustomGame
    } = useGameProfilesStore()

    const [availableGames, setAvailableGames] = useState<string[]>([])
    const [gamePreferences, setGamePreferences] = useState<string[]>([])
    const [experienceLevels, setExperienceLevels] = useState<string[]>([])
    const [isLoadingData, setIsLoadingData] = useState(true)
    const [dataError, setDataError] = useState<string | null>(null)

    useEffect(() => {
        const fetchGamesAndTags = async () => {
            try {
                setIsLoadingData(true)
                setDataError(null)
                
                const [games, preferences, experiences] = await Promise.all([
                    searchGamesByName(""),
                    getPreferenceTags(),
                    getExperienceTags()
                ])
                
                setAvailableGames(games.map(game => game.experiencelevel))
                setGamePreferences(preferences.map(pref => pref.experiencelevel))
                setExperienceLevels(experiences.map(exp => exp.experiencelevel))
            } catch (error) {
                setDataError(error instanceof Error ? error.message : "Failed to load games and tags")
            } finally {
                setIsLoadingData(false)
            }
        }

        fetchGamesAndTags()
    }, [])

    const handleAddGame = (game: string) => {
        const trimmedGame = game.trim()
        if (trimmedGame && !gameProfiles.some(p => p.gameName === trimmedGame)) {
            const newProfile: GameProfile = {
                gameName: trimmedGame,
                preferences: [],
                experience: "",
                inGameNickname: "",
                ranking: "",
                role: ""
            }
            onGameProfilesChange([...gameProfiles, newProfile])
            setCustomGame("")
            setSelectedGame("")
            toggleExpandedGame(trimmedGame)
        }
    }

    const handleRemoveGame = (gameToRemove: string) => {
        onGameProfilesChange(gameProfiles.filter(profile => profile.gameName !== gameToRemove))
        if (expandedGames.has(gameToRemove)) {
            const newExpanded = new Set(expandedGames)
            newExpanded.delete(gameToRemove)
            setExpandedGames(newExpanded)
        }
    }

    const handleUpdateGameProfile = (gameName: string, updates: Partial<GameProfile>) => {
        const updatedProfiles = gameProfiles.map(profile =>
            profile.gameName === gameName ? { ...profile, ...updates } : profile
        )
        onGameProfilesChange(updatedProfiles)
    }

    const togglePreference = (gameName: string, preference: string) => {
        const profile = gameProfiles.find(p => p.gameName === gameName)
        if (!profile) return

        const newPreferences = profile.preferences.includes(preference)
            ? profile.preferences.filter(p => p !== preference)
            : [...profile.preferences, preference]

        handleUpdateGameProfile(gameName, { preferences: newPreferences })
    }

    if (isLoadingData) {
        return (
            <div className="space-y-6">
                <p className="text-sm text-muted-foreground">Loading games and preferences...</p>
            </div>
        )
    }

    if (dataError) {
        return (
            <div className="space-y-6">
                <p className="text-sm text-destructive">Error: {dataError}</p>
                <Button
                    type="button"
                    onClick={() => window.location.reload()}
                    className="bg-primary text-primary-foreground hover:bg-primary/90"
                >
                    Retry
                </Button>
            </div>
        )
    }

    return (
        <div className="space-y-6">
            {/* Title */}
            <h2 className="text-2xl font-bold text-primary">Game Profiles</h2>
            {/* Subtitle */}
            <p className="text-sm text-muted-foreground">Choose games you play.</p>

            {/* Select from available games */}
            <div className="space-y-2">
                <Label className="text-sm text-muted-foreground">Select a game</Label>
                <Select 
                    value={selectedGame} 
                    onValueChange={(value) => {
                        setSelectedGame(value)
                        handleAddGame(value)
                    }}
                >
                    <SelectTrigger className="w-full border-border bg-card text-foreground">
                        <SelectValue placeholder="Select a game to add" />
                    </SelectTrigger>
                    <SelectContent className="border-border bg-card">
                        {availableGames.filter(game => !gameProfiles.some(p => p.gameName === game)).length > 0 ? (
                            availableGames
                                .filter(game => !gameProfiles.some(p => p.gameName === game))
                                .map((game) => (
                                    <SelectItem key={game} value={game} className="text-foreground">
                                        {game}
                                    </SelectItem>
                                ))
                        ) : (
                            <div className="px-2 py-1.5 text-sm text-muted-foreground">
                                All games added
                            </div>
                        )}
                    </SelectContent>
                </Select>
            </div>

            {/* Game profiles list */}
            {gameProfiles.length > 0 && (
                <div className="space-y-2">
                    {gameProfiles.map((profile) => {
                        const isExpanded = expandedGames.has(profile.gameName)
                        return (
                            <div
                                key={profile.gameName}
                                className="rounded-lg border border-border bg-card overflow-hidden"
                            >
                                {/* Collapsed Header */}
                                <div className="flex items-center justify-between px-4 py-3 bg-muted/50">
                                    <span className="text-sm font-semibold text-foreground">{profile.gameName}</span>
                                    <div className="flex items-center gap-2">
                                        <button
                                            type="button"
                                            onClick={() => handleRemoveGame(profile.gameName)}
                                            className="p-1.5 hover:bg-destructive/20 rounded transition-colors cursor-pointer"
                                            aria-label={`Remove ${profile.gameName}`}
                                        >
                                            <Trash2 className="h-4 w-4 text-muted-foreground hover:text-destructive" />
                                        </button>
                                        <button
                                            type="button"
                                            onClick={() => toggleExpandedGame(profile.gameName)}
                                            className="p-1.5 hover:bg-muted rounded transition-colors cursor-pointer"
                                            aria-label={`${isExpanded ? 'Collapse' : 'Expand'} ${profile.gameName}`}
                                        >
                                            {isExpanded ? (
                                                <ChevronUp className="h-4 w-4 text-muted-foreground" />
                                            ) : (
                                                <ChevronDown className="h-4 w-4 text-muted-foreground" />
                                            )}
                                        </button>
                                    </div>
                                </div>

                                {/* Expanded Content */}
                                {isExpanded && (
                                    <div className="px-4 py-4 space-y-4 bg-card">
                                        {/* Gameplay Preferences */}
                                        <div className="space-y-3">
                                            <Label className="text-sm text-muted-foreground">Choose your gameplay preferences</Label>
                                            <div className="flex flex-wrap gap-2">
                                                {gamePreferences.map((pref) => {
                                                    const isSelected = profile.preferences.includes(pref)
                                                    return (
                                                        <button
                                                            key={pref}
                                                            type="button"
                                                            onClick={() => togglePreference(profile.gameName, pref)}
                                                            className={`flex items-center rounded-md px-4 py-2 text-sm font-medium transition-all cursor-pointer ${
                                                                isSelected
                                                                    ? "bg-primary text-primary-foreground border-2 border-primary shadow-sm"
                                                                    : "bg-card text-muted-foreground border-2 border-transparent hover:bg-muted hover:text-foreground hover:border-border"
                                                            }`}
                                                        >
                                                            {isSelected && <Check className="h-3 w-3 mr-1" />}
                                                            {pref}
                                                        </button>
                                                    )
                                                })}
                                            </div>
                                        </div>

                                        {/* Experience */}
                                        <div className="space-y-3">
                                            <Label className="text-sm text-muted-foreground">Choose your game experience</Label>
                                            <RadioGroup
                                                value={profile.experience}
                                                onValueChange={(value) => handleUpdateGameProfile(profile.gameName, { experience: value })}
                                            >
                                                {experienceLevels.map((level) => (
                                                    <div key={level} className="flex items-center gap-2">
                                                        <RadioGroupItem value={level} id={`${profile.gameName}-${level}`} className="border-border" />
                                                        <Label htmlFor={`${profile.gameName}-${level}`} className="cursor-pointer text-sm text-foreground">
                                                            {level}
                                                        </Label>
                                                    </div>
                                                ))}
                                            </RadioGroup>
                                            {errors[profile.gameName]?.experience && (
                                                <p className="text-sm text-destructive">{errors[profile.gameName].experience}</p>
                                            )}
                                        </div>

                                        {/* In-game Nickname */}
                                        <div className="space-y-2">
                                            <Label htmlFor={`${profile.gameName}-nickname`} className="text-sm text-muted-foreground">Enter your in-game nickname</Label>
                                            <Input
                                                id={`${profile.gameName}-nickname`}
                                                type="text"
                                                value={profile.inGameNickname}
                                                onChange={(e) => handleUpdateGameProfile(profile.gameName, { inGameNickname: e.target.value })}
                                                placeholder="Enter your in-game nickname"
                                                className={`border-border bg-card text-foreground placeholder:text-muted-foreground ${errors[profile.gameName]?.inGameNickname ? 'border-destructive' : ''}`}
                                            />
                                            {errors[profile.gameName]?.inGameNickname && (
                                                <p className="text-sm text-destructive">{errors[profile.gameName].inGameNickname}</p>
                                            )}
                                        </div>

                                        {/* Ranking */}
                                        <div className="space-y-2">
                                            <Label htmlFor={`${profile.gameName}-ranking`} className="text-sm text-muted-foreground">Enter your game ranking (optional)</Label>
                                            <Input
                                                id={`${profile.gameName}-ranking`}
                                                type="text"
                                                value={profile.ranking || ""}
                                                onChange={(e) => handleUpdateGameProfile(profile.gameName, { ranking: e.target.value })}
                                                placeholder="0"
                                                className="border-border bg-card text-foreground placeholder:text-muted-foreground"
                                            />
                                        </div>

                                        {/* Role */}
                                        <div className="space-y-2">
                                            <Label htmlFor={`${profile.gameName}-role`} className="text-sm text-muted-foreground">Enter your in-game role (optional)</Label>
                                            <Input
                                                id={`${profile.gameName}-role`}
                                                type="text"
                                                value={profile.role || ""}
                                                onChange={(e) => handleUpdateGameProfile(profile.gameName, { role: e.target.value })}
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

            {/* Save Button */}
            <div className="flex justify-end pt-4">
                <Button
                    type="button"
                    onClick={onSave}
                    disabled={isLoading}
                    className="bg-primary px-8 py-2 text-base font-semibold text-primary-foreground hover:bg-primary/90 cursor-pointer disabled:opacity-50 disabled:cursor-not-allowed"
                >
                    {isLoading ? "Saving..." : "Save"}
                </Button>
            </div>
        </div>
    )
}
