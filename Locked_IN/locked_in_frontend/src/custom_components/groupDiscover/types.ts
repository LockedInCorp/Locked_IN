export interface GroupCard {
    id: string
    title: string
    game: string
    creator: string
    description: string
    image: string
    tags: string[]
    currentMembers: number
    maxMembers: number
    isPending?: boolean
}

export interface GameOption {
    id: string
    label: string
    isFavorite: boolean
}

export interface DiscoverFiltersProps {
    groupSearch: string
    onGroupSearchChange: (value: string) => void
    showPending: boolean
    onShowPendingChange: (value: boolean) => void
}

export interface DiscoverSidebarProps {
    gameSearch: string
    onGameSearchChange: (value: string) => void
    // Games that user can see/has added (favorites + custom). Used to resolve labels for tags.
    visibleGames: GameOption[]
    selectedGames: Set<string>
    onToggleGameFilter: (gameId: string) => void
    onAddGameFilter: (game: { id: string; label: string }) => void
}