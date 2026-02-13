"use client"

import { useState, useEffect, useCallback } from "react"
import { DiscoverSidebar } from "@/components/groupDiscover/DiscoverSidebar"
import { DiscoverFilters } from "@/components/groupDiscover/DiscoverFilters"
import { GroupCardGrid } from "@/components/groupDiscover/GroupCardGrid"
import { useGroupDiscoveryStore } from "@/stores/groupDiscoveryStore"
import { searchTeamsAdvanced } from "@/api/api"
import { useTeamJoinHub } from "@/hooks/signalr/useTeamJoinHub"
import type { GameOption } from "@/api/types"
import type { TeamSearchResult, TeamJoinStatusDto } from "@/api/types"
import { TeamMemberStatus as TeamMemberStatusValues } from "@/api/types"

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

    const [groups, setGroups] = useState<TeamSearchResult[]>([])
    const [totalPages, setTotalPages] = useState(1)
    const [pageSize, setPageSize] = useState(12)
    const [sortBy, setSortBy] = useState("relevance")

    const visibleGames: GameOption[] = customGames.map((g) => ({ ...g, isFavorite: false }))

    const fetchGroups = useCallback(async () => {
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
            
            const data = await searchTeamsAdvanced(dto)
            
            setGroups(data.items)
            setTotalPages(data.totalPages)
        } catch (error) {
            console.error(error)
        }
    }, [groupSearch, selectedGames, selectedTagIds, currentPage, pageSize, sortBy, showPending])

    useEffect(() => {
        fetchGroups()
    }, [fetchGroups])

    const handleJoinRequestStatus = useCallback((data: TeamJoinStatusDto) => {
        setGroups((prevGroups) => {
            const updatedGroups = prevGroups.map((group) => {
                if (group.id === data.teamId) {
                    return {
                        ...group,
                        teamMemberStatus: data.status as TeamMemberStatusValues
                    }
                }
                return group
            })
            return updatedGroups
        })

        if (showPending) {
            fetchGroups()
        }
    }, [showPending, fetchGroups])

    useTeamJoinHub(handleJoinRequestStatus)

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
                    onRefresh={fetchGroups}
                />
            </div>
        </div>
    )
}
