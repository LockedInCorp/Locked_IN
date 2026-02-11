import { useState, useEffect, useCallback } from "react"
import { useAuthStore } from "@/stores/authStore"
import { useProfileStore } from "@/stores/profileStore"
import {
    getUserProfile,
    updateUserProfile,
    getUserGameProfiles,
    createGameProfile,
    updateGameProfile,
    deleteGameProfile
} from "@/api/api"
import { extractAvatarPath, getImageUrl } from "@/utils/imageUtils"
import { persist } from "@/utils/auth/persistance"
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

    const [originalGameProfiles, setOriginalGameProfiles] = useState<GameProfile[]>([])

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

            const [profile, gameProfilesData] = await Promise.all([
                getUserProfile(userId),
                getUserGameProfiles(userId)
            ])

            if (!profile) {
                throw new Error('Failed to load profile')
            }

            let avatarUrl = getImageUrl(extractAvatarPath(profile as any))
            if (!avatarUrl) {
                avatarUrl = user?.avatarUrl
            }

            const gameProfiles: GameProfile[] = Array.isArray(gameProfilesData) ? gameProfilesData.map((gp: any) => ({
                id: gp.id,
                gameId: gp.gameId,
                gameName: gp.gameName || gp.name || "Unknown Game",

                preferences: gp.gameExpId ? [gp.gameExpId] : [],
                experience: gp.experienceTagId || "",

                inGameNickname: gp.inGameNickname || "",
                role: gp.role || "",

                ranking: gp.rank || ""
            })) : [];

            const profileDataToSet = {
                nickname: profile.username,
                gameProfiles,
                avatarUrl: avatarUrl,
                avatarFallback: profile.username.charAt(0).toUpperCase() || "U"
            }

            setProfileData(profileDataToSet)
            setOriginalGameProfiles(gameProfiles)
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

            const currentGameProfiles = profileData.gameProfiles;

            const profilesToDelete = originalGameProfiles.filter(orig =>
                !currentGameProfiles.some(curr => curr.id === orig.id)
            );

            for (const p of profilesToDelete) {
                if (p.id) {
                    console.log(`Deleting profile ${p.id}`);
                    await deleteGameProfile(p.id);
                }
            }

            for (const profile of currentGameProfiles) {

                let experienceId = 1;
                if (profile.experience) {
                    const parsed = Number(profile.experience);
                    if (!isNaN(parsed) && parsed > 0) experienceId = parsed;
                }

                let gameExpId = 1;
                if (profile.preferences && profile.preferences.length > 0) {
                    const parsed = Number(profile.preferences[0]);
                    if (!isNaN(parsed) && parsed > 0) gameExpId = parsed;
                }

                let safeRank: number | undefined = undefined;
                if (profile.ranking) {
                    const parsedRank = parseInt(profile.ranking);
                    if (!isNaN(parsedRank)) safeRank = parsedRank;
                }

                const payload = {
                    experienceTagId: experienceId,
                    gameExpId: gameExpId,
                    rank: safeRank,
                    isFavorite: true,
                    role: profile.role || null,
                    inGameNickname: profile.inGameNickname || null
                };

                if (profile.id) {
                    console.log(`Updating profile ${profile.id}`, payload);
                    await updateGameProfile(profile.id, payload);
                } else if (profile.gameId) {
                    console.log(`Creating profile for game ${profile.gameId}`, payload);
                    await createGameProfile({
                        gameId: profile.gameId,
                        ...payload
                    });
                }
            }

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
        user?.id, user?.email, profileData, avatarFile, originalGameProfiles,
        setProfileData, setAvatarPreview, setAvatarFile, setIsEditing, loadProfile
    ])

    useEffect(() => {
        if (user?.id) loadProfile()
    }, [user?.id, loadProfile])

    return { isLoading, isSaving, error, availableGames: [], loadProfile, saveProfile }
}