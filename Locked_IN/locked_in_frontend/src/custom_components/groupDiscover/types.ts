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

export interface DiscoverFiltersProps {
    groupSearch: string
    onGroupSearchChange: (value: string) => void
    showPending: boolean
    onShowPendingChange: (checked: boolean) => void
}

