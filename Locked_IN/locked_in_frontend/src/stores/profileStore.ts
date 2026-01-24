import { create } from "zustand"
import type { GameProfile } from "./authStore"

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
    profileDataBeforeEdit: ProfileData
    
    // Profile fields UI state
    expandedGames: Set<string>
    selectedGame: string
    customGame: string
    
    // Actions
    setIsEditing: (isEditing: boolean) => void
    setProfileData: (data: ProfileData) => void
    updateProfileData: (updates: Partial<ProfileData>) => void
    setAvatarPreview: (preview: string | null) => void
    setProfileDataBeforeEdit: (data: ProfileData) => void
    
    // Profile fields UI actions
    setExpandedGames: (games: Set<string>) => void
    toggleExpandedGame: (gameName: string) => void
    setSelectedGame: (game: string) => void
    setCustomGame: (game: string) => void
    
    // Profile actions
    startEditing: () => void
    cancelEditing: () => void
}

const initialProfileData: ProfileData = {
    nickname: "Jan Kowalski",
    gameProfiles: [
        {
            gameName: "Dota 2",
            preferences: ["Competitive", "Strategic"],
            experience: "Experienced",
            inGameNickname: "JanKowalski123",
            ranking: "4500",
            role: "Support"
        }
    ],
    avatarUrl: "/assets/diverse-user-avatars.png",
    avatarFallback: "JK"
}

export const useProfileStore = create<ProfileState>((set, get) => ({
    // Initial state
    isEditing: false,
    profileData: initialProfileData,
    avatarPreview: null,
    profileDataBeforeEdit: initialProfileData,
    
    // Profile fields UI state
    expandedGames: new Set(),
    selectedGame: "",
    customGame: "",
    
    // Basic setters
    setIsEditing: (isEditing) => set({ isEditing }),
    setProfileData: (data) => set({ profileData: data }),
    updateProfileData: (updates) => set((state) => ({
        profileData: { ...state.profileData, ...updates }
    })),
    setAvatarPreview: (preview) => set({ avatarPreview: preview }),
    setProfileDataBeforeEdit: (data) => set({ profileDataBeforeEdit: data }),
    
    // Profile fields UI actions
    setExpandedGames: (games) => set({ expandedGames: games }),
    toggleExpandedGame: (gameName) => {
        const newExpanded = new Set(get().expandedGames)
        if (newExpanded.has(gameName)) {
            newExpanded.delete(gameName)
        } else {
            newExpanded.add(gameName)
        }
        set({ expandedGames: newExpanded })
    },
    setSelectedGame: (game) => set({ selectedGame: game }),
    setCustomGame: (game) => set({ customGame: game }),
    
    // Profile actions
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
            avatarPreview: null
        })
    }
}))

