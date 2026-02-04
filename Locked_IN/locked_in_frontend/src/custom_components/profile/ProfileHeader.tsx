"use client"

import { useRef } from "react"
import { Upload } from "lucide-react"
import { Avatar, AvatarFallback, AvatarImage } from "@/components/ui/avatar"
import { Button } from "@/components/ui/button"
import { getImageUrl } from "@/utils/imageUtils"

type ProfileHeaderProps = {
    avatarUrl?: string
    avatarFallback?: string
    isEditing?: boolean
    onAvatarChange?: (file: File | null) => void
}

export default function ProfileHeader({ 
    avatarUrl = "/assets/diverse-user-avatars.png",
    avatarFallback = "U",
    isEditing = false,
    onAvatarChange
}: ProfileHeaderProps) {
    const fileInputRef = useRef<HTMLInputElement>(null)

    const handleFileSelect = (event: React.ChangeEvent<HTMLInputElement>) => {
        const file = event.target.files?.[0]
        if (file && onAvatarChange) {
            onAvatarChange(file)
        }
    }

    const handleUploadClick = () => {
        fileInputRef.current?.click()
    }

    return (
        <div className={`flex items-center gap-4 ${isEditing ? 'mb-12' : 'mb-8'}`}>
            <div className={`relative ${isEditing ? 'pb-3' : ''}`}>
                <Avatar className="h-24 w-24">
                    <AvatarImage src={getImageUrl(avatarUrl)} />
                    <AvatarFallback>{avatarFallback}</AvatarFallback>
                </Avatar>
                {isEditing && (
                    <>
                        <input
                            ref={fileInputRef}
                            type="file"
                            accept="image/*"
                            onChange={handleFileSelect}
                            className="hidden"
                            aria-label="Upload profile picture"
                        />
                        <Button
                            type="button"
                            variant="outline"
                            size="sm"
                            onClick={handleUploadClick}
                            className="absolute top-full left-1/2 -translate-x-1/2 mt-1 border-border bg-card text-muted-foreground hover:bg-muted hover:text-foreground cursor-pointer"
                        >
                            <Upload className="mr-1 size-3" />
                            Upload
                        </Button>
                    </>
                )}
            </div>
            <h2 className="text-2xl font-semibold text-foreground">My Profile</h2>
        </div>
    )
}

