"use client"

import { ChevronDown, ChevronUp, Trash2, Check } from "lucide-react"
import { Label } from "@/components/ui/label"
import { Input } from "@/components/ui/input"
import { Textarea } from "@/components/ui/textarea"
import { Button } from "@/components/ui/button"
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select"
import { RadioGroup, RadioGroupItem } from "@/components/ui/radio-group"
import type { GameProfile } from "./ProfileFields"
import { useProfileStore } from "@/stores/profileStore"

// Sample list of games - replace with actual API call later
const availableGames = [
    "Dota 2",
    "Counter-Strike 2",
    "League of Legends",
    "Valorant",
    "Apex Legends",
    "Fortnite",
    "Overwatch 2",
    "Rocket League",
    "Minecraft",
    "Terraria",
    "World of Warcraft",
    "Final Fantasy XIV",
    "Elden Ring",
    "The Witcher 3",
    "Cyberpunk 2077"
]

const gamePreferences = ["Chill", "Competitive", "Roleplay", "Strategic", "Hardcore"]
const experienceLevels = ["Beginner", "Experienced", "Professional"]

type ProfileFieldsEditProps = {
    nickname: string
    location: string
    dateOfBirth: string
    gameProfiles: GameProfile[]
    aboutMe: string
    onNicknameChange: (value: string) => void
    onLocationChange: (value: string) => void
    onDateOfBirthChange: (value: string) => void
    onGameProfilesChange: (profiles: GameProfile[]) => void
    onAboutMeChange: (value: string) => void
}

