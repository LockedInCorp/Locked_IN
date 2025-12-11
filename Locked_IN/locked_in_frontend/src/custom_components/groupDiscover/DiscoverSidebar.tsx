import { X, Search } from "lucide-react"
import { useEffect, useMemo, useState } from "react"
import { Input } from "@/components/ui/input"
import { Checkbox } from "@/components/ui/checkbox"
import type { DiscoverSidebarProps } from "./types"

export function DiscoverSidebar({
    gameSearch,
    onGameSearchChange,
    visibleGames,
    selectedGames,
    onToggleGameFilter,
    onAddGameFilter,
}: DiscoverSidebarProps) {

    const [suggestions, setSuggestions] = useState<Array<{ id: string; label: string }>>([])
    const [loading, setLoading] = useState(false)
    const [error, setError] = useState<string | null>(null)
    // Tags
    const [tags, setTags] = useState<Array<{ id: string; name: string }>>([])
    const [tagsLoading, setTagsLoading] = useState(false)
    const [tagsError, setTagsError] = useState<string | null>(null)

    const selectedGameTags = useMemo(() => {
        const byId = new Map(visibleGames.map((g) => [g.id, g.label]))
        return Array.from(selectedGames).map((id) => ({ id, label: byId.get(id) ?? id }))
    }, [selectedGames, visibleGames])

    // Debounced search to backend
    useEffect(() => {
        const q = gameSearch.trim()
        if (!q) {
            setSuggestions([])
            setError(null)
            setLoading(false)
            return
        }

        let active = true
        const timer = setTimeout(async () => {
            try {
                setLoading(true)
                setError(null)
                const res = await fetch(
                    `https://localhost:7252/api/Game/search?searchTerm=${encodeURIComponent(q)}`
                )
                if (!res.ok) throw new Error(`Search failed: ${res.status}`)
                const data: Array<{ id: number | string; name: string }> = await res.json()
                if (!active) return
                const mapped = data
                    .map((g) => ({ id: String(g.id), label: g.name }))
                    .filter((g) => !selectedGames.has(g.id) && !visibleGames.some((v) => v.id === g.id))
                setSuggestions(mapped)
            } catch {
                setError('Failed to load suggestions')
                setSuggestions([])
            } finally {
                if (active) setLoading(false)
            }
        }, 300)

        return () => {
            active = false
            clearTimeout(timer)
        }
    }, [gameSearch, selectedGames, visibleGames])

    // Load tags from backend
    useEffect(() => {
        let cancelled = false
        const load = async () => {
            try {
                setTagsLoading(true)
                setTagsError(null)
                const res = await fetch('https://localhost:7252/api/Tag')
                if (!res.ok) throw new Error(`Tags fetch failed: ${res.status}`)
                const data: Array<{ id: number | string; name: string }> = await res.json()
                if (cancelled) return
                setTags(data.map(t => ({ id: String(t.id), name: t.name })))
            } catch {
                if (cancelled) return
                setTagsError('Failed to load tags')
                setTags([])
            } finally {
                if (!cancelled) setTagsLoading(false)
            }
        }
        load()
        return () => { cancelled = true }
    }, [])

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
                            {!loading && !error && suggestions.length === 0 && (
                                <div className="px-4 py-2 text-sm text-muted-foreground">No results</div>
                            )}
                            {!loading && !error && suggestions.map((game) => (
                                <button
                                    key={game.id}
                                    className="w-full text-left px-4 py-2 text-sm hover:bg-accent hover:text-accent-foreground transition-colors"
                                    onClick={() => {
                                        onAddGameFilter(game)
                                        onGameSearchChange("")
                                        setSuggestions([])
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
                            <Checkbox className="data-[state=checked]:bg-primary data-[state=checked]:border-primary" />
                        </div>
                    ))}
                </div>
            </div>
        </div>
    )
}

