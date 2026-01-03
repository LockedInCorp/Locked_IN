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
                        onChange={(e) => setGameSearch(e.target.value)}
                        className="bg-muted border-border text-foreground placeholder:text-muted-foreground pr-8"
                    />
                    {gameSearch && (
                        <button
                            onClick={() => setGameSearch("")}
                            className="absolute right-2 top-1/2 -translate-y-1/2 text-muted-foreground hover:text-foreground cursor-pointer"
                        >
                            <X className="h-4 w-4" />
                        </button>
                    )}
                </div>

                {/* Game Selection Filter Buttons */}
                <div className="flex flex-wrap gap-2">
                    {/* TODO: Add games from the database */}
                    {[1, 2, 3, 4, 5].map((item, index) => (
                        <Button
                            key={`filter-${item}`}
                            onClick={() => toggleFilter(index)}
                            className={`h-9 px-4 rounded-md font-medium transition-all ${
                                selectedFilters.has(index)
                                    ? "bg-primary hover:bg-primary/90 text-primary-foreground"
                                    : "bg-muted hover:bg-muted/80 text-muted-foreground border border-border"
                            }`}
                        >
                            {selectedFilters.has(index) && (
                                <Check className="h-4 w-4 mr-1" />
                            )}
                            Label
                        </Button>
                    ))}
                </div>
            </div>

            {/* Tags Section */}
            <div className="space-y-4">
                <h2 className="text-2xl font-bold text-foreground">Tags</h2>
                <div className="space-y-2">
                    {[1, 2, 3, 4].map((item) => (
                        <div key={`tag-${item}`} className="flex items-center justify-between bg-muted p-3 rounded-lg">
                            <span className="text-muted-foreground">List item</span>
                            <Checkbox
                                className="data-[state=checked]:bg-primary data-[state=checked]:border-primary"
                            />
                        </div>
                    ))}
                </div>
            </div>
        </div>
    )
}

