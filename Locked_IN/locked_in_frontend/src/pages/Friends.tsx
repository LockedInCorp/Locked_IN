"use client"

import { useState, useEffect } from "react"
import { useNavigate } from "react-router-dom"
import { Avatar, AvatarFallback, AvatarImage } from "@/lib/components/ui/avatar"
import { Button } from "@/lib/components/ui/button"
import { getImageUrl } from "@/utils/imageUtils"
import { useFriends } from "@/hooks/friendship/useFriends"
import { useAuthStore } from "@/stores/authStore"
import { updateUserAvailability, createDirectChat } from "@/api/api"
import { isCellAvailable, toggleHour } from "@/utils/friendship_and_availability/availabilityUtils"
import { formatDate, formatTimeAgo } from "@/utils/dateUtils"
import { Check, X, MessageCircle, User } from "lucide-react"

const DAYS_OF_WEEK = ["monday", "tuesday", "wednesday", "thursday", "friday", "saturday", "sunday"]
const DAY_LABELS = ["MO", "TU", "WE", "TH", "FR", "ST", "SU"]
const HOURS = Array.from({ length: 24 }, (_, i) => i.toString().padStart(2, '0'))

type SelectedTab = "user" | number

export default function Friends() {
    const navigate = useNavigate()
    const { user } = useAuthStore()
    const { 
        friends, 
        pendingRequests, 
        userAvailability, 
        loadFriendAvailability, 
        setUserAvailability, 
        processingRequest,
        handleAcceptRequest,
        handleDeclineRequest
    } = useFriends()
    const [selectedTab, setSelectedTab] = useState<SelectedTab>("user")
    const [editingAvailability, setEditingAvailability] = useState<Record<string, string[]>>({})
    const [hasChanges, setHasChanges] = useState(false)
    const [isSaving, setIsSaving] = useState(false)

    const handleStartDirectChat = async (targetUserId: number) => {
        try {
            const chat = await createDirectChat(targetUserId)
            navigate(`/my-groups/${chat.id}`)
        } catch (err) {
            console.error('Failed to create direct chat:', err)
            alert(err instanceof Error ? err.message : 'Failed to create direct chat')
        }
    }

    useEffect(() => {
        if (selectedTab === "user") {
            setEditingAvailability({ ...userAvailability })
            setHasChanges(false)
        } else if (typeof selectedTab === "number") {
            const friend = friends.find(f => f.friendId === selectedTab)
            if (friend && !friend.availability) {
                loadFriendAvailability(friend.friendId)
            }
        }
    }, [selectedTab, userAvailability, friends, loadFriendAvailability])

    const handleToggleHour = (day: string, hour: number) => {
        if (selectedTab !== "user") return

        setEditingAvailability(prev => {
            const newAvailability = toggleHour(day, hour, prev)
            setHasChanges(true)
            return newAvailability
        })
    }

    const handleSave = async () => {
        if (!user?.id) return

        setIsSaving(true)
        try {
            await updateUserAvailability(editingAvailability)
            setUserAvailability(editingAvailability)
            setHasChanges(false)
        } catch (err) {
            console.error('Failed to save availability:', err)
            alert(err instanceof Error ? err.message : 'Failed to save availability')
        } finally {
            setIsSaving(false)
        }
    }

    const getCurrentAvailability = (): Record<string, string[]> => {
        if (selectedTab === "user") {
            return editingAvailability
        } else {
            const friend = friends.find(f => f.friendId === selectedTab)
            return friend?.availability || {}
        }
    }


    const currentAvailability = getCurrentAvailability()
    const isEditing = selectedTab === "user"

    return (
        <div className="flex h-full bg-background">
            {/* Left Panel - Friends List */}
            <div className="w-[440px] border-r border-border flex-shrink-0 flex flex-col">
                {/* My Friends Section */}
                <div className="flex-1 overflow-y-auto px-6 pt-6 pb-4">
                    <h2 className="text-2xl font-bold text-primary mb-6">My Friends</h2>
                    
                    {/* User's Availability Tab */}
                    <div
                        onClick={() => setSelectedTab("user")}
                        className={`flex items-center gap-3 p-4 rounded-xl mb-4 cursor-pointer transition-colors ${
                            selectedTab === "user" 
                                ? "bg-muted" 
                                : "hover:bg-muted/50"
                        }`}
                    >
                        <Avatar className="h-12 w-12 flex-shrink-0">
                            <AvatarImage src={getImageUrl(user?.avatarUrl)} />
                            <AvatarFallback>{user?.nickname.charAt(0).toUpperCase() || "U"}</AvatarFallback>
                        </Avatar>
                        <div className="flex-1 min-w-0">
                            <h3 className="font-semibold text-foreground">{user?.nickname || "You"}</h3>
                            <p className="text-sm text-muted-foreground">Your availability</p>
                        </div>
                    </div>

                    {/* Friends List */}
                    <div className="space-y-3">
                        {friends.map((friend) => (
                            <div
                                key={friend.friendId}
                                onClick={() => setSelectedTab(friend.friendId)}
                                className={`flex items-center gap-3 p-4 rounded-xl cursor-pointer transition-colors ${
                                    selectedTab === friend.friendId 
                                        ? "bg-muted" 
                                        : "hover:bg-muted/50"
                                }`}
                            >
                                <Avatar className="h-12 w-12 flex-shrink-0">
                                    <AvatarImage src={getImageUrl(friend.friendAvatarUrl)} />
                                    <AvatarFallback>{friend.friendUsername.charAt(0).toUpperCase()}</AvatarFallback>
                                </Avatar>
                                <div className="flex-1 min-w-0">
                                    <h3 className="font-semibold text-foreground mb-0 leading-none">{friend.friendUsername}</h3>
                                    <span className="text-xs text-muted-foreground">Friends since: {formatDate(friend.since)}</span>
                                </div>
                                <div className="flex gap-2">
                                    <Button
                                        size="icon"
                                        variant="ghost"
                                        onClick={() => navigate(`/friend/${friend.friendId}`)}
                                        className="h-8 w-8 text-muted-foreground hover:text-foreground"
                                    >
                                        <User className="h-4 w-4" />
                                    </Button>
                                    <Button
                                        size="icon"
                                        variant="ghost"
                                        onClick={() => handleStartDirectChat(friend.friendId)}
                                        className="h-8 w-8 text-muted-foreground hover:text-foreground"
                                    >
                                        <MessageCircle className="h-4 w-4" />
                                    </Button>
                                </div>
                            </div>
                        ))}
                    </div>
                </div>

                {/* Incoming Requests Section */}
                <div className="border-t border-border px-6 py-4 flex-shrink-0">
                    <h3 className="text-lg font-semibold text-foreground mb-4">Incoming Requests</h3>
                    {pendingRequests.length > 0 ? (
                        <div className="space-y-3">
                            {pendingRequests.map((request) => (
                                <div
                                    key={request.friendshipId}
                                    className="flex items-center justify-between p-3 rounded-xl bg-muted/50"
                                >
                                    <div className="flex-1 min-w-0">
                                        <p className="font-medium text-foreground">{request.requesterUsername}</p>
                                        <p className="text-xs text-muted-foreground">{formatTimeAgo(request.requestTimestamp)}</p>
                                    </div>
                                    <div className="flex gap-2">
                                        <Button
                                            size="icon"
                                            variant="ghost"
                                            onClick={() => handleAcceptRequest(request.friendshipId)}
                                            disabled={processingRequest === request.friendshipId}
                                            className="h-8 w-8 text-green-600 hover:text-green-700 hover:bg-green-600/10 disabled:opacity-50"
                                        >
                                            <Check className="h-4 w-4" />
                                        </Button>
                                        <Button
                                            size="icon"
                                            variant="ghost"
                                            onClick={() => handleDeclineRequest(request.friendshipId)}
                                            disabled={processingRequest === request.friendshipId}
                                            className="h-8 w-8 text-red-600 hover:text-red-700 hover:bg-red-600/10 disabled:opacity-50"
                                        >
                                            <X className="h-4 w-4" />
                                        </Button>
                                    </div>
                                </div>
                            ))}
                        </div>
                    ) : (
                        <p className="text-sm text-muted-foreground">You don&apos;t have any friend requests yet.</p>
                    )}
                </div>
            </div>

            {/* Right Panel - Availability Calendar */}
            <div className="flex-1 flex flex-col min-w-0">
                <div className="flex-1 overflow-auto p-6">
                    <div className="mb-4">
                        <h2 className="text-2xl font-bold text-foreground mb-2">
                            {selectedTab === "user" 
                                ? `${user?.nickname}'s Availability` 
                                : friends.find(f => f.friendId === selectedTab)?.friendUsername + "'s Availability"}
                        </h2>
                        {isEditing && (
                            <div className="flex items-center gap-4">
                                <Button
                                    onClick={handleSave}
                                    disabled={!hasChanges || isSaving}
                                    className="bg-primary text-primary-foreground hover:bg-primary/90"
                                >
                                    {isSaving ? "Saving..." : "Save"}
                                </Button>
                                {hasChanges && (
                                    <span className="text-sm text-muted-foreground">You have unsaved changes</span>
                                )}
                            </div>
                        )}
                    </div>

                    {/* Calendar Grid */}
                    <div className="overflow-x-auto">
                        <div className="inline-block min-w-full border-2 border-foreground/20 rounded-lg overflow-hidden">
                            {/* Header Row - Hours */}
                            <div className="flex border-b-2 border-foreground/20">
                                <div className="w-16 flex-shrink-0 border-r-2 border-foreground/20"></div>
                                {HOURS.map((hour) => (
                                    <div
                                        key={hour}
                                        className="flex-1 min-w-[40px] text-center text-xs text-muted-foreground py-2 border-r border-border last:border-r-0"
                                    >
                                        {hour}
                                    </div>
                                ))}
                            </div>

                            {/* Day Rows */}
                            {DAYS_OF_WEEK.map((day, dayIndex) => (
                                <div key={day} className="flex border-b border-border last:border-b-0">
                                    {/* Day Label */}
                                    <div className="w-16 flex-shrink-0 flex items-center justify-center text-sm font-medium text-foreground border-r-2 border-foreground/20 py-2">
                                        {DAY_LABELS[dayIndex]}
                                    </div>

                                    {/* Hour Cells */}
                                    <div className="flex-1 flex relative">
                                        {HOURS.map((hourStr) => {
                                            const hour = parseInt(hourStr)
                                            const isAvailable = isCellAvailable(day, hour, currentAvailability)
                                            const isClickable = isEditing

                                            return (
                                                <div
                                                    key={hourStr}
                                                    onClick={() => isClickable && handleToggleHour(day, hour)}
                                                    className={`flex-1 min-w-[40px] h-12 border-r border-border last:border-r-0 ${
                                                        isClickable 
                                                            ? "cursor-pointer hover:bg-primary/40 transition-colors" 
                                                            : "cursor-default"
                                                    } ${
                                                        isAvailable 
                                                            ? "bg-primary/90" 
                                                            : ""
                                                    }`}
                                                    title={`${day} ${hourStr}:00`}
                                                />
                                            )
                                        })}
                                    </div>
                                </div>
                            ))}
                        </div>
                    </div>
                </div>
            </div>
        </div>
    )
}
