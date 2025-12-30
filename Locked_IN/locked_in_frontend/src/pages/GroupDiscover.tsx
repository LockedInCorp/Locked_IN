"use client"

import { useState } from "react"
import { DiscoverSidebar } from "@/custom_components/groupDiscover/DiscoverSidebar"
import { DiscoverFilters } from "@/custom_components/groupDiscover/DiscoverFilters"
import { GroupCardGrid } from "@/custom_components/groupDiscover/GroupCardGrid"
import type { GroupCard } from "@/custom_components/groupDiscover/types"

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
]

export default function DiscoverPage() {
    const [groupSearch, setGroupSearch] = useState("")
    const [showPending, setShowPending] = useState(false)

    return (
        <div className="flex h-full overflow-hidden">
            <DiscoverSidebar />

            <div className="flex-1 flex flex-col p-6 overflow-hidden">
                <DiscoverFilters
                    groupSearch={groupSearch}
                    onGroupSearchChange={setGroupSearch}
                    showPending={showPending}
                    onShowPendingChange={setShowPending}
                />

                <GroupCardGrid groups={mockGroups} />
            </div>
        </div>
    )
}
