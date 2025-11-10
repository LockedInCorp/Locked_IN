import type { GroupCard } from "./types"
import { GroupCard as GroupCardComponent } from "./GroupCard"

interface GroupCardGridProps {
    groups: GroupCard[]
}

export function GroupCardGrid({ groups }: GroupCardGridProps) {
    return (
        <div className="flex-1 overflow-y-auto pr-2">
            <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
                {groups.map((group) => (
                    <GroupCardComponent key={group.id} group={group} />
                ))}
            </div>
        </div>
    )
}
