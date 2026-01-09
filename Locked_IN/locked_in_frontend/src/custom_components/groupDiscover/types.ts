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

export interface PagedResult<T> {
    items: T[]
    totalCount: number
    page: number
    pageSize: number
    totalPages: number
}

export interface TeamSearchResult {
    id: number
    name: string
    minCompScore: number
    maxPlayerCount: number
    description: string
    gameId: number
    gameName: string
    isPrivate: boolean
    isBlitz: boolean
    experienceTagId: number
    experienceLevel: string
    currentMemberCount: number
    members: {
        id: number
        isLeader: boolean
        joinTimestamp: string
        teamId: number
        userId: number
        memberStatusId: number
        memberStatusName: string
        user: {
            id: number
            email: string
            nickname: string
            availability: string
        }
    }[]
    preferenceTags: string[]
    creationTimestamp: string
    iconUrl: string
    searchRank: number
    teamLeaderNickname: string
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
    pageSize: number
    onPageSizeChange: (value: number) => void
    sortBy: string
    onSortByChange: (value: string) => void
}

export interface DiscoverSidebarProps {
    gameSearch: string
    onGameSearchChange: (value: string) => void
    visibleGames: GameOption[]
    selectedGames: Set<string>
    onToggleGameFilter: (gameId: string) => void
    onAddGameFilter: (game: { id: string; label: string }) => void
    selectedTagIds: Set<string>
    onToggleTagFilter: (tagId: string) => void
}