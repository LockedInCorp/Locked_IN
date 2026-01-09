import { X, Check } from "lucide-react"
import { Input } from "@/components/ui/input"
import { Checkbox } from "@/components/ui/checkbox"
import { Button } from "@/components/ui/button"
import { useGroupDiscoveryStore } from "@/stores/groupDiscoveryStore"

export function DiscoverSidebar() {
    const { gameSearch, selectedFilters, setGameSearch, toggleFilter } = useGroupDiscoveryStore()

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

