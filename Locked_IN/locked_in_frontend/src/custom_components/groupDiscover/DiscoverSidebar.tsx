import { useState } from "react"
import { Star, Gamepad2, X } from "lucide-react"
import { Input } from "@/components/ui/input"
import { Checkbox } from "@/components/ui/checkbox"

export function DiscoverSidebar() {
    const [gameSearch, setGameSearch] = useState("")

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

                {/* Favorite Section */}
                <div className="space-y-3">
                    <div className="flex items-center gap-2 text-foreground">
                        <Star className="h-4 w-4" />
                        <span className="font-medium">Favorite</span>
                    </div>
                    <div className="space-y-2">
                        {[1, 2, 3].map((item) => (
                            <div key={`fav-${item}`} className="flex items-center justify-between bg-muted p-3 rounded-lg">
                                <span className="text-muted-foreground">List item</span>
                                <Checkbox
                                    className="data-[state=checked]:bg-primary data-[state=checked]:border-primary"
                                />
                            </div>
                        ))}
                    </div>
                </div>

                {/* All Games Section */}
                <div className="space-y-3">
                    <div className="flex items-center gap-2 text-foreground">
                        <Gamepad2 className="h-4 w-4" />
                        <span className="font-medium">All games</span>
                    </div>
                    <div className="space-y-2">
                        {[1, 2, 3].map((item) => (
                            <div key={`game-${item}`} className="flex items-center justify-between bg-muted p-3 rounded-lg">
                                <span className="text-muted-foreground">List item</span>
                                <Checkbox
                                    className="data-[state=checked]:bg-primary data-[state=checked]:border-primary"
                                />
                            </div>
                        ))}
                    </div>
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

