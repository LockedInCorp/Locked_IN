import type { TeamMemberStatus } from "@/api/types"

export interface GroupCard {
    id: string
    title: string
    game: string
    teamLeaderUsername: string
    description: string
    image: string
    tags: string[]
    currentMembers: number
    maxMembers: number
    isPending?: boolean
    autoAccept: boolean
    teamMemberStatus: TeamMemberStatus
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