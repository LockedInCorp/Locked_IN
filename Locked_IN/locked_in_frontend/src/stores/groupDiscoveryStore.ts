import { create } from "zustand"

interface GroupDiscoveryState {
    groupSearch: string
    showPending: boolean
    gameSearch: string
    selectedFilters: Set<number>
    currentPage: number
    
    setGroupSearch: (search: string) => void
    setShowPending: (show: boolean) => void
    setGameSearch: (search: string) => void
    toggleFilter: (index: number) => void
    setCurrentPage: (page: number) => void
    resetFilters: () => void
}

export const useGroupDiscoveryStore = create<GroupDiscoveryState>((set, get) => ({
    groupSearch: "",
    showPending: false,
    gameSearch: "",
    selectedFilters: new Set([0]),
    currentPage: 1,
    
    setGroupSearch: (search) => set({ groupSearch: search }),
    setShowPending: (show) => set({ showPending: show }),
    setGameSearch: (search) => set({ gameSearch: search }),
    toggleFilter: (index) => {
        const newSelected = new Set(get().selectedFilters)
        if (newSelected.has(index)) {
            newSelected.delete(index)
        } else {
            newSelected.add(index)
        }
        set({ selectedFilters: newSelected })
    },
    setCurrentPage: (page) => set({ currentPage: page }),
    resetFilters: () => set({
        groupSearch: "",
        showPending: false,
        gameSearch: "",
        selectedFilters: new Set([0]),
        currentPage: 1
    })
}))

