import { create } from "zustand"
import type { GameProfile } from "@/api/types"

interface ProfileData {
    nickname: string
    gameProfiles: GameProfile[]
    avatarUrl?: string
    avatarFallback: string
}

interface ProfileState {
    isEditing: boolean
    profileData: ProfileData
    avatarPreview: string | null
    avatarFile: File | null
    profileDataBeforeEdit: ProfileData
    
    expandedGames: Set<number>
    selectedGame: string
    customGame: string
    
    setIsEditing: (isEditing: boolean) => void
    setProfileData: (data: ProfileData) => void
    updateProfileData: (updates: Partial<ProfileData>) => void
    setAvatarPreview: (preview: string | null) => void
    setAvatarFile: (file: File | null) => void
    setProfileDataBeforeEdit: (data: ProfileData) => void
    
    setExpandedGames: (games: Set<number>) => void
    toggleExpandedGame: (gameId: number) => void
    setSelectedGame: (game: string) => void
    setCustomGame: (game: string) => void
    
    startEditing: () => void
    cancelEditing: () => void
}

const initialProfileData: ProfileData = {
    nickname: "",
    gameProfiles: [],
    avatarUrl: "/assets/diverse-user-avatars.png",
    avatarFallback: ""
}

export const useProfileStore = create<ProfileState>((set, get) => ({
    isEditing: false,
    profileData: initialProfileData,
    avatarPreview: null,
    avatarFile: null,
    profileDataBeforeEdit: initialProfileData,
    
    expandedGames: new Set(),
    selectedGame: "",
    customGame: "",
    
    setIsEditing: (isEditing) => set({ isEditing }),
    setProfileData: (data) => set({ profileData: data }),
    updateProfileData: (updates) => set((state) => ({
        profileData: { ...state.profileData, ...updates }
    })),
    setAvatarPreview: (preview) => set({ avatarPreview: preview }),
    setAvatarFile: (file) => set({ avatarFile: file }),
    setProfileDataBeforeEdit: (data) => set({ profileDataBeforeEdit: data }),
    
    setExpandedGames: (games) => set({ expandedGames: games }),
    toggleExpandedGame: (gameId) => {
        const newExpanded = new Set(get().expandedGames)
        if (newExpanded.has(gameId)) {
            newExpanded.delete(gameId)
        } else {
            newExpanded.add(gameId)
        }
        set({ expandedGames: newExpanded })
    },
    setSelectedGame: (game) => set({ selectedGame: game }),
    setCustomGame: (game) => set({ customGame: game }),
    
    startEditing: () => {
        const currentData = get().profileData
        set({
            isEditing: true,
            profileDataBeforeEdit: { ...currentData }
        })
    },
    
    cancelEditing: () => {
        const beforeEdit = get().profileDataBeforeEdit
        set({
            isEditing: false,
            profileData: { ...beforeEdit },
            avatarPreview: null,
            avatarFile: null
        })
    }
}))

