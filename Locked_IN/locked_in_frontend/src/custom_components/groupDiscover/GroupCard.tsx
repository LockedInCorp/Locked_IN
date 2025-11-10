import { X, Users } from "lucide-react"
import { Button } from "@/components/ui/button"
import type { GroupCard as GroupCardType } from "./types"

interface GroupCardProps {
    group: GroupCardType
}

export function GroupCard({ group }: GroupCardProps) {
    return (
        <div className="bg-card rounded-lg overflow-hidden border border-border hover:border-border transition-colors cursor-pointer">
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
    )
}

