"use client"

import { useState, useEffect } from "react"
import { X, Search } from "lucide-react"
import { Input } from "@/components/ui/input"
import { Checkbox } from "@/components/ui/checkbox"
import { apiClientHttps } from "@/lib/apiClient"
import type { DiscoverSidebarProps } from "./types"

interface Tag {
    id: string
    name: string
}

export function DiscoverSidebar({
    gameSearch,
    onGameSearchChange,
    visibleGames,
    selectedGames,
    onToggleGameFilter,
    onAddGameFilter,
    selectedTagIds,
    onToggleTagFilter,
}: DiscoverSidebarProps) {
    const [tags, setTags] = useState<Tag[]>([])
    const [tagsLoading, setTagsLoading] = useState(true)
    const [tagsError, setTagsError] = useState<string | null>(null)
    const [gameSuggestions, setGameSuggestions] = useState<Array<{ id: string; label: string }>>([])
    const [loading, setLoading] = useState(false)
    const [error, setError] = useState<string | null>(null)

    useEffect(() => {
        const fetchTags = async () => {
            try {
                setTagsLoading(true)
                const response = await apiClientHttps.get("/Tag")
                const data = response.data
                
                if (data.data?.preferenceTags) {
                    const mappedTags: Tag[] = data.data.preferenceTags.map((tag: any) => ({
                        id: tag.id?.toString() || "",
                        name: tag.preferencename || tag.name || ""
                    })).filter((tag: Tag) => tag.id && tag.name)
                    setTags(mappedTags)
                }
            } catch (err) {
                setTagsError(err instanceof Error ? err.message : "Failed to load tags")
            } finally {
                setTagsLoading(false)
            }
        }

        fetchTags()
    }, [])

    useEffect(() => {
        if (!gameSearch || gameSearch.length < 2) {
            setGameSuggestions([])
            return
        }

        const searchGames = async () => {
            try {
                setLoading(true)
                setError(null)
                const response = await apiClientHttps.get(`/Game/search?searchTerm=${encodeURIComponent(gameSearch)}`)
                const data = response.data
                
                const suggestions = Array.isArray(data) 
                    ? data.map((game: any) => ({
                        id: game.id?.toString() || "",
                        label: game.name || ""
                    }))
                    : []
                setGameSuggestions(suggestions)
            } catch (err) {
                setError(err instanceof Error ? err.message : "Failed to search games")
                setGameSuggestions([])
            } finally {
                setLoading(false)
            }
        }

        const debounceTimer = setTimeout(searchGames, 300)
        return () => clearTimeout(debounceTimer)
    }, [gameSearch])

    const selectedGameTags = visibleGames.filter(game => selectedGames.has(game.id))

    return (
        <div className="w-[380px] flex-shrink-0 p-6 space-y-6 overflow-y-auto">
            {/* Game Selection */}
            <div className="space-y-4">
                <h2 className="text-2xl font-bold text-foreground">Game Selection</h2>

                <div className="relative">
                    <Input
                        placeholder="Search by game title..."
                        value={gameSearch}
                        onChange={(e) => onGameSearchChange(e.target.value)}
                        className="bg-muted border-border text-foreground placeholder:text-muted-foreground pr-8"
                    />
                    {gameSearch ? (
                        <button
                            onClick={() => onGameSearchChange("")}
                            className="absolute right-2 top-1/2 -translate-y-1/2 text-muted-foreground hover:text-foreground cursor-pointer"
                        >
                            <X className="h-4 w-4" />
                        </button>
                    ) : (
                        <Search className="absolute right-2 top-1/2 -translate-y-1/2 h-4 w-4 text-muted-foreground" />
                    )}

                    {/* Search Results Dropdown */}
                    {gameSearch && (
                        <div className="absolute z-10 w-full mt-1 bg-popover border border-border rounded-md shadow-md overflow-hidden">
                            {loading && (
                                <div className="px-4 py-2 text-sm text-muted-foreground">Searching...</div>
                            )}
                            {!loading && error && (
                                <div className="px-4 py-2 text-sm text-destructive">{error}</div>
                            )}
                            {!loading && !error && gameSuggestions.length === 0 && (
                                <div className="px-4 py-2 text-sm text-muted-foreground">No results</div>
                            )}
                            {!loading && !error && gameSuggestions.map((game) => (
                                <button
                                    key={game.id}
                                    className="w-full text-left px-4 py-2 text-sm hover:bg-accent hover:text-accent-foreground transition-colors"
                                    onClick={() => {
                                        onAddGameFilter(game)
                                        onGameSearchChange("")
                                        setGameSuggestions([])
                                    }}
                                >
                                    {game.label}
                                </button>
                            ))}
                        </div>
                    )}
                </div>

                {/* Selected Games as Tags */}
                <div className="flex flex-wrap gap-2">
                    {selectedGameTags.length === 0 ? (
                        <span className="text-sm text-muted-foreground">No games selected</span>
                    ) : (
                        selectedGameTags.map((g) => (
                            <div
                                key={g.id}
                                className="inline-flex items-center gap-2 px-3 py-1 rounded-full bg-muted text-foreground border border-border"
                            >
                                <span className="text-sm">{g.label}</span>
                                <button
                                    aria-label={`Remove ${g.label}`}
                                    className="text-muted-foreground hover:text-foreground"
                                    onClick={() => onToggleGameFilter(g.id)}
                                >
                                    <X className="h-3.5 w-3.5" />
                                </button>
                            </div>
                        ))
                    )}
                </div>
            </div>

            {/* Tags Section */}
            <div className="space-y-4">
                <h2 className="text-2xl font-bold text-foreground">Tags</h2>
                <div className="space-y-2">
                    {tagsLoading && (
                        <div className="px-1 text-sm text-muted-foreground">Loading tags...</div>
                    )}
                    {!tagsLoading && tagsError && (
                        <div className="px-1 text-sm text-destructive">{tagsError}</div>
                    )}
                    {!tagsLoading && !tagsError && tags.length === 0 && (
                        <div className="px-1 text-sm text-muted-foreground">No tags</div>
                    )}
                    {!tagsLoading && !tagsError && tags.map((tag) => (
                        <div key={tag.id} className="flex items-center justify-between bg-muted p-3 rounded-lg">
                            <span className="text-muted-foreground">{tag.name}</span>
                            <Checkbox
                                checked={selectedTagIds.has(tag.id)}
                                onCheckedChange={() => onToggleTagFilter(tag.id)}
                                className="data-[state=checked]:bg-primary data-[state=checked]:border-primary"
                            />
                        </div>
                    ))}
                </div>
            </div>
        </div>
    )
}
