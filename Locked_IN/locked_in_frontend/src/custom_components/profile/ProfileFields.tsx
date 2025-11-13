"use client"

import { Label } from "@/components/ui/label"

type ProfileFieldsProps = {
    nickname: string
    location: string
    dateOfBirth: string
    favoriteGames: string[]
    aboutMe: string
}

export default function ProfileFields({
    nickname,
    location,
    dateOfBirth,
    favoriteGames,
    aboutMe
}: ProfileFieldsProps) {
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

            {/* Favorite Games */}
            <div className="space-y-3">
                <Label className="text-sm text-muted-foreground">Favorite Games</Label>
                <div className="flex flex-wrap gap-2">
                    {favoriteGames.map((game, index) => (
                        <div
                            key={index}
                            className="flex items-center rounded-md px-4 py-2 text-sm font-medium bg-primary text-primary-foreground border-2 border-primary shadow-sm"
                        >
                            {game}
                        </div>
                    ))}
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

