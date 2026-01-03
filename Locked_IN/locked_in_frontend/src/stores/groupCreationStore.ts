import { create } from "zustand"

interface GroupCreationState {
    selectedTags: string[]
    blitzRoom: boolean
    autoAccept: boolean
    experience: string
    groupSize: string
    
    // Actions
    setSelectedTags: (tags: string[]) => void
    toggleTag: (tag: string) => void
    setBlitzRoom: (blitzRoom: boolean) => void
    setAutoAccept: (autoAccept: boolean) => void
    setExperience: (experience: string) => void
    setGroupSize: (size: string) => void
    resetForm: () => void
}

export const useGroupCreationStore = create<GroupCreationState>((set, get) => ({
    // Initial state
    selectedTags: [],
    blitzRoom: false,
    autoAccept: false,
    experience: "beginner",
    groupSize: "",
    
    // Actions
    setSelectedTags: (tags) => set({ selectedTags: tags }),
    toggleTag: (tag) => {
        const currentTags = get().selectedTags
        const newTags = currentTags.includes(tag)
            ? currentTags.filter(t => t !== tag)
            : [...currentTags, tag]
        set({ selectedTags: newTags })
    },
    setBlitzRoom: (blitzRoom) => set({ blitzRoom }),
    setAutoAccept: (autoAccept) => set({ autoAccept }),
    setExperience: (experience) => set({ experience }),
    setGroupSize: (size) => set({ groupSize: size }),
    resetForm: () => set({
        selectedTags: [],
        blitzRoom: false,
        autoAccept: false,
        experience: "beginner",
        groupSize: ""
    })
}))

