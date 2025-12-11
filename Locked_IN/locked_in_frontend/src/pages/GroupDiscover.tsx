"use client"

import { useState } from "react"
import { DiscoverSidebar } from "@/custom_components/groupDiscover/DiscoverSidebar"
import { DiscoverFilters } from "@/custom_components/groupDiscover/DiscoverFilters"
import { GroupCardGrid } from "@/custom_components/groupDiscover/GroupCardGrid"
import type { GroupCard, GameOption } from "@/custom_components/groupDiscover/types"

const MOCK_FAVORITES: GameOption[] = [
    { id: "ow2", label: "Overwatch 2", isFavorite: true },
    { id: "lol", label: "League of Legends", isFavorite: true },
    { id: "val", label: "Valorant", isFavorite: true },
]

// Placeholder ALL_GAMES_MOCK removed. Search now queries backend.

const mockGroups: GroupCard[] = [
// ... existing code ...
    {
        id: "6",
        title: "Lets get ON!",
        game: "Overwatch",
        creator: "Riggid",
        description:
            "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.",
        image: "/sunset-silhouette-gaming.jpg",
        tags: ["Chill", "Arcade", "NewPeople"],
        currentMembers: 1,
        maxMembers: 6,
    },
]

export default function DiscoverPage() {
    const [groupSearch, setGroupSearch] = useState("")
    const [showPending, setShowPending] = useState(false)

    // Sidebar State
    const [gameSearch, setGameSearch] = useState("")
    const [selectedGames, setSelectedGames] = useState<Set<string>>(new Set())
    const [customGames, setCustomGames] = useState<Array<{ id: string; label: string }>>([])

    const visibleGames: GameOption[] = [
        ...MOCK_FAVORITES,
        ...customGames.map((g) => ({ ...g, isFavorite: false })),
    ]

    const handleAddGameFilter = (game: { id: string; label: string }) => {
        const isFavorite = MOCK_FAVORITES.some((f) => f.id === game.id)
        const isCustom = customGames.some((c) => c.id === game.id)

        if (!isFavorite && !isCustom) {
            setCustomGames((prev) => [...prev, game])
        }

        setSelectedGames((prev) => {
            const next = new Set(prev)
            next.add(game.id)
            return next
        })
        setGameSearch("")
    }

    const handleToggleGameFilter = (gameId: string) => {
        const isSelected = selectedGames.has(gameId)

        if (isSelected) {
            setSelectedGames((prev) => {
                const next = new Set(prev)
                next.delete(gameId)
                return next
            })

            const isFavorite = MOCK_FAVORITES.some((f) => f.id === gameId)
            if (!isFavorite) {
                setCustomGames((prev) => prev.filter((g) => g.id !== gameId))
            }
        } else {
            setSelectedGames((prev) => {
                const next = new Set(prev)
                next.add(gameId)
                return next
            })
        }
    }

    return (
        <div className="flex h-full overflow-hidden">
            <DiscoverSidebar
                gameSearch={gameSearch}
                onGameSearchChange={setGameSearch}
                visibleGames={visibleGames}
                selectedGames={selectedGames}
                onToggleGameFilter={handleToggleGameFilter}
                onAddGameFilter={handleAddGameFilter}
            />

            <div className="flex-1 flex flex-col p-6 overflow-hidden">
                <DiscoverFilters
                    groupSearch={groupSearch}
                    onGroupSearchChange={setGroupSearch}
                    showPending={showPending}
                    onShowPendingChange={setShowPending}
                />

                <GroupCardGrid groups={mockGroups} />
            </div>
        </div>
    )
}
