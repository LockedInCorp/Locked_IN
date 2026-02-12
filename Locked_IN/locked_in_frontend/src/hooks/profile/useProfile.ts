import { useState, useEffect, useCallback } from "react"
import { useAuthStore } from "@/stores/authStore"
import { useProfileStore } from "@/stores/profileStore"
import {
    getUserProfile,
    updateUserProfile,
    getUserGameProfiles,
    getPreferenceTags
} from "@/api/api"
import { extractAvatarPath, getImageUrl } from "@/utils/imageUtils"
import { persist } from "@/utils/auth/persistance"
import type { GameProfile } from "@/api/types"

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

            const [profile, gameProfilesData, prefTags] = await Promise.all([
                getUserProfile(userId),
                getUserGameProfiles(userId),
                getPreferenceTags()
            ])

            if (!profile) {
                throw new Error('Failed to load profile')
            }

            let avatarUrl = getImageUrl(extractAvatarPath(profile as any))
            if (!avatarUrl) {
                avatarUrl = user?.avatarUrl
            }

            const nameToId = (prefTags || []).reduce((acc: Record<string, number>, p) => {
                acc[p.name] = p.id
                return acc
            }, {})

            const gameProfiles: GameProfile[] = Array.isArray(gameProfilesData) ? gameProfilesData.map((gp: any) => {
                const prefNames = gp.preferences ?? []
                const prefIds = prefNames
                    .map((name: string) => nameToId[name])
                    .filter((id: number) => id != null)
                return {
                    id: gp.id,
                    gameId: gp.gameId,
                    gameName: gp.gameName || gp.name || "Unknown Game",
                    preferences: prefIds.length > 0 ? prefIds : [],
                    experience: gp.experienceTagId || "",
                    inGameNickname: gp.inGameNickname || "",
                    role: gp.role || "",
                    rank: gp.rank || ""
                }
            }) : [];

            const profileDataToSet = {
                nickname: profile.username,
                gameProfiles,
                avatarUrl: avatarUrl,
                avatarFallback: profile.username.charAt(0).toUpperCase() || "U"
            }

            setProfileData(profileDataToSet)
            if (avatarUrl && user) {
                const updatedUser = { ...user, avatarUrl: avatarUrl };
                setUser(updatedUser);
                persist.setUserData(updatedUser);
            }
        } catch (err) {
            const errorMessage = err instanceof Error ? err.message : 'Failed to load profile'
            setError(errorMessage)
            console.error(err)
        } finally {
            setIsLoading(false)
        }
    }, [user?.id, setProfileData, setUser])

    const saveProfile = useCallback(async () => {
        if (!user?.id) return

        setIsSaving(true)
        setError(null)

        try {
            const updatedProfile = await updateUserProfile({
                username: profileData.nickname,
                email: user.email || '',
                avatar: avatarFile
            })

            if (updatedProfile) {
                const updatedAvatarUrl = getImageUrl(extractAvatarPath(updatedProfile as any))
                const finalAvatarUrl = updatedAvatarUrl || profileData.avatarUrl

                await loadProfile();

                if (user && finalAvatarUrl) {
                    const updatedUser = {
                        ...user,
                        avatarUrl: finalAvatarUrl,
                        nickname: updatedProfile.username
                    }
                    setUser(updatedUser)
                    persist.setUserData(updatedUser)
                }
            }

            setAvatarPreview(null)
            setAvatarFile(null)
            setIsEditing(false)
        } catch (err: any) {
            console.error('Failed to save profile full error:', err);

            let errorMessage = 'Failed to save profile';
            if (err.response && err.response.data) {
                errorMessage = typeof err.response.data === 'string'
                    ? err.response.data
                    : JSON.stringify(err.response.data);
            } else if (err instanceof Error) {
                errorMessage = err.message;
            }

            setError(errorMessage)
        } finally {
            setIsSaving(false)
        }
    }, [
        user?.id, user?.email, profileData, avatarFile,
        setProfileData, setAvatarPreview, setAvatarFile, setIsEditing, loadProfile
    ])

    useEffect(() => {
        if (user?.id) loadProfile()
    }, [user?.id, loadProfile])

    return { isLoading, isSaving, error, availableGames: [], loadProfile, saveProfile }
}