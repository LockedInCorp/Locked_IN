"use client"

import { X } from "lucide-react"
import { useNavigate } from "react-router-dom"
import { Avatar, AvatarFallback, AvatarImage } from "@/components/ui/avatar"
import { Input } from "@/components/ui/input"
import { Button } from "@/components/ui/button"

const groups = [
    {
        id: 2,
        name: "Group name",
        lastMessage: "Last message",
        user: "User",
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
        id: 2,
        name: "Group name",
        lastMessage: "Last message",
        user: "User",
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
        id: 2,
        name: "Group name",
        lastMessage: "Last message",
        user: "User",
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
]

export function GroupsList() {
    const navigate = useNavigate();

    const handleCreateGroup = () => {
        navigate("/creation");
    }

    return (
        <div className="flex flex-col h-full bg-background">
            {/* Header Section - Fixed */}
            <div className="px-6 pt-6 pb-4 flex-shrink-0">
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
            </div>

            {/* Groups List OR Empty State - Scrollable */}
            <div className="flex-1 overflow-y-auto px-6 pb-6">
                {groups.length > 0 ? (
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
                ) : (
                    <div className="flex flex-col items-center justify-center mt-20 text-center text-muted-foreground">
                        <p className="text-lg mb-4">You don't have any groups yet.</p>
                        <Button
                            onClick={handleCreateGroup}
                            className="bg-primary text-primary-foreground hover:bg-primary/90"
                        >
                            Create Group
                        </Button>
                    </div>
                )}
            </div>
        </div>
    )
}
