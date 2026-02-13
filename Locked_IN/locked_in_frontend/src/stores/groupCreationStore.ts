import { create } from "zustand"

interface GroupCreationState {
    selectedTags: number[]
    isPrivate: boolean
    autoAccept: boolean
    experience: number
    competitiveScore: string
    groupSize: string
    gameId: number | null
    gameName: string
    groupName: string
    description: string
    communicationService?: number
    communicationLink?: string

    setSelectedTags: (tags: number[]) => void
    toggleTag: (tagId: number) => void
    setIsPrivate: (isPrivate: boolean) => void
    setAutoAccept: (autoAccept: boolean) => void
    setExperience: (experience: number) => void
    setCompetitiveScore: (score: string) => void
    setGroupSize: (size: string) => void
    setGame: (id: number | null, name: string) => void
    setGroupName: (name: string) => void
    setDescription: (description: string) => void
    setCommunicationService: (service?: number) => void
    setCommunicationLink: (link: string) => void
    resetForm: () => void
}

export const useGroupCreationStore = create<GroupCreationState>((set, get) => ({
    selectedTags: [],
    isPrivate: false,
    autoAccept: false,
    experience: 0,
    competitiveScore: "0",
    groupSize: "",
    gameId: null,
    gameName: "",
    groupName: "",
    description: "",
    communicationService: undefined,
    communicationLink: "",
    
    setSelectedTags: (tags) => set({ selectedTags: tags }),
    toggleTag: (tagId) => {
        const currentTags = get().selectedTags
        const newTags = currentTags.includes(tagId)
            ? currentTags.filter(id => id !== tagId)
            : [...currentTags, tagId]
        set({ selectedTags: newTags })
    },
    setIsPrivate: (isPrivate) => set({ isPrivate: isPrivate }),
    setAutoAccept: (autoAccept) => set({ autoAccept }),
    setExperience: (experience) => set({ experience }),
    setCompetitiveScore: (competitiveScore) => set({ competitiveScore }),
    setGroupSize: (size) => set({ groupSize: size }),
    setGame: (id, name) => set({ gameId: id, gameName: name }),
    setGroupName: (groupName) => set({ groupName }),
    setDescription: (description) => set({ description }),
    setCommunicationService: (service) => set({ communicationService: service }),
    setCommunicationLink: (link) => set({ communicationLink: link }),
    resetForm: () => set({
        selectedTags: [],
        isPrivate: false,
        autoAccept: false,
        experience: 0,
        competitiveScore: "0",
        groupSize: "",
        gameId: null,
        gameName: "",
        groupName: "",
        description: "",
        communicationService: undefined,
        communicationLink: ""
    })
}))

