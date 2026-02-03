import { useState, useEffect, useCallback } from "react"
import { useAuthStore } from "@/stores/authStore"
import { useProfileStore } from "@/stores/profileStore"
import { 
    getUserProfile, 
    updateUserProfile,
    getUserGameProfiles
} from "@/api/api"
import { extractAvatarFromResponse } from "@/utils/profile/avatarUtils"
import { tokenStorage } from "@/utils/auth/tokenStorage"
import type { GameProfile } from "@/stores/authStore"

export function useProfile() {
    const { user, setUser } = useAuthStore()
    const { 
        profileData, 
        setProfileData,
        setIsEditing,
        setAvatarPreview,
        avatarFile,
        setAvatarFile
    } = useProfileStore()
    
    const [isLoading, setIsLoading] = useState(false)
    const [isSaving, setIsSaving] = useState(false)
    const [error, setError] = useState<string | null>(null)
    const [availableGames] = useState<string[]>([])

    const loadProfile = useCallback(async () => {
        if (!user?.id) {
            setError("User not logged in")
            setIsLoading(false)
            return
        }

        setIsLoading(true)
        setError(null)

        try {
            const userId = parseInt(user.id)
            
            const [profile, gameProfilesResponse] = await Promise.all([
                getUserProfile(userId),
                getUserGameProfiles(userId).catch(() => ({ success: false, message: '', data: [] })),
            ])

            if (!profile) {
                throw new Error('Failed to load profile')
            }
            
            let avatarUrl = await extractAvatarFromResponse(profile as any)
            
            if (!avatarUrl) {
                avatarUrl = user?.avatarUrl
            }
            
            const rawData = gameProfilesResponse.data
            const profilesArray = Array.isArray(rawData) ? rawData : (rawData ? [rawData] : [])
            
            const gameProfiles: GameProfile[] = profilesArray.map(gp => ({
                gameName: gp.gameName,
                preferences: [],
                experience: "",
                inGameNickname: "",
                ranking: gp.rank || "",
                role: ""
            }))


            const profileDataToSet = {
                nickname: profile.username,
                gameProfiles,
                avatarUrl: avatarUrl,
                avatarFallback: profile.username.charAt(0).toUpperCase() || "U"
            }

            setProfileData(profileDataToSet)
            
            if (avatarUrl && user) {
                const updatedUser = {
                    ...user,
                    avatarUrl: avatarUrl
                };
                setUser(updatedUser);
                tokenStorage.setUserData(updatedUser);
            }
        } catch (err) {
            const errorMessage = err instanceof Error ? err.message : 'Failed to load profile'
            setError(errorMessage)
        } finally {
            setIsLoading(false)
        }
    }, [user?.id, setProfileData, setUser])

    const saveProfile = useCallback(async () => {
        if (!user?.id) {
            setError("User not logged in")
            return
        }

        setIsSaving(true)
        setError(null)

        try {
            const updatedProfile = await updateUserProfile({
                username: profileData.nickname,
                email: user.email || '',
                avatar: avatarFile
            })

            if (updatedProfile) {
                const updatedAvatarUrl = await extractAvatarFromResponse(updatedProfile as any)
                
                setProfileData({
                    ...profileData,
                    nickname: updatedProfile.username,
                    avatarUrl: updatedAvatarUrl || profileData.avatarUrl,
                    avatarFallback: updatedProfile.username.charAt(0).toUpperCase() || "U"
                })
            } else {
                setProfileData({
                    ...profileData,
                    avatarUrl: profileData.avatarUrl
                })
            }

            setAvatarPreview(null)
            setAvatarFile(null)
            setIsEditing(false)
        } catch (err) {
            const errorMessage = err instanceof Error ? err.message : 'Failed to save profile'
            setError(errorMessage)
            throw err
        } finally {
            setIsSaving(false)
        }
    }, [user?.id, user?.email, profileData, avatarFile, setProfileData, setAvatarPreview, setAvatarFile, setIsEditing])

    useEffect(() => {
        if (user?.id) {
            loadProfile()
        }
    }, [user?.id, loadProfile])

    return {
        isLoading,
        isSaving,
        error,
        availableGames,
        loadProfile,
        saveProfile
    }
}
