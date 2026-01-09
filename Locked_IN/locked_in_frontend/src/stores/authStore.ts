import { create } from "zustand"

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
    
    // Session actions
    login: async (email, password) => {
        // TODO: Implement actual login API call
        console.log("Logging in:", { email, password })
        // After successful login:
        set({
            isLoggedIn: true,
            user: {
                id: "1",
                email,
                nickname: "User",
                avatarUrl: undefined
            },
            loginEmail: "",
            loginPassword: ""
        })
    },
    
    logout: () => set({
        isLoggedIn: false,
        user: null,
        loginEmail: "",
        loginPassword: ""
    }),
    
    setUser: (user) => set({ user, isLoggedIn: user !== null })
}))

