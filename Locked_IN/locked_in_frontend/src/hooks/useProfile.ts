import { useState, useEffect, useCallback } from "react"
import { useAuthStore } from "@/stores/authStore"
import { useProfileStore } from "@/stores/profileStore"
import { 
    getUserProfile, 
    updateUserProfile,
    getUserGameProfiles,
    getAllTags
} from "@/utils/profileApi"
import { extractAvatarFromResponse } from "@/utils/avatarUtils"
import { API_BASE_URL } from "@/lib/api"
import type { GameProfile } from "@/stores/authStore"

export function useProfile() {
    const { user, setUser } = useAuthStore()
    const { 
        profileData, 
        setProfileData,
        setIsEditing,
        setAvatarPreview,
        avatarPreview
    } = useProfileStore()
    
    const [isLoading, setIsLoading] = useState(false)
    const [isSaving, setIsSaving] = useState(false)
    const [error, setError] = useState<string | null>(null)
    const [availableGames, setAvailableGames] = useState<string[]>([])

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
            
            const [profileResponse, gameProfilesResponse, tagsResponse] = await Promise.all([
                getUserProfile(userId),
                getUserGameProfiles(userId).catch(() => ({ success: false, message: '', data: [] })),
                getAllTags().catch(() => ({ success: false, message: '', data: undefined }))
            ])

            if (!profileResponse.success || !profileResponse.data) {
                throw new Error(profileResponse.message || 'Failed to load profile')
            }

            const profile = profileResponse.data
            
            let avatarUrl = await extractAvatarFromResponse(profile as any, API_BASE_URL)
            
            if (!avatarUrl) {
                avatarUrl = user?.avatarUrl
            }
            
            const gameProfiles: GameProfile[] = (gameProfilesResponse.data || []).map(gp => ({
                gameName: gp.gameName,
                preferences: [],
                experience: "",
                inGameNickname: "",
                ranking: gp.rank || "",
                role: ""
            }))

            if (tagsResponse.data?.games) {
                setAvailableGames(tagsResponse.data.games.map(g => g.name))
            }

            const profileDataToSet = {
                nickname: profile.username,
                gameProfiles,
                avatarUrl: avatarUrl,
                avatarFallback: profile.username.charAt(0).toUpperCase() || "U"
            }

            setProfileData(profileDataToSet)
            
            if (avatarUrl && user) {
                setUser({
                    ...user,
                    avatarUrl: avatarUrl
                })
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
                avatar: avatarPreview || undefined
            })

            if (updatedProfile.success && updatedProfile.data) {
                const updatedAvatarUrl = await extractAvatarFromResponse(updatedProfile.data as any, API_BASE_URL)
                
                setProfileData({
                    ...profileData,
                    nickname: updatedProfile.data.username,
                    avatarUrl: updatedAvatarUrl || profileData.avatarUrl,
                    avatarFallback: updatedProfile.data.username.charAt(0).toUpperCase() || "U"
                })
            } else {
                setProfileData({
                    ...profileData,
                    avatarUrl: profileData.avatarUrl
                })
            }

            setAvatarPreview(null)
            setIsEditing(false)
        } catch (err) {
            const errorMessage = err instanceof Error ? err.message : 'Failed to save profile'
            setError(errorMessage)
            throw err
        } finally {
            setIsSaving(false)
        }
    }, [user?.id, user?.email, profileData, avatarPreview, setProfileData, setAvatarPreview, setIsEditing])

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
