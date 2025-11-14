"use client"

import { ChevronDown, ChevronUp, MoreHorizontal, UserPlus, Users, UserMinus, Check, X } from "lucide-react"
import { Avatar, AvatarFallback, AvatarImage } from "@/components/ui/avatar"
import { Button } from "@/components/ui/button"
import { useState } from "react"

const members = [
    { id: 1, name: "Friend Name", avatar: "/diverse-user-avatars.png" },
    { id: 2, name: "Friend N", avatar: "/diverse-user-avatars.png" },
    { id: 3, name: "Friend Name", avatar: "/diverse-user-avatars.png" },
]

const requests = [
    { id: 1, name: "Name", avatar: "/diverse-user-avatars.png" },
    { id: 2, name: "Name", avatar: "/diverse-user-avatars.png" },
    { id: 3, name: "Name", avatar: "/diverse-user-avatars.png" },
]

export function GroupInfo() {
    const [membersExpanded, setMembersExpanded] = useState(true)
    const [requestsExpanded, setRequestsExpanded] = useState(true)

    return (
        <div className="flex flex-col h-min bg-background overflow-y-auto">
            {/* Group Header */}
            <div className="px-6 py-6 border-b border-border">
                <div className="flex items-center gap-3 mb-4">
                    <Avatar className="h-12 w-12">
                        <AvatarImage src="/diverse-group-avatars.png" />
                        <AvatarFallback>G</AvatarFallback>
                    </Avatar>
                    <div>
                        <h2 className="font-semibold text-foreground">Group name</h2>
                        <p className="text-xs text-muted-foreground">5 members</p>
                    </div>
                </div>

                {/* Tags */}
                <div className="flex gap-2 mb-3">
                    <span className="px-3 py-1 bg-muted text-foreground text-sm rounded-full">Chill</span>
                    <span className="px-3 py-1 bg-muted text-foreground text-sm rounded-full">Competitive</span>
                </div>

                {/* Experience */}
                <div className="mb-2">
                    <span className="text-sm text-muted-foreground">Experience: </span>
                    <span className="text-sm font-semibold text-primary">999 lvl</span>
                </div>

                {/* Description */}
                <div className="mb-2">
                    <p className="text-sm text-muted-foreground">Description</p>
                </div>

                {/* Communication Service */}
                <div>
                    <p className="text-sm text-muted-foreground">Communication Service</p>
                </div>
            </div>

            {/* Games Section */}
            <div className="px-6 py-4 border-b border-border">
                <h3 className="text-sm font-semibold text-foreground mb-3">Games we play</h3>
                <div className="flex gap-2">
                    <span className="px-3 py-1 bg-muted text-foreground text-sm rounded-full">Terraria</span>
                    <span className="px-3 py-1 bg-muted text-foreground text-sm rounded-full">Overwatch</span>
                </div>
            </div>

            {/* Members Section */}
            <div className="px-6 py-4 border-b border-border">
                <button
                    onClick={() => setMembersExpanded(!membersExpanded)}
                    className="flex items-center justify-between w-full mb-3 text-sm font-semibold text-foreground"
                >
                    <div className="flex items-center gap-2">
                        <Users className="h-4 w-4" />
                        <span>[10] members</span>
                    </div>
                    {membersExpanded ? <ChevronUp className="h-4 w-4" /> : <ChevronDown className="h-4 w-4" />}
                </button>

                {membersExpanded && (
                    <div className="space-y-2">
                        {members.map((member) => (
                            <div key={member.id} className="flex items-center justify-between">
                                <div className="flex items-center gap-2">
                                    <Avatar className="h-8 w-8">
                                        <AvatarImage src={member.avatar || "/placeholder.svg"} />
                                        <AvatarFallback>F</AvatarFallback>
                                    </Avatar>
                                    <span className="text-sm text-foreground">{member.name}</span>
                                </div>
                                <div className="flex items-center gap-1">
                                    <Button size="icon" variant="ghost" className="h-6 w-6 text-muted-foreground hover:text-foreground">
                                        <UserPlus className="h-3 w-3" />
                                    </Button>
                                    <Button size="icon" variant="ghost" className="h-6 w-6 text-muted-foreground hover:text-foreground">
                                        <Users className="h-3 w-3" />
                                    </Button>
                                    <Button size="icon" variant="ghost" className="h-6 w-6 text-muted-foreground hover:text-foreground">
                                        <UserMinus className="h-3 w-3" />
                                    </Button>
                                    <Button size="icon" variant="ghost" className="h-6 w-6 text-muted-foreground hover:text-foreground">
                                        <MoreHorizontal className="h-3 w-3" />
                                    </Button>
                                </div>
                            </div>
                        ))}
                    </div>
                )}
            </div>

            {/* Requests Section */}
            <div className="px-6 py-4 flex-1">
                <button
                    onClick={() => setRequestsExpanded(!requestsExpanded)}
                    className="flex items-center justify-between w-full mb-3 text-sm font-semibold text-foreground"
                >
                    <div className="flex items-center gap-2">
                        <Users className="h-4 w-4" />
                        <span>[10] requests</span>
                    </div>
                    {requestsExpanded ? <ChevronUp className="h-4 w-4" /> : <ChevronDown className="h-4 w-4" />}
                </button>

                {requestsExpanded && (
                    <div className="space-y-2">
                        {requests.map((request) => (
                            <div key={request.id} className="flex items-center justify-between">
                                <div className="flex items-center gap-2">
                                    <Avatar className="h-8 w-8">
                                        <AvatarImage src={request.avatar || "/placeholder.svg"} />
                                        <AvatarFallback>N</AvatarFallback>
                                    </Avatar>
                                    <span className="text-sm text-foreground">{request.name}</span>
                                </div>
                                <div className="flex items-center gap-1">
                                    <Button size="icon" variant="ghost" className="h-6 w-6 text-green-500 hover:text-green-400">
                                        <Check className="h-4 w-4" />
                                    </Button>
                                    <Button size="icon" variant="ghost" className="h-6 w-6 text-red-500 hover:text-red-400">
                                        <X className="h-4 w-4" />
                                    </Button>
                                </div>
                            </div>
                        ))}
                    </div>
                )}
            </div>

            {/* Action Buttons */}
            <div className="px-6 py-4 border-t border-border flex gap-3">
                <Button
                    variant="outline"
                    className="flex-1 border-primary text-primary hover:bg-primary hover:text-primary-foreground bg-transparent"
                >
                    Leave
                </Button>
                <Button className="flex-1 bg-primary text-primary-foreground hover:bg-primary/90">Edit</Button>
            </div>
        </div>
    )
}
