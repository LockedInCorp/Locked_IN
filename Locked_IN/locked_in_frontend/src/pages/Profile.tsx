"use client"

import { Button } from "@/components/ui/button"
import ProfileHeader from "@/custom_components/profile/ProfileHeader"
import ProfileFields from "@/custom_components/profile/ProfileFields"
import ProfileFieldsEdit from "@/custom_components/profile/ProfileFieldsEdit"
import { useProfileStore } from "@/stores/profileStore"
import { useLogout } from "@/hooks/useLogout"

export default function Profile() {
    const {
        isEditing,
        profileData,
        avatarPreview,
        setIsEditing,
        setAvatarPreview,
        startEditing,
        cancelEditing,
        saveProfile,
        updateProfileData
    } = useProfileStore()
    
    const logoutMutation = useLogout()

    const handleAvatarChange = (file: File | null) => {
        if (file) {
            const reader = new FileReader()
            reader.onloadend = () => {
                setAvatarPreview(reader.result as string)
            }
            reader.readAsDataURL(file)
            
            // TODO: Upload file to server and get URL
        } else {
            setAvatarPreview(null)
        }
    }

    const handleSave = async () => {
        await saveProfile()
    }

    const handleCancel = () => {
        cancelEditing()
    }

    const handleLogout = () => {
        logoutMutation.mutate()
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
                                onNicknameChange={(value) => updateProfileData({ nickname: value })}
                                onLocationChange={(value) => updateProfileData({ location: value })}
                                onDateOfBirthChange={(value) => updateProfileData({ dateOfBirth: value })}
                                onGameProfilesChange={(profiles) => updateProfileData({ gameProfiles: profiles })}
                                onAboutMeChange={(value) => updateProfileData({ aboutMe: value })}
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
                        <div className="mt-8 flex justify-between items-center">
                            <Button 
                                variant="outline"
                                onClick={handleLogout}
                                disabled={logoutMutation.isPending}
                                className="px-6 py-2 text-base font-semibold border-destructive text-destructive hover:bg-destructive/10 cursor-pointer disabled:opacity-50 disabled:cursor-not-allowed"
                            >
                                {logoutMutation.isPending ? "Logging out..." : "Logout"}
                            </Button>
                            
                            <div className="flex gap-3">
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
                                        onClick={startEditing}
                                    >
                                        Edit profile
                                    </Button>
                                )}
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    )
}

