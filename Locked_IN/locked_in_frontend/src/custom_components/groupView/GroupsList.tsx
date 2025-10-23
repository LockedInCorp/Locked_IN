"use client"

import { X } from "lucide-react"
import { Avatar, AvatarFallback, AvatarImage } from "@/components/ui/avatar"
import { Input } from "@/components/ui/input"
import { Button } from "@/components/ui/button"

const groups = [
    {
        id: 1,
        name: "Group name",
        lastMessage: "Last message",
        date: "12/12/2025",
        avatar: "/diverse-group-avatars.png",
    },
    {
        id: 2,
        name: "Group name",
        lastMessage: "Last message",
        user: "User",
        date: "12/12/2025",
        avatar: "/diverse-group-avatars.png",
    },
    {
        id: 3,
        name: "Group name",
        lastMessage: "Last message",
        user: "User",
        date: "12/12/2025",
        avatar: "/diverse-group-avatars.png",
    },
    {
        id: 4,
        name: "Group name",
        lastMessage: "Last message",
        user: "User",
        date: "12/12/2025",
        avatar: "/diverse-group-avatars.png",
    },
]

export function GroupsList() {
    return (
        <div className="flex flex-col h-full bg-background">
            {/* Header */}
            <div className="flex items-center justify-between px-6 py-5 border-b border-border">
                <div className="flex items-center gap-6">
                    <h1 className="text-xl font-bold text-primary">Locked IN!</h1>
                    <div className="flex items-center gap-4 text-sm text-foreground">
                        <button className="hover:text-primary transition-colors">Discover</button>
                        <button className="hover:text-primary transition-colors">MyGroups</button>
                    </div>
                </div>
                <Avatar className="h-8 w-8">
                    <AvatarImage src="/diverse-user-avatars.png" />
                    <AvatarFallback>U</AvatarFallback>
                </Avatar>
            </div>

            {/* Groups Section */}
            <div className="flex-1 overflow-y-auto px-6 py-6">
                <h2 className="text-2xl font-bold text-primary mb-6">Your Groups</h2>

                {/* Search Bar */}
                <div className="relative mb-4">
                    <Input
                        placeholder="Search by name..."
                        className="bg-muted border-0 pr-10 text-foreground placeholder:text-muted-foreground"
                    />
                    <Button
                        size="icon"
                        variant="ghost"
                        className="absolute right-2 top-1/2 -translate-y-1/2 h-6 w-6 text-muted-foreground hover:text-foreground"
                    >
                        <X className="h-4 w-4" />
                    </Button>
                </div>

                {/* Groups List */}
                <div className="space-y-3">
                    {groups.map((group) => (
                        <div
                            key={group.id}
                            className="flex items-center gap-3 p-4 bg-muted rounded-xl hover:bg-muted/80 transition-colors cursor-pointer"
                        >
                            <Avatar className="h-12 w-12 flex-shrink-0">
                                <AvatarImage src={group.avatar || "/placeholder.svg"} />
                                <AvatarFallback>G</AvatarFallback>
                            </Avatar>
                            <div className="flex-1 min-w-0">
                                <div className="flex items-center justify-between mb-1">
                                    <h3 className="font-semibold text-foreground">{group.name}</h3>
                                    <span className="text-xs text-muted-foreground">{group.date}</span>
                                </div>
                                <p className="text-sm text-muted-foreground truncate">
                                    {group.user && <span className="text-primary">{group.user}: </span>}
                                    {group.lastMessage}
                                </p>
                            </div>
                        </div>
                    ))}
                </div>
            </div>
        </div>
    )
}
