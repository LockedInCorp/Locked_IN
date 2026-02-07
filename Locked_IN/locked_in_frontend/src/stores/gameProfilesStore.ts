import { create } from "zustand"

interface GameProfilesState {
    expandedGames: Set<string>
    selectedGame: string
    customGame: string
    
    setExpandedGames: (games: Set<string>) => void
    toggleExpandedGame: (gameName: string) => void
    setSelectedGame: (game: string) => void
    setCustomGame: (game: string) => void
    resetGameProfilesUI: () => void
}

export const useGameProfilesStore = create<GameProfilesState>((set, get) => ({
    expandedGames: new Set(),
    selectedGame: "",
    customGame: "",
    
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
    resetGameProfilesUI: () => set({
        expandedGames: new Set(),
        selectedGame: "",
        customGame: ""
    })
}))
