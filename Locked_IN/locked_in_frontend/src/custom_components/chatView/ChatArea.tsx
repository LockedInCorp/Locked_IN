"use client"

import { ImageIcon, Send } from "lucide-react"
import { useMemo, useRef, useState, useCallback, useEffect } from "react"
import { useParams } from "react-router-dom"
import { Avatar, AvatarFallback, AvatarImage } from "@/components/ui/avatar"
import { Button } from "@/components/ui/button"
import { Input } from "@/components/ui/input"
import { useChatDetails } from "@/hooks/chat/useChatDetails"
import { getImageUrl } from "@/utils/imageUtils.ts"
import { persist } from "@/utils/auth/persistance"
import { sendMessage } from "@/api/api.ts"

export function ChatArea() {
    const { chatId } = useParams<{ chatId: string }>()

    const numericChatId = chatId ? parseInt(chatId, 10) : null
    const {
        chatDetails,
        messages: allMessages,
        isFetchingMore,
        hasMore,
        fetchMore,
        page
    } = useChatDetails(numericChatId)

    const scrollContainerRef = useRef<HTMLDivElement>(null)
    const lastScrollHeightRef = useRef<number>(0)

    const handleScroll = useCallback(() => {
        if (!scrollContainerRef.current || !hasMore || isFetchingMore) return

        const { scrollTop } = scrollContainerRef.current
        if (scrollTop === 0) {
            lastScrollHeightRef.current = scrollContainerRef.current.scrollHeight
            fetchMore()
        }
    }, [hasMore, isFetchingMore, fetchMore])

    useEffect(() => {
        if (lastScrollHeightRef.current && scrollContainerRef.current && !isFetchingMore) {
            const newScrollHeight = scrollContainerRef.current.scrollHeight
            const heightDifference = newScrollHeight - lastScrollHeightRef.current
            if (heightDifference > 0) {
                scrollContainerRef.current.scrollTop = heightDifference
                lastScrollHeightRef.current = 0
            }
        }
    }, [allMessages, isFetchingMore])

    // Initial scroll to bottom
    useEffect(() => {
        if (allMessages.length > 0 && page === 1 && scrollContainerRef.current) {
            scrollContainerRef.current.scrollTop = scrollContainerRef.current.scrollHeight
        }
    }, [allMessages, page]) // Fix: trigger when allMessages or page changes

    const [messageText, setMessageText] = useState("")
    const [selectedFile, setSelectedFile] = useState<File | null>(null)
    const fileInputRef = useRef<HTMLInputElement | null>(null)

    const messages = useMemo(() => {
        const currentUser = persist.getUserData()

        return allMessages.map(m => {
            return ({
                id: m.id,
                sender: m.senderUsername,
                content: m.content,
                attachmentUrl: m.attachmentUrl,
                isCurrentUser: currentUser ? m.senderId.toString() === currentUser.id : false
            });
        })
    }, [allMessages])

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
    async function handleSend() {
        if (!numericChatId) return
        if (!messageText.trim() && !selectedFile) return
        try {
            await sendMessage({
                chatId: numericChatId,
                content: messageText.trim(),
                attachmentFile: selectedFile ?? undefined,
            })
            setMessageText("")
            setSelectedFile(null)
            if (fileInputRef.current) fileInputRef.current.value = ""
            
            // Reload first page to see the new message if we are at the bottom
            // or just append it locally for immediate feedback
            // For now, let's just refetch the first page
            // fetchMessages(1, true)
        } catch (e) {
            console.error(e)
        }
    }

    return (
        <div className="flex flex-col h-full overflow-hidden">
            {/* Header */}
            <div className="flex items-center justify-between px-6 py-4 border-b border-border shrink-0">
                <div className="flex items-center gap-3">
                    <Avatar className="h-10 w-10">
                        <AvatarImage src={getImageUrl(chatDetails?.chatIconUrl)} />
                        <AvatarFallback>{chatDetails?.chatName?.[0] || "G"}</AvatarFallback>
                    </Avatar>
                    <div>
                        <h2 className="font-semibold text-foreground">{chatDetails?.chatName || "Loading..."}</h2>
                        <p className="text-xs text-muted-foreground">{chatDetails?.chatType === 'Team' ? 'Group' : 'Direct'}</p>
                    </div>
                </div>
            </div>

            {/* Chat Area Messages */}
            <div 
                ref={scrollContainerRef}
                onScroll={handleScroll}
                className="flex-1 overflow-y-auto px-6 py-6 space-y-6 min-h-0"
            >
                {isFetchingMore && (
                    <div className="text-center text-xs text-muted-foreground py-2">
                        Loading older messages...
                    </div>
                )}
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
                                            {m.attachmentUrl && (
                                                <div className="mb-2 max-w-sm overflow-hidden rounded-lg">
                                                    <img
                                                        src={getImageUrl(m.attachmentUrl)}
                                                        alt="Attachment"
                                                        className="w-full h-auto object-cover"
                                                    />
                                                </div>
                                            )}
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
            <div className="px-6 py-4 border-t border-border shrink-0">
                <div className="flex items-center gap-3 bg-muted rounded-full px-5 py-3">
                    <Input
                        placeholder="Write a message..."
                        className="flex-1 bg-transparent border-0 focus-visible:ring-0 focus-visible:ring-offset-0 text-foreground placeholder:text-muted-foreground"
                        value={messageText}
                        onChange={(e) => setMessageText(e.target.value)}
                        onKeyDown={(e) => {
                            if (e.key === 'Enter' && !e.shiftKey) {
                                e.preventDefault()
                                handleSend()
                            }
                        }}
                    />
                    {/* Hidden file input */}
                    <input
                        ref={fileInputRef}
                        type="file"
                        className="hidden"
                        onChange={(e) => {
                            const file = e.target.files?.[0] || null
                            setSelectedFile(file)
                        }}
                    />
                    <Button
                        size="icon"
                        variant="ghost"
                        className="h-8 w-8 text-muted-foreground hover:text-foreground shrink-0"
                        onClick={() => fileInputRef.current?.click()}
                        title={selectedFile ? selectedFile.name : "Attach a file"}
                    >
                        <ImageIcon className="h-5 w-5" />
                    </Button>
                    <Button
                        size="icon"
                        variant="ghost"
                        className="h-8 w-8 text-muted-foreground hover:text-foreground shrink-0"
                        onClick={handleSend}
                        disabled={!messageText.trim() && !selectedFile}
                        title="Send"
                    >
                        <Send className="h-5 w-5" />
                    </Button>
                </div>
                {selectedFile && (
                    <div className="px-2 pt-2 text-xs text-muted-foreground truncate" title={selectedFile.name}>
                        Attached: {selectedFile.name}
                    </div>
                )}
            </div>
        </div>
    )
}
