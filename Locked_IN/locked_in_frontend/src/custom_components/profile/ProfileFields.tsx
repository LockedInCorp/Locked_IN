"use client"

import { ChevronDown, ChevronUp } from "lucide-react"
import { Label } from "@/components/ui/label"
import { Check } from "lucide-react"
import { useProfileStore } from "@/stores/profileStore"

export type GameProfile = {
    gameName: string
    preferences: string[] // e.g., "Chill", "Competitive", "Roleplay", "Strategic", "Hardcore"
    experience: string // "Beginner", "Experienced", "Professional"
    inGameNickname: string
    ranking?: string
    role?: string
}

type ProfileFieldsProps = {
    nickname: string
    location: string
    dateOfBirth: string
    gameProfiles: GameProfile[]
    aboutMe: string
}

export default function ProfileFields({
    nickname,
    location,
    dateOfBirth,
    gameProfiles,
    aboutMe
}: ProfileFieldsProps) {
    const { expandedGames, toggleExpandedGame } = useProfileStore()

    return (
        <div className="space-y-6">
            {/* Nickname */}
            <div className="space-y-2">
                <Label className="text-sm text-muted-foreground">Nickname</Label>
                <p className="text-base font-semibold text-foreground">{nickname}</p>
            </div>

            {/* Location */}
            <div className="space-y-2">
                <Label className="text-sm text-muted-foreground">Location</Label>
                <p className="text-base font-semibold text-foreground">{location}</p>
            </div>

            {/* Date of birth */}
            <div className="space-y-2">
                <Label className="text-sm text-muted-foreground">Date of birth</Label>
                <p className="text-base font-semibold text-foreground">{dateOfBirth}</p>
            </div>

            {/* Game Profiles */}
            <div className="space-y-3">
                <Label className="text-sm text-muted-foreground">Game Profiles</Label>
                <div className="space-y-2">
                    {gameProfiles.length === 0 ? (
                        <p className="text-sm text-muted-foreground">No games added yet</p>
                    ) : (
                        gameProfiles.map((profile) => {
                            const isExpanded = expandedGames.has(profile.gameName)
                            return (
                                <div
                                    key={profile.gameName}
                                    className="rounded-lg border border-border bg-card overflow-hidden"
                                >
                                    {/* Collapsed Header */}
                                    <button
                                        onClick={() => toggleExpandedGame(profile.gameName)}
                                        className="w-full flex items-center justify-between px-4 py-3 bg-muted/50 hover:bg-muted transition-colors cursor-pointer"
                                    >
                                        <span className="text-sm font-semibold text-foreground">{profile.gameName}</span>
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
                                                        {profile.preferences.map((pref) => (
                                                            <div
                                                                key={pref}
                                                                className="flex items-center rounded-md px-3 py-1.5 text-sm font-medium bg-primary text-primary-foreground border-2 border-primary shadow-sm"
                                                            >
                                                                <Check className="h-3 w-3 mr-1" />
                                                                {pref}
                                                            </div>
                                                        ))}
                                                    </div>
                                                </div>
                                            )}

                                            {/* Experience */}
                                            {profile.experience && (
                                                <div className="space-y-2">
                                                    <Label className="text-sm text-muted-foreground">Experience</Label>
                                                    <p className="text-sm font-medium text-foreground capitalize">{profile.experience}</p>
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

            {/* About me */}
            <div className="space-y-2">
                <Label className="text-sm text-muted-foreground">About me</Label>
                <p className="text-base text-foreground whitespace-pre-wrap">{aboutMe}</p>
            </div>
        </div>
    )
}

