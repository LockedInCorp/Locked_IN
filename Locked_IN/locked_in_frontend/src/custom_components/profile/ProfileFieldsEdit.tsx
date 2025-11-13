"use client"

import { useState } from "react"
import { X } from "lucide-react"
import { Label } from "@/components/ui/label"
import { Input } from "@/components/ui/input"
import { Textarea } from "@/components/ui/textarea"
import { Button } from "@/components/ui/button"
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select"

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

type ProfileFieldsEditProps = {
    nickname: string
    location: string
    dateOfBirth: string
    favoriteGames: string[]
    aboutMe: string
    onNicknameChange: (value: string) => void
    onLocationChange: (value: string) => void
    onDateOfBirthChange: (value: string) => void
    onFavoriteGamesChange: (games: string[]) => void
    onAboutMeChange: (value: string) => void
}

export default function ProfileFieldsEdit({
    nickname,
    location,
    dateOfBirth,
    favoriteGames,
    aboutMe,
    onNicknameChange,
    onLocationChange,
    onDateOfBirthChange,
    onFavoriteGamesChange,
    onAboutMeChange
}: ProfileFieldsEditProps) {
    const [selectedGame, setSelectedGame] = useState<string>("")
    const [customGame, setCustomGame] = useState<string>("")

    const handleAddGame = (game: string) => {
        const trimmedGame = game.trim()
        if (trimmedGame && !favoriteGames.includes(trimmedGame)) {
            onFavoriteGamesChange([...favoriteGames, trimmedGame])
            setSelectedGame("")
            setCustomGame("")
        }
    }

    const handleRemoveGame = (gameToRemove: string) => {
        onFavoriteGamesChange(favoriteGames.filter(game => game !== gameToRemove))
    }

    const handleCustomGameKeyPress = (e: React.KeyboardEvent<HTMLInputElement>) => {
        if (e.key === "Enter") {
            e.preventDefault()
            handleAddGame(customGame)
        }
    }

    // Filter out already selected games from available games
    const availableGamesFiltered = availableGames.filter(game => !favoriteGames.includes(game))

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

            {/* Favorite Games */}
            <div className="space-y-3">
                <Label className="text-sm text-muted-foreground">Favorite Games</Label>
                
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
                        placeholder="Or enter a custom game name"
                        className="flex-1 border-border bg-card text-foreground placeholder:text-muted-foreground"
                    />
                    <Button
                        type="button"
                        variant="outline"
                        onClick={() => handleAddGame(customGame)}
                        disabled={!customGame.trim() || favoriteGames.includes(customGame.trim())}
                        className="border-border bg-card text-muted-foreground hover:bg-muted hover:text-foreground cursor-pointer disabled:opacity-50 disabled:cursor-not-allowed"
                    >
                        Add
                    </Button>
                </div>

                {/* Tags list with delete buttons */}
                {favoriteGames.length > 0 && (
                    <div className="flex flex-wrap gap-2 mt-3">
                        {favoriteGames.map((game, index) => (
                            <div
                                key={index}
                                className="flex items-center gap-2 rounded-md px-4 py-2 text-sm font-medium bg-primary text-primary-foreground border-2 border-primary shadow-sm"
                            >
                                <span>{game}</span>
                                <button
                                    type="button"
                                    onClick={() => handleRemoveGame(game)}
                                    className="ml-1 hover:bg-primary-foreground/20 rounded-full p-0.5 transition-colors cursor-pointer"
                                    aria-label={`Remove ${game}`}
                                >
                                    <X className="h-3 w-3" />
                                </button>
                            </div>
                        ))}
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

