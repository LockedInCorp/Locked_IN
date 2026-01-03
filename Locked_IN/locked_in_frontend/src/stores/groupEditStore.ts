import { create } from "zustand"

interface GroupEditState {
    // General fields
    groupName: string
    game: string
    groupSize: string
    blitzRoom: boolean
    autoAccept: boolean
    previewImage: File | null
    
    // Finder Settings fields
    selectedTags: string[]
    experience: string
    competitiveScore: string
    communicationService: string
    description: string
    
    // Team ID being edited
    teamId: number | null
    
    // Actions - General
    setGroupName: (name: string) => void
    setGame: (game: string) => void
    setGroupSize: (size: string) => void
    setBlitzRoom: (blitzRoom: boolean) => void
    setAutoAccept: (autoAccept: boolean) => void
    setPreviewImage: (file: File | null) => void
    
    // Actions - Finder Settings
    setSelectedTags: (tags: string[]) => void
    toggleTag: (tag: string) => void
    setExperience: (experience: string) => void
    setCompetitiveScore: (score: string) => void
    setCommunicationService: (service: string) => void
    setDescription: (description: string) => void
    
    // Actions - Team management
    setTeamId: (id: number | null) => void
    loadTeamData: (teamData: any) => void
    resetForm: () => void
}

export const useGroupEditStore = create<GroupEditState>((set, get) => ({
    // Initial state
    groupName: "",
    game: "",
    groupSize: "",
    blitzRoom: false,
    autoAccept: false,
    previewImage: null,
    selectedTags: [],
    experience: "beginner",
    competitiveScore: "0",
    communicationService: "discord",
    description: "",
    teamId: null,
    
    // Actions - General
    setGroupName: (name) => set({ groupName: name }),
    setGame: (game) => set({ game }),
    setGroupSize: (size) => set({ groupSize: size }),
    setBlitzRoom: (blitzRoom) => set({ blitzRoom }),
    setAutoAccept: (autoAccept) => set({ autoAccept }),
    setPreviewImage: (file) => set({ previewImage: file }),
    
    // Actions - Finder Settings
    setSelectedTags: (tags) => set({ selectedTags: tags }),
    toggleTag: (tag) => {
        const currentTags = get().selectedTags
        const newTags = currentTags.includes(tag)
            ? currentTags.filter(t => t !== tag)
            : [...currentTags, tag]
        set({ selectedTags: newTags })
    },
    setExperience: (experience) => set({ experience }),
    setCompetitiveScore: (score) => set({ competitiveScore: score }),
    setCommunicationService: (service) => set({ communicationService: service }),
    setDescription: (description) => set({ description }),
    
    // Actions - Team management
    setTeamId: (id) => set({ teamId: id }),
    loadTeamData: (teamData) => {
        set({
            groupName: teamData.name || "",
            game: teamData.gameName || "",
            groupSize: teamData.maxPlayerCount?.toString() || "",
            blitzRoom: teamData.isBlitz || false,
            autoAccept: false, // This might need to come from API
            selectedTags: teamData.preferenceTags || [],
            experience: teamData.experienceLevel?.toLowerCase() || "beginner",
            competitiveScore: teamData.minCompScore?.toString() || "0",
            communicationService: "discord", // This might need to come from API
            description: teamData.description || "",
            teamId: teamData.id || null
        })
    },
    resetForm: () => set({
        groupName: "",
        game: "",
        groupSize: "",
        blitzRoom: false,
        autoAccept: false,
        previewImage: null,
        selectedTags: [],
        experience: "beginner",
        competitiveScore: "0",
        communicationService: "discord",
        description: "",
        teamId: null
    })
}))

