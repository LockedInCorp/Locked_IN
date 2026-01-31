"use client"

import { useState, useEffect } from "react"
import { DiscoverSidebar } from "@/custom_components/groupDiscover/DiscoverSidebar"
import { DiscoverFilters } from "@/custom_components/groupDiscover/DiscoverFilters"
import { GroupCardGrid } from "@/custom_components/groupDiscover/GroupCardGrid"
import { useGroupDiscoveryStore } from "@/stores/groupDiscoveryStore"
import { apiClientHttps } from "@/lib/apiClient"
import type { GroupCard, GameOption, PagedResult, TeamSearchResult } from "@/custom_components/groupDiscover/types"
//#TODO when current page=maxpage and changeView to lower value displays empty page
export default function DiscoverPage() {
    const { 
        groupSearch, 
        showPending, 
        currentPage,
        setGroupSearch, 
        setShowPending,
        setCurrentPage 
    } = useGroupDiscoveryStore()

    const [gameSearch, setGameSearch] = useState("")
    const [selectedGames, setSelectedGames] = useState<Set<string>>(new Set())
    const [selectedTagIds, setSelectedTagIds] = useState<Set<string>>(new Set())
    const [customGames, setCustomGames] = useState<Array<{ id: string; label: string }>>([])

    const [groups, setGroups] = useState<GroupCard[]>([])
    const [totalPages, setTotalPages] = useState(1)
    const [pageSize, setPageSize] = useState(12)
    const [sortBy, setSortBy] = useState("relevance")

    const visibleGames: GameOption[] = customGames.map((g) => ({ ...g, isFavorite: false }))

    useEffect(() => {
        const fetchGroups = async () => {
            try {
                const dto = {
                    gameIds: Array.from(selectedGames).map(id => parseInt(id)),
                    preferenceTagIds: Array.from(selectedTagIds).map(id => parseInt(id)),
                    searchTerm: groupSearch,
                    page: currentPage,
                    pageSize: pageSize,
                    sortBy: sortBy,
                    showPendingRequests: showPending
                }
                
                const response = await apiClientHttps.post<PagedResult<TeamSearchResult>>('/Team/search/advanced', dto)
                const data = response.data
                
                const mappedGroups: GroupCard[] = data.items.map((team: TeamSearchResult) => ({
                    id: team.id.toString(),
                    title: team.name,
                    game: team.gameName || "Unknown Game",
                    creator: team.teamLeaderNickname || "Unknown",
                    description: team.description,
                    image: team.iconUrl || "/assets/sunset-silhouette-gaming.jpg",
                    tags: team.preferenceTags || [],
                    currentMembers: team.currentMemberCount,
                    maxMembers: team.maxPlayerCount,
                    isPending: false
                }))

                setGroups(mappedGroups)
                setTotalPages(data.totalPages)
            } catch (error) {
                console.error(error)
            }
        }

        fetchGroups()
    }, [groupSearch, selectedGames, selectedTagIds, currentPage, pageSize, sortBy, showPending])

    const handleToggleTagFilter = (tagId: string) => {
        setSelectedTagIds((prev) => {
            const next = new Set(prev)
            if (next.has(tagId)) {
                next.delete(tagId)
            } else {
                next.add(tagId)
            }
            return next
        })
    }

    const handleAddGameFilter = (game: { id: string; label: string }) => {
        const isCustom = customGames.some((c) => c.id === game.id)

        if (!isCustom) {
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

            setCustomGames((prev) => prev.filter((g) => g.id !== gameId))
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
                selectedTagIds={selectedTagIds}
                onToggleTagFilter={handleToggleTagFilter}
            />

            <div className="flex-1 flex flex-col p-6 overflow-hidden min-w-0 w-full">
                <DiscoverFilters
                    groupSearch={groupSearch}
                    onGroupSearchChange={setGroupSearch}
                    showPending={showPending}
                    onShowPendingChange={setShowPending}
                    pageSize={pageSize}
                    onPageSizeChange={setPageSize}
                    sortBy={sortBy}
                    onSortByChange={setSortBy}
                />

                <GroupCardGrid
                    groups={groups}
                    currentPage={currentPage}
                    totalPages={totalPages}
                    onPageChange={setCurrentPage}
                />
            </div>
        </div>
    )
}
