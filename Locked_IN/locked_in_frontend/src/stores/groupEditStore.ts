import { create } from "zustand"

interface GroupEditState {
    groupName: string
    gameId: number | null
    gameName: string
    groupSize: string
    isPrivate: boolean
    autoAccept: boolean
    previewImage: File | null

    selectedTags: number[]
    experience: number
    competitiveScore: string
    communicationService: number | undefined
    communicationLink: string
    description: string

    teamId: number | null

    setGroupName: (name: string) => void
    setGame: (id: number | null, name: string) => void
    setGroupSize: (size: string) => void
    setIsPrivate: (isPrivate: boolean) => void
    setAutoAccept: (autoAccept: boolean) => void
    setPreviewImage: (file: File | null) => void

    setSelectedTags: (tags: number[]) => void
    toggleTag: (tagId: number) => void
    setExperience: (experience: number) => void
    setCompetitiveScore: (score: string) => void
    setCommunicationService: (service?: number) => void
    setCommunicationLink: (link: string) => void
    setDescription: (description: string) => void

    setTeamId: (id: number | null) => void
    loadTeamData: (teamData: any) => void
    resetForm: () => void
}

export const useGroupEditStore = create<GroupEditState>((set, get) => ({
    // Initial state
    groupName: "",
    gameId: null,
    gameName: "",
    groupSize: "",
    isPrivate: false,
    autoAccept: false,
    previewImage: null,
    selectedTags: [],
    experience: 0,
    competitiveScore: "0",
    communicationService: undefined,
    communicationLink: "",
    description: "",
    teamId: null,

    // Actions - General
    setGroupName: (name) => set({ groupName: name }),
    setGame: (id, name) => set({ gameId: id, gameName: name }),
    setGroupSize: (size) => set({ groupSize: size }),
    setIsPrivate: (isPrivate) => set({ isPrivate }),
    setAutoAccept: (autoAccept) => set({ autoAccept }),
    setPreviewImage: (file) => set({ previewImage: file }),
    
    // Actions - Finder Settings
    setSelectedTags: (tags) => set({ selectedTags: tags }),
    toggleTag: (tagId) => {
        const currentTags = get().selectedTags
        const newTags = currentTags.includes(tagId)
            ? currentTags.filter(id => id !== tagId)
            : [...currentTags, tagId]
        set({ selectedTags: newTags })
    },
    setExperience: (experience) => set({ experience }),
    setCompetitiveScore: (score) => set({ competitiveScore: score }),
    setCommunicationService: (service) => set({ communicationService: service }),
    setCommunicationLink: (link) => set({ communicationLink: link }),
    setDescription: (description) => set({ description }),
    
    // Actions - Team management
    setTeamId: (id) => set({ teamId: id }),
    loadTeamData: (teamData) => {
        const prefTags = teamData.preferenceTags || []
        const tagIds = prefTags.map((p: { id: number }) => p.id)
        const expId = teamData.experienceLevel?.id ?? teamData.experienceTagId ?? 0
        const commServiceId = teamData.communicationService?.id

        set({
            groupName: teamData.name || "",
            gameId: teamData.game?.id ?? teamData.gameId ?? null,
            gameName: teamData.game?.name ?? teamData.gameName ?? "",
            groupSize: teamData.maxPlayerCount?.toString() ?? "",
            isPrivate: teamData.isPrivate ?? false,
            autoAccept: teamData.autoAccept ?? false,
            selectedTags: tagIds,
            experience: expId,
            competitiveScore: teamData.minCompScore?.toString() || "0",
            communicationService: commServiceId,
            communicationLink: teamData.communicationServiceLink || "",
            description: teamData.description || "",
            teamId: teamData.id || null
        })
    },
    resetForm: () => set({
        groupName: "",
        gameId: null,
        gameName: "",
        groupSize: "",
        isPrivate: false,
        autoAccept: false,
        previewImage: null,
        selectedTags: [],
        experience: 0,
        competitiveScore: "0",
        communicationService: undefined,
        communicationLink: "",
        description: "",
        teamId: null
    })
}))

