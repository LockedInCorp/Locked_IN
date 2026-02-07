"use client"

import { useNavigate, useParams } from "react-router-dom"
import { useEffect, useState } from "react"
import { ChevronDown, ChevronUp, MoreHorizontal, UserPlus, Users, UserMinus, Check, X } from "lucide-react"
import { Avatar, AvatarFallback, AvatarImage } from "@/components/ui/avatar"
import { Button } from "@/components/ui/button"
import { useChatViewStore } from "@/stores/chatViewStore"
import { getGroupDetails, getUserChats } from "@/api/api"
import type {GroupDetailsDto, UserChatDto} from "@/api/types"
import { ChatType } from "@/api/types"
import { getImageUrl } from "@/utils/imageUtils"

export function ChatInfo() {
    const navigate = useNavigate()
    const { chatId } = useParams<{ chatId: string }>()
    const { selectedGroupId, chatType, membersExpanded, requestsExpanded, toggleMembersExpanded, toggleRequestsExpanded } = useChatViewStore()
    const [group, setGroup] = useState<GroupDetailsDto | null>(null)
    const [friendChat, setFriendChat] = useState<UserChatDto | null>(null)
    const [loading, setLoading] = useState(false)

    useEffect(() => {
        if (!chatId) {
            setGroup(null)
            setFriendChat(null)
            return
        }

        const fetchChatInfo = async () => {
            setLoading(true)
            setGroup(null)
            setFriendChat(null)
            try {
                if (chatType === ChatType.Team) {
                    const data = await getGroupDetails(chatId)
                    setGroup(data)
                    setFriendChat(null)
                } else if (chatType === ChatType.Direct) {
                    const chats = await getUserChats()
                    const chat = chats.find(c => c.id.toString() === chatId)
                    setFriendChat(chat || null)
                    setGroup(null)
                } else {
                    const chats = await getUserChats()
                    const chat = chats.find(c => c.id.toString() === chatId)
                    if (chat?.chatType === ChatType.Team) {
                        const data = await getGroupDetails(chatId)
                        setGroup(data)
                        setFriendChat(null)
                    } else {
                        setFriendChat(chat || null)
                        setGroup(null)
                    }
                }
            } catch (error) {
                console.error("Failed to fetch chat info:", error)
            } finally {
                setLoading(false)
            }
        }

        fetchChatInfo()
    }, [chatId, chatType])

    const handleEdit = () => {
        const groupId = selectedGroupId || "1"
        navigate(`/groups/edit/${groupId}`)
    }

    const handleViewProfile = () => {
        // TODO: Implement navigation to friend's profile
        // navigate(`/profile/${friendChat?.id}`)
    }

    if (loading) return <div className="p-6 text-center text-muted-foreground">Loading info...</div>
    
    const isGroupChat = chatType === ChatType.Team || (chatType === null && group !== null)
    
    if (isGroupChat && !group) return <div className="p-6 text-center text-muted-foreground">No group info available</div>
    if (!isGroupChat && !friendChat) return <div className="p-6 text-center text-muted-foreground">No chat info available</div>
    
    if (!isGroupChat && friendChat) {
        return (
            <div className="flex flex-col h-full bg-background overflow-y-auto">
                {/* Friend Header */}
                <div className="px-6 py-6 border-b border-border">
                    <div className="flex items-center gap-3 mb-4">
                        <Avatar className="h-16 w-16">
                            <AvatarImage 
                                src={getImageUrl(friendChat.chatIconUrl)} 
                                onError={(e) => {
                                    const target = e.target as HTMLImageElement;
                                    if (!target.src.includes("/assets/sunset-silhouette-gaming.jpg")) {
                                        target.src = "/assets/sunset-silhouette-gaming.jpg";
                                    }
                                }}
                            />
                            <AvatarFallback>{friendChat.chatName?.charAt(0).toUpperCase() || "F"}</AvatarFallback>
                        </Avatar>
                        <div>
                            <h2 className="text-xl font-semibold text-foreground">{friendChat.chatName || "Friend"}</h2>
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
                            src={getImageUrl(group.avatarUrl)} 
                            onError={(e) => {
                                const target = e.target as HTMLImageElement;
                                if (!target.src.includes("/assets/sunset-silhouette-gaming.jpg")) {
                                    target.src = "/assets/sunset-silhouette-gaming.jpg";
                                }
                            }}
                        />
                        <AvatarFallback>G</AvatarFallback>
                    </Avatar>
                    <div>
                        <h2 className="font-semibold text-foreground">{group.name}</h2>
                        <p className="text-xs text-muted-foreground">{group.memberCount} members</p>
                    </div>
                </div>

                {/* Tags */}
                <div className="flex flex-wrap gap-2 mb-3">
                    {group.tags.map((tag, idx) => (
                        <span key={idx} className="px-3 py-1 bg-muted text-foreground text-sm rounded-full">{tag}</span>
                    ))}
                </div>

                {/* Experience */}
                <div className="mb-2">
                    <span className="text-sm text-muted-foreground">Experience: </span>
                    <span className="text-sm font-semibold text-primary">{group.experience}</span>
                </div>

                {/* Description */}
                <p className="text-sm text-muted-foreground mb-2">{group.description || "No description"}</p>

                {/* Communication Service */}
                <p className="text-sm text-muted-foreground">{group.communicationService ? `Communication service ID: ${group.communicationService}` : "No communication service"}</p>
                {group.communicationServiceLink && (
                    <p className="text-sm text-muted-foreground">
                        Join link: <a href={group.communicationServiceLink} target="_blank" rel="noopener noreferrer" className="text-primary hover:underline">{group.communicationServiceLink}</a>
                    </p>
                )}
            </div>

            {/* Games Section */}
            <div className="px-6 py-4 border-b border-border">
                <h3 className="text-sm font-semibold text-foreground mb-3">Games we play</h3>
                <div className="flex flex-wrap gap-2">
                    {group.games.map((game, idx) => (
                        <span key={idx} className="px-3 py-1 bg-muted text-foreground text-sm rounded-full">{game}</span>
                    ))}
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
                                        <AvatarImage src={getImageUrl(member.avatarUrl) || "/placeholder.svg"} />
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
            <div className="px-6 py-4 flex-1">
                <button
                    onClick={toggleRequestsExpanded}
                    className="flex items-center justify-between w-full mb-3 text-sm font-semibold text-foreground cursor-pointer"
                >
                    <div className="flex items-center gap-2">
                        <Users className="h-4 w-4" />
                        <span>[{group.requests.length}] requests</span>
                    </div>
                    {requestsExpanded ? <ChevronUp className="h-4 w-4" /> : <ChevronDown className="h-4 w-4" />}
                </button>

                {requestsExpanded && (
                    <div className="space-y-2">
                        {group.requests.map((request) => (
                            <div key={request.id} className="flex items-center justify-between">
                                <div className="flex items-center gap-2">
                                    <Avatar className="h-8 w-8">
                                        <AvatarImage src={getImageUrl(request.avatarUrl) || "/placeholder.svg"} />
                                        <AvatarFallback>N</AvatarFallback>
                                    </Avatar>
                                    <span className="text-sm text-foreground">{request.username}</span>
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
                <Button 
                    onClick={handleEdit}
                    className="flex-1 bg-primary text-primary-foreground hover:bg-primary/90"
                >
                    Edit
                </Button>
            </div>
        </div>
    )
}
