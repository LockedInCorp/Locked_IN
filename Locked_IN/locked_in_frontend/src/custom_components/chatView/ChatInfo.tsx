"use client"

import { useState } from "react"
import { useNavigate, useParams } from "react-router-dom"
import { ChevronDown, ChevronUp, MoreHorizontal, UserPlus, Users, UserMinus, Check, X } from "lucide-react"
import { Avatar, AvatarFallback, AvatarImage } from "@/components/ui/avatar"
import { Button } from "@/components/ui/button"
import { ChatType } from "@/api/types"
import { getImageUrl } from "@/utils/imageUtils"
import { useChatDetails } from "@/hooks/chat/useChatDetails"
import { useGroupDetails } from "@/hooks/chat/useGroupDetails"
import { useJoinRequests } from "@/hooks/chat/useJoinRequests"
import { acceptJoinRequest, declineJoinRequest, leaveTeam } from "@/api/api"
import { useAuthStore } from "@/stores/authStore"
import { useQueryClient } from "@tanstack/react-query"

export function ChatInfo() {
    const navigate = useNavigate()
    const { chatId } = useParams<{ chatId: string }>()
    const queryClient = useQueryClient()
    
    const { user } = useAuthStore()
    
    const [membersExpanded, setMembersExpanded] = useState(true)
    const [requestsExpanded, setRequestsExpanded] = useState(true)

    const toggleMembersExpanded = () => setMembersExpanded(!membersExpanded)
    const toggleRequestsExpanded = () => setRequestsExpanded(!requestsExpanded)

    const numericChatId = chatId ? parseInt(chatId, 10) : null
    const { chatDetails, isLoading: isChatLoading } = useChatDetails(numericChatId)

    const teamId = chatDetails?.chatType === ChatType.Team ? chatDetails.teamId : null
    const { group, isLoading: groupLoading, refetch: refetchGroup } = useGroupDetails(teamId)
    
    const isLeader = user?.id && group?.leader?.id ? user.id === group.leader.id.toString() : false
    const { requests, isLoading: requestsLoading, refetch: refetchRequests } = useJoinRequests(teamId, isLeader)

    const handleEdit = () => {
        const groupId = teamId || "1"
        navigate(`/groups/edit/${groupId}`)
    }

    const handleViewProfile = () => {
        // TODO: Implement navigation to friend's profile
        // navigate(`/profile/${chatDetails?.id}`)
    }

    const handleAcceptRequest = async (userId: number) => {
        if (!teamId) return;
        try {
            await acceptJoinRequest(teamId, userId)
            refetchRequests()
            refetchGroup()
        } catch (error) {
            console.error("Failed to accept request:", error)
        }
    }

    const handleDeclineRequest = async (userId: number) => {
        if (!teamId) return
        try {
            await declineJoinRequest(teamId, userId)
            refetchRequests()
        } catch (error) {
            console.error("Failed to decline request:", error)
        }
    }

    const handleLeaveTeam = async () => {
        if (!teamId) return
        if (window.confirm("Are you sure you want to leave this team?")) {
            try {
                await leaveTeam(teamId)
                queryClient.invalidateQueries({ queryKey: ["userChats"] })
                navigate("/my-groups")
            } catch (error) {
                console.error("Failed to leave team:", error)
            }
        }
    }

    if (isChatLoading) return <div className="p-6 text-center text-muted-foreground">Loading info...</div>
    
    const isGroupChat = chatDetails?.chatType === ChatType.Team
    
    if (isGroupChat && !group) return <div className="p-6 text-center text-muted-foreground">{groupLoading ? 'Loading info...' : 'No group info available'}</div>
    if (!isGroupChat && !chatDetails) return <div className="p-6 text-center text-muted-foreground">No chat info available</div>
    
    if (!isGroupChat && chatDetails) {
        return (
            <div className="flex flex-col h-full bg-background overflow-y-auto">
                {/* Friend Header */}
                <div className="px-6 py-6 border-b border-border">
                    <div className="flex items-center gap-3 mb-4">
                        <Avatar className="h-16 w-16">
                            <AvatarImage 
                                src={getImageUrl(chatDetails?.chatIconUrl)}
                            />
                            <AvatarFallback>{chatDetails.chatName?.charAt(0).toUpperCase() || "F"}</AvatarFallback>
                        </Avatar>
                        <div>
                            <h2 className="text-xl font-semibold text-foreground">{chatDetails.chatName || "Friend"}</h2>
                        </div>
                    </div>
                </div>

                {/* Action Button */}
                <div className="px-6 py-4 border-t border-border mt-auto">
                    <Button 
                        onClick={handleViewProfile}
                        className="w-full bg-primary text-primary-foreground hover:bg-primary/90"
                    >
                        View profile
                    </Button>
                </div>
            </div>
        )
    }

    if (!group) return <div className="p-6 text-center text-muted-foreground">No group info available</div>

    return (
        <div className="flex flex-col h-full bg-background overflow-y-auto">
            {/* Group Header */}
            <div className="px-6 py-6 border-b border-border">
                <div className="flex items-center gap-3 mb-4">
                    <Avatar className="h-12 w-12">
                        <AvatarImage 
                            src={getImageUrl(group?.iconUrl)}
                        />
                        <AvatarFallback>1</AvatarFallback>
                    </Avatar>
                    <div>
                        <h2 className="font-semibold text-foreground">{group.name}</h2>
                        <p className="text-xs text-muted-foreground">{group.currentMemberCount} members</p>
                    </div>
                </div>

                {/* Tags */}
                <div className="flex flex-wrap gap-2 mb-3">
                    {group.preferenceTags.map((tag, idx) => (
                        <span key={idx} className="px-3 py-1 bg-muted text-foreground text-sm rounded-full">{tag.name}</span>
                    ))}
                </div>

                {/* Experience */}
                <div className="mb-2">
                    <span className="text-sm text-muted-foreground">Experience: </span>
                    <span className="text-sm font-semibold text-primary">{group.experienceLevel.name}</span>
                </div>

                {/* Description */}
                <p className="text-sm text-muted-foreground mb-2">{group.description || "No description"}</p>

                {/* Communication Service */}
                <p className="text-sm text-muted-foreground">{group.communicationService ? `Communication service: ${group.communicationService.name}` : "No communication service"}</p>
                {group.communicationServiceLink && (
                    <p className="text-sm text-muted-foreground">
                        Join link: <a href={group.communicationServiceLink} target="_blank" rel="noopener noreferrer" className="text-primary hover:underline">{group.communicationServiceLink}</a>
                    </p>
                )}
            </div>

            {/* Games Section */}
            <div className="px-6 py-4 border-b border-border">
                <h3 className="text-sm font-semibold text-foreground mb-3">Game we play</h3>
                <div className="flex flex-wrap gap-2">
                        <span className="px-3 py-1 bg-muted text-foreground text-sm rounded-full">{group.game.name}</span>
                </div>
            </div>

            {/* Members Section */}
            <div className="px-6 py-4 border-b border-border">
                <button
                    onClick={toggleMembersExpanded}
                    className="flex items-center justify-between w-full mb-3 text-sm font-semibold text-foreground cursor-pointer"
                >
                    <div className="flex items-center gap-2">
                        <Users className="h-4 w-4" />
                        <span>[{group.members.length}] members</span>
                    </div>
                    {membersExpanded ? <ChevronUp className="h-4 w-4" /> : <ChevronDown className="h-4 w-4" />}
                </button>

                {membersExpanded && (
                    <div className="space-y-2">
                        {group.members.map((member) => (
                            <div key={member.id} className="flex items-center justify-between">
                                <div className="flex items-center gap-2">
                                    <Avatar className="h-8 w-8">
                                        <AvatarImage src={getImageUrl(member.avatarUrl)} />
                                        <AvatarFallback>F</AvatarFallback>
                                    </Avatar>
                                    <span className="text-sm text-foreground">{member.username}</span>
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
            {isLeader && (
                <div className="px-6 py-4 flex-1">
                    <button
                        onClick={toggleRequestsExpanded}
                        className="flex items-center justify-between w-full mb-3 text-sm font-semibold text-foreground cursor-pointer"
                    >
                        <div className="flex items-center gap-2">
                            <Users className="h-4 w-4" />
                            <span>[{requestsLoading ? '...' : requests.length}] requests</span>
                        </div>
                        {requestsExpanded ? <ChevronUp className="h-4 w-4" /> : <ChevronDown className="h-4 w-4" />}
                    </button>

                    {requestsExpanded && (
                        <div className="space-y-2">
                            {requests.map((request) => (
                                <div key={request.userId} className="flex items-center justify-between">
                                    <div className="flex items-center gap-2">
                                        <Avatar className="h-8 w-8">
                                            <AvatarImage src={getImageUrl(request.avatarUrl) || "/placeholder.svg"} />
                                            <AvatarFallback>{request.username.charAt(0)}</AvatarFallback>
                                        </Avatar>
                                        <span className="text-sm text-foreground">{request.username}</span>
                                    </div>
                                    <div className="flex items-center gap-1">
                                        <Button 
                                            size="icon" 
                                            variant="ghost" 
                                            className="h-6 w-6 text-green-500 hover:text-green-400"
                                            onClick={() => handleAcceptRequest(request.userId)}
                                        >
                                            <Check className="h-4 w-4" />
                                        </Button>
                                        <Button 
                                            size="icon" 
                                            variant="ghost" 
                                            className="h-6 w-6 text-red-500 hover:text-red-400"
                                            onClick={() => handleDeclineRequest(request.userId)}
                                        >
                                            <X className="h-4 w-4" />
                                        </Button>
                                    </div>
                                </div>
                            ))}
                        </div>
                    )}
                </div>
            )}

            {/* Action Buttons */}
            <div className="px-6 py-4 border-t border-border flex gap-3">
                <Button
                    variant="outline"
                    className="flex-1 border-primary text-primary hover:bg-primary hover:text-primary-foreground bg-transparent"
                    onClick={handleLeaveTeam}
                >
                    Leave
                </Button>
                {isLeader && (
                    <Button 
                        onClick={handleEdit}
                        className="flex-1 bg-primary text-primary-foreground hover:bg-primary/90"
                    >
                        Edit
                    </Button>
                )}
            </div>
        </div>
    )
}
