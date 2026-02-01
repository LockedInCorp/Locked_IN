import { create } from "zustand"
import { loginUser } from "@/api/api"

export type GameProfile = {
    gameName: string
    preferences: string[]
    experience: string
    inGameNickname: string
    ranking?: string
    role?: string
}

interface AuthState {
    // Login state
    loginEmail: string
    loginPassword: string
    
    // Register state
    registerStep: 1 | 2
    registerEmail: string
    registerNickname: string
    registerPassword: string
    registerRepeatPassword: string
    registerAvatarFile: File | null
    registerAvatarPreview: string | null
    registerGameProfiles: GameProfile[]
    
    // Session state
    isLoggedIn: boolean
    user: {
        id: string
        email: string
        nickname: string
        avatarUrl?: string
    } | null
    
    // Actions
    setLoginEmail: (email: string) => void
    setLoginPassword: (password: string) => void
    resetLoginForm: () => void
    
    setRegisterStep: (step: 1 | 2) => void
    setRegisterEmail: (email: string) => void
    setRegisterNickname: (nickname: string) => void
    setRegisterPassword: (password: string) => void
    setRegisterRepeatPassword: (repeatPassword: string) => void
    setRegisterAvatarFile: (file: File | null) => void
    setRegisterAvatarPreview: (preview: string | null) => void
    setRegisterGameProfiles: (profiles: GameProfile[]) => void
    resetRegisterForm: () => void
    
    login: (email: string, password: string) => Promise<void>
    logout: () => void
    setUser: (user: { id: string; email: string; nickname: string; avatarUrl?: string } | null) => void
}

export const useAuthStore = create<AuthState>((set) => ({
    // Initial login state
    loginEmail: "",
    loginPassword: "",
    
    // Initial register state
    registerStep: 1,
    registerEmail: "",
    registerNickname: "",
    registerPassword: "",
    registerRepeatPassword: "",
    registerAvatarFile: null,
    registerAvatarPreview: null,
    registerGameProfiles: [],
    
    // Initial session state
    isLoggedIn: false,
    user: null,
    
    // Login actions
    setLoginEmail: (email) => set({ loginEmail: email }),
    setLoginPassword: (password) => set({ loginPassword: password }),
    resetLoginForm: () => set({ loginEmail: "", loginPassword: "" }),
    
    // Register actions
    setRegisterStep: (step) => set({ registerStep: step }),
    setRegisterEmail: (email) => set({ registerEmail: email }),
    setRegisterNickname: (nickname) => set({ registerNickname: nickname }),
    setRegisterPassword: (password) => set({ registerPassword: password }),
    setRegisterRepeatPassword: (repeatPassword) => set({ registerRepeatPassword: repeatPassword }),
    setRegisterAvatarFile: (file) => set({ registerAvatarFile: file }),
    setRegisterAvatarPreview: (preview) => set({ registerAvatarPreview: preview }),
    setRegisterGameProfiles: (profiles) => set({ registerGameProfiles: profiles }),
    resetRegisterForm: () => set({
        registerStep: 1,
        registerEmail: "",
        registerNickname: "",
        registerPassword: "",
        registerRepeatPassword: "",
        registerAvatarFile: null,
        registerAvatarPreview: null,
        registerGameProfiles: []
    }),
    
    login: async (email, password) => {
        try {
            const userData = await loginUser({
                username: email.trim(),
                password: password,
            })

            if (userData) {
                const avatarUrl = userData.avatarURL || (typeof userData.avatar === 'string' ? userData.avatar : undefined);
                set({
                    isLoggedIn: true,
                    user: {
                        id: userData.id.toString(),
                        email: userData.email,
                        nickname: userData.username,
                        avatarUrl: avatarUrl,
                    },
                    loginEmail: "",
                    loginPassword: ""
                })
            }
        } catch (error) {
            const errorMessage = error instanceof Error ? error.message : 'Login failed. Please try again.'
            throw new Error(errorMessage)
        }
    },
    
    logout: () => set({
        isLoggedIn: false,
        user: null,
        loginEmail: "",
        loginPassword: ""
    }),
    
    setUser: (user) => set({ user, isLoggedIn: user !== null })
}))

