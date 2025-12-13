"use client"

import { useState } from "react"
import { Button } from "@/components/ui/button"
import ProfileHeader from "@/custom_components/profile/ProfileHeader"
import ProfileFields from "@/custom_components/profile/ProfileFields"
import ProfileFieldsEdit from "@/custom_components/profile/ProfileFieldsEdit"
import type { GameProfile } from "@/custom_components/profile/ProfileFields"

export default function Profile() {
    // Sample data - will be replaced with actual data later
    const initialProfileData = {
        nickname: "Jan Kowalski",
        location: "Warsaw, Poland",
        dateOfBirth: "01.01.2001",
        gameProfiles: [
            {
                gameName: "Dota 2",
                preferences: ["Competitive", "Strategic"],
                experience: "Experienced",
                inGameNickname: "JanKowalski123",
                ranking: "4500",
                role: "Support"
            }
        ] as GameProfile[],
        aboutMe: "I like pilaying Dota 2!",
        avatarUrl: "/assets/diverse-user-avatars.png",
        avatarFallback: "JK"
    }

    const [isEditing, setIsEditing] = useState(false)
    const [profileData, setProfileData] = useState(initialProfileData)
    const [avatarPreview, setAvatarPreview] = useState<string | null>(null)
    const [profileDataBeforeEdit, setProfileDataBeforeEdit] = useState(initialProfileData)

    const handleAvatarChange = (file: File | null) => {
        if (file) {
            // Create a preview URL for the selected image
            const reader = new FileReader()
            reader.onloadend = () => {
                setAvatarPreview(reader.result as string)
            }
            reader.readAsDataURL(file)
            
            // TODO: Upload file to server and get URL
            // For now, we'll use the preview URL
        } else {
            setAvatarPreview(null)
        }
    }

    const handleSave = () => {
        // TODO: Implement save functionality (API call)
        // Include avatarPreview or uploaded file URL
        console.log("Saving profile:", {
            ...profileData,
            avatarUrl: avatarPreview || profileData.avatarUrl
        })
        
        // Update avatar URL if preview exists
        if (avatarPreview) {
            setProfileData({ ...profileData, avatarUrl: avatarPreview })
        }
        
        setAvatarPreview(null)
        setIsEditing(false)
    }

    const handleCancel = () => {
        // Reset to state before entering edit mode
        setProfileData(profileDataBeforeEdit)
        setAvatarPreview(null)
        setIsEditing(false)
    }

    return (
        <div className="relative w-full min-h-full overflow-y-auto bg-background">
            {/* Main content */}
            <div className="relative z-10 min-h-full flex items-center justify-center px-6 py-12">
                <div className="w-full max-w-3xl">
                    {/* Profile Card */}
                    <div className="rounded-lg border border-border bg-card p-8 shadow-lg">
                        <ProfileHeader 
                            avatarUrl={avatarPreview || profileData.avatarUrl}
                            avatarFallback={profileData.avatarFallback}
                            isEditing={isEditing}
                            onAvatarChange={handleAvatarChange}
                        />

                        {isEditing ? (
                            <ProfileFieldsEdit
                                nickname={profileData.nickname}
                                location={profileData.location}
                                dateOfBirth={profileData.dateOfBirth}
                                gameProfiles={profileData.gameProfiles}
                                aboutMe={profileData.aboutMe}
                                onNicknameChange={(value) => setProfileData({ ...profileData, nickname: value })}
                                onLocationChange={(value) => setProfileData({ ...profileData, location: value })}
                                onDateOfBirthChange={(value) => setProfileData({ ...profileData, dateOfBirth: value })}
                                onGameProfilesChange={(profiles) => setProfileData({ ...profileData, gameProfiles: profiles })}
                                onAboutMeChange={(value) => setProfileData({ ...profileData, aboutMe: value })}
                            />
                        ) : (
                            <ProfileFields
                                nickname={profileData.nickname}
                                location={profileData.location}
                                dateOfBirth={profileData.dateOfBirth}
                                gameProfiles={profileData.gameProfiles}
                                aboutMe={profileData.aboutMe}
                            />
                        )}

                        {/* Action Buttons */}
                        <div className="mt-8 flex justify-end gap-3">
                            {isEditing ? (
                                <>
                                    <Button 
                                        variant="outline"
                                        onClick={handleCancel}
                                        className="px-6 py-2 text-base font-semibold border-border hover:bg-muted cursor-pointer"
                                    >
                                        Cancel
                                    </Button>
                                    <Button 
                                        className="bg-primary px-6 py-2 text-base font-semibold text-primary-foreground hover:bg-primary/90 cursor-pointer"
                                        onClick={handleSave}
                                    >
                                        Save changes
                                    </Button>
                                </>
                            ) : (
                                <Button 
                                    className="bg-primary px-6 py-2 text-base font-semibold text-primary-foreground hover:bg-primary/90 cursor-pointer"
                                    onClick={() => {
                                        // Save current state before entering edit mode
                                        setProfileDataBeforeEdit(profileData)
                                        setIsEditing(true)
                                    }}
                                >
                                    Edit profile
                                </Button>
                            )}
                        </div>
                    </div>
                </div>
            </div>
        </div>
    )
}

