import { X, Users } from "lucide-react"
import { Button } from "@/components/ui/button"
import type { GroupCard as GroupCardType } from "./types"
import { getImageUrl } from "@/utils/imageUtils.ts"
import { useNavigate } from "react-router-dom"
import { TeamMemberStatus } from "@/api/types"
import { useAuthStore } from "@/stores/authStore"
import { useMutation, useQueryClient } from "@tanstack/react-query"
import { requestToJoinTeam, cancelJoinRequest } from "@/api/api"

interface GroupCardProps {
    group: GroupCardType
    onUpdate?: () => void
}

export function GroupCard({ group, onUpdate }: GroupCardProps) {
    const navigate = useNavigate()
    const { user } = useAuthStore()
    const queryClient = useQueryClient()

    const joinMutation = useMutation({
        mutationFn: () => requestToJoinTeam(group.id, parseInt(user?.id || '0')),
        onSuccess: () => {
            onUpdate?.()
            queryClient.invalidateQueries({ queryKey: ['teams'] })
        },
    })

    const cancelMutation = useMutation({
        mutationFn: () => cancelJoinRequest(group.id, parseInt(user?.id || '0')),
        onSuccess: () => {
            onUpdate?.()
            queryClient.invalidateQueries({ queryKey: ['teams'] })
        },
    })

    const handleOpenClick = () => {
        navigate(`/my-groups/${group.id}`)
    }

    const handleJoinClick = () => {
        if (user?.id) {
            joinMutation.mutate()
        }
    }

    const handleCancelClick = () => {
        if (user?.id) {
            cancelMutation.mutate()
        }
    }

    const renderButton = () => {
        if (group.teamMemberStatus === TeamMemberStatus.STATUS_LEADER ||
            group.teamMemberStatus === TeamMemberStatus.STATUS_MEMBER) {
            return (
                <Button
                    size="sm"
                    onClick={handleOpenClick}
                    className="bg-green-600 hover:bg-green-700 text-white h-9 px-6"
                >
                    Open
                </Button>
            )
        }

        if (group.teamMemberStatus === TeamMemberStatus.STATUS_PENDING) {
            return (
                <Button
                    size="sm"
                    variant="secondary"
                    onClick={handleCancelClick}
                    disabled={cancelMutation.isPending}
                    className="bg-muted hover:bg-muted/80 text-muted-foreground h-9 px-6"
                >
                    Cancel <X className="h-4 w-4 ml-1" />
                </Button>
            )
        }

        return (
            <Button
                size="sm"
                onClick={handleJoinClick}
                disabled={joinMutation.isPending}
                className="bg-orange-600 hover:bg-orange-700 text-white h-9 px-6"
            >
                {joinMutation.isPending ? 'Joining...' : 'Join'}
            </Button>
        )
    }

    return (
        <div className="bg-card rounded-lg overflow-hidden border border-border hover:border-border transition-colors cursor-pointer flex flex-col h-full">
            {/* Card Header */}
            <div className="flex gap-3 p-4 border-b border-border">
                <img
                    src={getImageUrl(group.image) || "/assets/sunset-silhouette-gaming.jpg"}
                    alt={group.title}
                    className="w-16 h-16 rounded-lg object-cover"
                    onError={(e) => {
                        (e.target as HTMLImageElement).src = "/assets/sunset-silhouette-gaming.jpg"
                    }}
                />
                <div className="flex-1 min-w-0">
                    <h3 className="text-foreground font-semibold text-lg">{group.title}</h3>
                    <p className="text-muted-foreground text-sm">
                        {typeof group.game === 'object' ? (group.game as any).name : group.game}
                        <br/>
                        by <span className="text-primary">{group.teamLeaderUsername}</span>
                    </p>
                </div>
            </div>

            {/* Card Body */}
            <div className="p-4 space-y-3 flex-1 flex flex-col">
                <div className="flex items-center gap-2">
                    <span className={`px-2 py-1 text-xs font-medium rounded ${
                        group.autoAccept
                            ? 'bg-green-500/10 text-green-600 dark:text-green-400'
                            : 'bg-orange-500/10 text-orange-600 dark:text-orange-400'
                    }`}>
                        {group.autoAccept ? 'Auto-Accept' : 'Manual Approval'}
                    </span>
                </div>

                <p className="text-muted-foreground text-sm line-clamp-4 leading-relaxed">
                    {group.description}
                </p>

                <div className="flex items-center gap-2 flex-wrap">
                    <span className="text-muted-foreground text-xs font-medium">TAGS</span>
                    {group.tags.map((tag: any) => (
                        <span
                            key={tag.id || tag}
                            className="px-2 py-1 bg-muted text-muted-foreground text-xs rounded"
                        >
                            {typeof tag === 'object' ? tag.name : tag}
                        </span>
                    ))}
                </div>

                <div className="flex items-center justify-between pt-2 mt-auto">
                    <div className="flex items-center gap-2 text-muted-foreground">
                        <Users className="h-4 w-4" />
                        <span className="text-sm">
                            {group.currentMembers}/{group.maxMembers}
                        </span>
                    </div>

                    {renderButton()}
                </div>
            </div>
        </div>
    )
}