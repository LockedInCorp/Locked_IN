import { create } from "zustand"

interface RegisterState {
    // Register Part 2 UI state
    expandedGames: Set<string>
    selectedGame: string
    customGame: string
    
    // Actions
    setExpandedGames: (games: Set<string>) => void
    toggleExpandedGame: (gameName: string) => void
    setSelectedGame: (game: string) => void
    setCustomGame: (game: string) => void
    resetRegisterUI: () => void
}

export const useRegisterStore = create<RegisterState>((set, get) => ({
    // Initial state
    expandedGames: new Set(),
    selectedGame: "",
    customGame: "",
    
    // Actions
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
    resetRegisterUI: () => set({
        expandedGames: new Set(),
        selectedGame: "",
        customGame: ""
    })
}))