export default function ProfileFieldsEdit({
    nickname,
    location,
    dateOfBirth,
    gameProfiles,
    aboutMe,
    onNicknameChange,
    onLocationChange,
    onDateOfBirthChange,
    onGameProfilesChange,
    onAboutMeChange
}: ProfileFieldsEditProps) {
    const {
        expandedGames,
        selectedGame,
        customGame,
        toggleExpandedGame,
        setExpandedGames,
        setSelectedGame,
        setCustomGame
    } = useProfileStore()

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
            setSelectedGame("")
            setCustomGame("")
            // Auto-expand the newly added game
            toggleExpandedGame(trimmedGame)
        }
    }

    const handleRemoveGame = (gameToRemove: string) => {
        onGameProfilesChange(gameProfiles.filter(profile => profile.gameName !== gameToRemove))
        // Remove from expandedGames if it exists there
        if (expandedGames.has(gameToRemove)) {
            const newExpanded = new Set(expandedGames)
            newExpanded.delete(gameToRemove)
            setExpandedGames(newExpanded)
        }
    }

    const handleUpdateGameProfile = (gameName: string, updates: Partial<GameProfile>) => {
        onGameProfilesChange(
            gameProfiles.map(profile =>
                profile.gameName === gameName ? { ...profile, ...updates } : profile
            )
        )
    }

    const togglePreference = (gameName: string, preference: string) => {
        const profile = gameProfiles.find(p => p.gameName === gameName)
        if (!profile) return

        const newPreferences = profile.preferences.includes(preference)
            ? profile.preferences.filter(p => p !== preference)
            : [...profile.preferences, preference]

        handleUpdateGameProfile(gameName, { preferences: newPreferences })
    }

    const handleCustomGameKeyPress = (e: React.KeyboardEvent<HTMLInputElement>) => {
        if (e.key === "Enter") {
            e.preventDefault()
            handleAddGame(customGame)
        }
    }

    // Filter out already selected games from available games
    const availableGamesFiltered = availableGames.filter(game => !gameProfiles.some(p => p.gameName === game))

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

            {/* Location */}
            <div className="space-y-2">
                <Label htmlFor="location" className="text-sm text-muted-foreground">Location (optional)</Label>
                <Input
                    id="location"
                    value={location}
                    onChange={(e) => onLocationChange(e.target.value)}
                    placeholder="Enter your location"
                    className="border-border bg-card text-foreground placeholder:text-muted-foreground"
                />
            </div>

            {/* Date of birth */}
            <div className="space-y-2">
                <Label htmlFor="date-of-birth" className="text-sm text-muted-foreground">Date of birth (optional)</Label>
                <Input
                    id="date-of-birth"
                    type="text"
                    value={dateOfBirth}
                    onChange={(e) => onDateOfBirthChange(e.target.value)}
                    placeholder="DD.MM.YYYY"
                    className="border-border bg-card text-foreground placeholder:text-muted-foreground"
                />
            </div>

            {/* Game Profiles */}
            <div className="space-y-3">
                <Label className="text-sm text-muted-foreground">Game Profiles</Label>
                
                {/* Add new game section */}
                <div className="space-y-2">
                    {/* Dropdown to select games */}
                    <Select value={selectedGame} onValueChange={(value) => {
                        setSelectedGame(value)
                        handleAddGame(value)
                    }}>
                        <SelectTrigger className="w-full border-border bg-card text-foreground">
                            <SelectValue placeholder="Select a game to add" />
                        </SelectTrigger>
                        <SelectContent className="border-border bg-card">
                            {availableGamesFiltered.length > 0 ? (
                                availableGamesFiltered.map((game) => (
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

                    {/* Custom game input */}
                    <div className="flex gap-2">
                        <Input
                            type="text"
                            value={customGame}
                            onChange={(e) => setCustomGame(e.target.value)}
                            onKeyPress={handleCustomGameKeyPress}
                            placeholder="ex. Terraria, Overwatch"
                            className="flex-1 border-border bg-card text-foreground placeholder:text-muted-foreground"
                        />
                        <Button
                            type="button"
                            variant="outline"
                            onClick={() => handleAddGame(customGame)}
                            disabled={!customGame.trim() || gameProfiles.some(p => p.gameName === customGame.trim())}
                            className="border-border bg-card text-muted-foreground hover:bg-muted hover:text-foreground cursor-pointer disabled:opacity-50 disabled:cursor-not-allowed"
                        >
                            Add
                        </Button>
                    </div>
                </div>

                {/* Game profiles list */}
                {gameProfiles.length > 0 && (
                    <div className="space-y-2 mt-3">
                        {gameProfiles.map((profile) => {
                            const isExpanded = expandedGames.has(profile.gameName)
                            return (
                                <div
                                    key={profile.gameName}
                                    className="rounded-lg border border-border bg-card overflow-hidden"
                                >
                                    {/* Collapsed Header */}
                                    <div className="flex items-center justify-between px-4 py-3 bg-muted/50">
                                        <button
                                            onClick={() => toggleExpandedGame(profile.gameName)}
                                            className="flex-1 flex items-center justify-between cursor-pointer"
                                        >
                                            <span className="text-sm font-semibold text-foreground">{profile.gameName}</span>
                                            {isExpanded ? (
                                                <ChevronUp className="h-4 w-4 text-muted-foreground" />
                                            ) : (
                                                <ChevronDown className="h-4 w-4 text-muted-foreground" />
                                            )}
                                        </button>
                                        <button
                                            type="button"
                                            onClick={() => handleRemoveGame(profile.gameName)}
                                            className="ml-2 p-1.5 hover:bg-destructive/20 rounded transition-colors cursor-pointer"
                                            aria-label={`Remove ${profile.gameName}`}
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
                                                    className="border-border bg-card text-foreground placeholder:text-muted-foreground"
                                                />
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
            </div>

            {/* About me */}
            <div className="space-y-2">
                <Label htmlFor="about-me" className="text-sm text-muted-foreground">About me (optional)</Label>
                <Textarea
                    id="about-me"
                    value={aboutMe}
                    onChange={(e) => onAboutMeChange(e.target.value)}
                    placeholder="Tell us about yourself"
                    className="min-h-32 resize-none border-border bg-card text-foreground placeholder:text-muted-foreground"
                />
            </div>
        </div>
    )
}

