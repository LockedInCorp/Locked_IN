"use client"

import { ImageIcon, Send } from "lucide-react"
import { useEffect, useState } from "react"
import { useParams } from "react-router-dom"
import { Avatar, AvatarFallback, AvatarImage } from "@/components/ui/avatar"
import { Button } from "@/components/ui/button"
import { Input } from "@/components/ui/input"
import { useGroupViewStore } from "@/stores/groupViewStore"
import { getChatMessages, getGroupDetails } from "@/api/api"

export function ChatArea() {
    const { chatId } = useParams<{ chatId: string }>()
    const { messages, setMessages } = useGroupViewStore()
    const [groupName, setGroupName] = useState("Loading...")
    const [groupIcon, setGroupIcon] = useState("/diverse-group-avatars.png")

    useEffect(() => {
        if (!chatId) return

        const fetchChatData = async () => {
            try {
                const [msgData, groupData] = await Promise.all([
                    getChatMessages(chatId),
                    getGroupDetails(chatId)
                ])

                setMessages(msgData.map(m => ({
                    id: m.id,
                    sender: m.senderUsername,
                    content: m.content,
                    isCurrentUser: m.isCurrentUser
                })))

                setGroupName(groupData.name)
                setGroupIcon(groupData.avatarUrl || "/diverse-group-avatars.png")
            } catch (error) {
                console.error("Failed to fetch chat data:", error)
            }
        }

        fetchChatData()
    }, [chatId, setMessages])

    const groups = messages.reduce<
        { sender: string; isCurrentUser: boolean; items: any[] }[]
    >((acc, m) => {
        const last = acc[acc.length - 1]
        if (last && last.sender === m.sender && last.isCurrentUser === m.isCurrentUser) {
            last.items.push(m)
        } else {
            acc.push({ sender: m.sender, isCurrentUser: m.isCurrentUser, items: [m] })
        }
        return acc
    }, [])
    return (
        <div className="flex flex-col h-svh">
            {/* Header */}
            <div className="flex items-center justify-between px-6 py-4 border-b border-border">
                <div className="flex items-center gap-3">
                    <Avatar className="h-10 w-10">
                        <AvatarImage src={groupIcon} />
                        <AvatarFallback>G</AvatarFallback>
                    </Avatar>
                    <div>
                        <h2 className="font-semibold text-foreground">{groupName}</h2>
                        <p className="text-xs text-muted-foreground">icon</p>
                    </div>
                </div>
            </div>

            {/* Chat Area */}
            <div className="flex-1 overflow-y-auto px-6 py-6 space-y-6">
                {groups.map((g, gi) => (
                    <div
                        key={`${g.sender}-${gi}`}
                        className={`flex ${g.isCurrentUser ? "justify-end" : "justify-start"}`}
                    >
                        {!g.isCurrentUser && (
                            <Avatar className="h-10 w-10 mr-3 mt-[2px] shrink-0">
                                <AvatarFallback className="bg-muted text-foreground">
                                    {g.sender[0]}
                                </AvatarFallback>
                            </Avatar>
                        )}

                        <div className={`max-w-xl flex flex-col ${g.isCurrentUser ? "items-end" : "items-start"}`}>
                            {/* show sender once per group  */}
                            {!g.isCurrentUser && (
                                <span className="text-xs text-primary mb-1">{g.sender}</span>
                            )}

                            {/* the bubbles inside this group */}
                            <div className="flex flex-col gap-1.5">
                                {g.items.map((m, idx) => {
                                    const first = idx === 0
                                    const last = idx === g.items.length - 1

                                    // slightly different rounding so stacked bubbles look connected
                                    const bubbleRadius = g.isCurrentUser
                                        ? [
                                            "rounded-tl-2xl rounded-bl-2xl",
                                            first ? "rounded-tr-2xl" : "rounded-tr-lg",
                                            last ? "rounded-br-2xl" : "rounded-br-lg",
                                        ].join(" ")
                                        : [
                                            "rounded-tr-2xl rounded-br-2xl",
                                            first ? "rounded-tl-2xl" : "rounded-tl-lg",
                                            last ? "rounded-bl-2xl" : "rounded-bl-lg",
                                        ].join(" ")

                                    return (
                                        <div
                                            key={m.id}
                                            className={`px-4 py-2 bg-muted text-foreground ${bubbleRadius}`}
                                        >
                                            {m.content}
                                        </div>
                                    )
                                })}
                            </div>
                        </div>
                    </div>
                ))}
            </div>

            {/* Input area */}
            <div className="px-6 py-4 border-t border-border">
                <div className="flex items-center gap-3 bg-muted rounded-full px-5 py-3">
                    <Input
                        placeholder="Write a message..."
                        className="flex-1 bg-transparent border-0 focus-visible:ring-0 focus-visible:ring-offset-0 text-foreground placeholder:text-muted-foreground"
                    />
                    <Button size="icon" variant="ghost" className="h-8 w-8 text-muted-foreground hover:text-foreground shrink-0">
                        <ImageIcon className="h-5 w-5" />
                    </Button>
                    <Button size="icon" variant="ghost" className="h-8 w-8 text-muted-foreground hover:text-foreground shrink-0">
                        <Send className="h-5 w-5" />
                    </Button>
                </div>
            </div>
        </div>
    )
}
