"use client"

import { useState } from "react"
import {Star, Gamepad2, X, Users, Plus} from "lucide-react"
import { Input } from "@/components/ui/input"
import { Button } from "@/components/ui/button"
import { Checkbox } from "@/components/ui/checkbox"
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select"
import {useNavigate} from "react-router-dom";

interface GroupCard {
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

const mockGroups: GroupCard[] = [
    {
        id: "1",
        title: "Lets get ON!",
        game: "Overwatch",
        creator: "Riggid",
        description:
            "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.",
        image: "/sunset-silhouette-gaming.jpg",
        tags: ["Chill", "Arcade", "NewPeople"],
        currentMembers: 1,
        maxMembers: 6,
        isPending: true,
    },
    {
        id: "2",
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
    {
        id: "3",
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
    {
        id: "4",
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
    {
        id: "5",
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
    const [gameSearch, setGameSearch] = useState("")
    const [groupSearch, setGroupSearch] = useState("")
    const [showPending, setShowPending] = useState(false)

    const navigate = useNavigate();

    return (
        <div className="flex h-full overflow-hidden">
            {/* Left Sidebar */}
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
                                        defaultChecked
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
                                        defaultChecked
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
                                    defaultChecked
                                    className="data-[state=checked]:bg-primary data-[state=checked]:border-primary"
                                />
                            </div>
                        ))}
                    </div>
                </div>
            </div>

            {/* Main Content */}
            <div className="flex-1 flex flex-col p-6 overflow-hidden">
                <div className="shrink-0 mb-6 space-y-4">
                    {/* Search and Filters */}
                    <Input
                        placeholder="Search groups..."
                        value={groupSearch}
                        onChange={(e) => setGroupSearch(e.target.value)}
                        className="border-border text-foreground placeholder:text-muted-foreground h-12"
                    />

                    <div className="flex items-center justify-between gap-4">
                        <div className="flex items-center gap-4">
                            <Select defaultValue="20">
                                <SelectTrigger className="w-[140px] bg-card border-border text-foreground">
                                    <SelectValue placeholder="View"/>
                                </SelectTrigger>
                                <SelectContent className="bg-card border-border text-foreground">
                                    <SelectItem value="10">View: 10</SelectItem>
                                    <SelectItem value="20">View: 20</SelectItem>
                                    <SelectItem value="50">View: 50</SelectItem>
                                </SelectContent>
                            </Select>

                            <Select defaultValue="relevance">
                                <SelectTrigger className="w-[200px] bg-card border-border text-foreground">
                                    <SelectValue placeholder="Sort by"/>
                                </SelectTrigger>
                                <SelectContent className="bg-card border-border text-foreground">
                                    <SelectItem value="relevance">Sort by: Relevance</SelectItem>
                                    <SelectItem value="newest">Sort by: Newest</SelectItem>
                                    <SelectItem value="popular">Sort by: Popular</SelectItem>
                                </SelectContent>
                            </Select>

                            <label className="flex items-center gap-2 text-foreground cursor-pointer">
                                <Checkbox
                                    checked={showPending}
                                    onCheckedChange={(checked) => setShowPending(checked as boolean)}
                                    className="data-[state=checked]:bg-primary data-[state=checked]:border-primary"
                                />
                                <span>Only Show Pending</span>
                            </label>
                        </div>

                        <Button
                            className="bg-primary hover:bg-primary/90 text-primary-foreground h-10 px-6 font-medium"
                            onClick={() => navigate("/groups/new")}
                        >
                            <Plus className="h-5 w-5 mr-2"/>
                            Create group
                        </Button>
                    </div>
                </div>


                <div className="flex-1 overflow-y-auto pr-2">
                    <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
                        {mockGroups.map((group) => (
                            <div
                                key={group.id}
                                className="bg-card rounded-lg overflow-hidden border border-border hover:border-border transition-colors cursor-pointer"
                            >
                                {/* Card Header */}
                                <div className="flex gap-3 p-4 border-b border-border">
                                    <img
                                        src={group.image || "/placeholder.svg"}
                                        alt={group.title}
                                        className="w-16 h-16 rounded-lg object-cover"
                                    />
                                    <div className="flex-1 min-w-0">
                                        <h3 className="text-foreground font-semibold text-lg">{group.title}</h3>
                                        <p className="text-muted-foreground text-sm">
                                            {group.game}
                                            <br/>
                                            by <span className="text-primary">{group.creator}</span>
                                        </p>
                                    </div>
                                </div>

                                {/* Card Body */}
                                <div className="p-4 space-y-3">
                                    <p className="text-muted-foreground text-sm line-clamp-4 leading-relaxed">
                                        {group.description}
                                    </p>

                                    <div className="flex items-center gap-2 flex-wrap">
                                    <span className="text-muted-foreground text-xs font-medium">TAGS</span>
                                        {group.tags.map((tag) => (
                                            <span
                                                key={tag}
                                                className="px-2 py-1 bg-muted text-muted-foreground text-xs rounded"
                                            >
                                    {tag}
                                </span>
                                        ))}
                                    </div>

                                    <div className="flex items-center justify-between pt-2">
                                        <div className="flex items-center gap-2 text-muted-foreground">
                                            <Users className="h-4 w-4" />
                                            <span className="text-sm">
                                    {group.currentMembers}/{group.maxMembers}
                                </span>
                                        </div>

                                        {group.isPending ? (
                                            <Button
                                                size="sm"
                                                variant="destructive"
                                            >
                                                <X className="h-4 w-4" />
                                            </Button>
                                        ) : (
                                            <Button
                                                size="sm"
                                                className="bg-primary hover:bg-primary/90 text-primary-foreground h-9 px-6"
                                            >
                                                Join
                                            </Button>
                                        )}
                                    </div>
                                </div>
                            </div>
                        ))}
                    </div>
                </div>
            </div>

        </div>
    )
}
