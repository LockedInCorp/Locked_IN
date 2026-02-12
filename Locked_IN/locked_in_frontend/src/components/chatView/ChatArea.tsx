"use client"

import { ImageIcon, MoreVertical, Pencil, Send, Trash, X } from "lucide-react"
import { useMemo, useRef, useState, useCallback, useEffect } from "react"
import { useParams } from "react-router-dom"
import { Avatar, AvatarFallback, AvatarImage } from "@/lib/components/ui/avatar"
import { Button } from "@/lib/components/ui/button"
import { Input } from "@/lib/components/ui/input"
import {
    DropdownMenu,
    DropdownMenuContent,
    DropdownMenuItem,
    DropdownMenuTrigger,
} from "@/lib/components/ui/dropdown-menu"
import { ConfirmDialog } from "@/lib/components/ui/confirm-dialog"
import { useChatDetails } from "@/hooks/chat/useChatDetails"
import { getImageUrl } from "@/utils/imageUtils.ts"
import { persist } from "@/utils/auth/persistance"
import { sendMessage, editMessage, deleteMessage } from "@/api/api.ts"

export function ChatArea() {
    const { chatId } = useParams<{ chatId: string }>()

    const numericChatId = chatId ? parseInt(chatId, 10) : null
    const {
        chatDetails,
        messages: allMessages,
        isFetchingMore,
        hasMore,
        fetchMore,
        page,
        refetch
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
    }, [allMessages, page])

    useEffect(() => {
        const handleVisibilityChange = () => {
            if (document.visibilityState === 'visible' && numericChatId) {
                refetch()
            }
        }
        document.addEventListener('visibilitychange', handleVisibilityChange)
        return () => document.removeEventListener('visibilitychange', handleVisibilityChange)
    }, [numericChatId, refetch])

    const [messageText, setMessageText] = useState("")
    const [selectedFile, setSelectedFile] = useState<File | null>(null)
    const [editingMessageId, setEditingMessageId] = useState<number | null>(null)
    const [deleteConfirmOpen, setDeleteConfirmOpen] = useState(false)
    const [messageToDelete, setMessageToDelete] = useState<{ id: number } | null>(null)
    const fileInputRef = useRef<HTMLInputElement | null>(null)

    const messages = useMemo(() => {
        const currentUser = persist.getUserData()

        return allMessages
            .filter(m => !m.isDeleted)
            .map(m => {
            return ({
                id: m.id,
                sender: m.senderUsername ?? '',
                content: m.content ?? '',
                attachmentUrl: m.attachmentUrl,
                isCurrentUser: currentUser ? m.senderId.toString() === currentUser.id : false,
                senderAvatarUrl: m.senderAvatarUrl
            });
        })
    }, [allMessages])

    const groups = messages.reduce<
        { sender: string; isCurrentUser: boolean; items: any[]; senderAvatarUrl?: string }[]
    >((acc, m) => {
        const last = acc[acc.length - 1]
        if (last && last.sender === m.sender && last.isCurrentUser === m.isCurrentUser) {
            last.items.push(m)
        } else {
            acc.push({ sender: m.sender, isCurrentUser: m.isCurrentUser, items: [m], senderAvatarUrl: m.senderAvatarUrl })
        }
        return acc
    }, [])
    const handleSend = useCallback(async () => {
        if (!numericChatId) return
        if (editingMessageId) {
            if (!messageText.trim()) return
            try {
                await editMessage({
                    messageId: editingMessageId,
                    content: messageText.trim()
                })
                setMessageText("")
                setEditingMessageId(null)
            } catch (e) {
                console.error(e)
            }
        } else {
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
            } catch (e) {
                console.error(e)
            }
        }
    }, [numericChatId, editingMessageId, messageText, selectedFile])

    const handleEditMessage = useCallback((content: string, messageId: number) => {
        setMessageText(content)
        setEditingMessageId(messageId)
    }, [])

    const handleCancelEdit = useCallback(() => {
        setMessageText("")
        setEditingMessageId(null)
    }, [])

    const handleDeleteMessageClick = useCallback((messageId: number) => {
        setMessageToDelete({ id: messageId })
        setDeleteConfirmOpen(true)
    }, [])

    const handleConfirmDelete = useCallback(async () => {
        if (!messageToDelete) return
        try {
            await deleteMessage(messageToDelete.id)
        } catch (e) {
            console.error(e)
        } finally {
            setMessageToDelete(null)
        }
    }, [messageToDelete])

    return (
        <div className="flex flex-col h-full overflow-hidden">
            <ConfirmDialog
                open={deleteConfirmOpen}
                onOpenChange={setDeleteConfirmOpen}
                title="Delete message"
                description="Are you sure you want to delete this message?"
                onConfirm={handleConfirmDelete}
                confirmLabel="Yes"
                cancelLabel="No"
                confirmVariant="destructive"
            />
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
                                <AvatarImage src={getImageUrl(g.senderAvatarUrl)} />
                                <AvatarFallback className="bg-muted text-foreground">
                                    {(g.sender || '?')[0]}
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
                                            className={`group relative ${g.isCurrentUser ? "self-end" : "self-start"}`}
                                        >
                                            <div
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
                                            {g.isCurrentUser && (
                                                <div className={`absolute top-0 h-7 ${g.isCurrentUser ? "left-0 -translate-x-full pr-1" : "right-0 translate-x-full pl-1"}`}>
                                                    <DropdownMenu>
                                                        <DropdownMenuTrigger asChild>
                                                            <Button
                                                                size="icon"
                                                                variant="ghost"
                                                                className="h-7 w-7 shrink-0 opacity-0 group-hover:opacity-100 transition-opacity text-muted-foreground hover:text-foreground"
                                                            >
                                                                <MoreVertical className="h-4 w-4" />
                                                            </Button>
                                                        </DropdownMenuTrigger>
                                                        <DropdownMenuContent align={g.isCurrentUser ? "end" : "start"}>
                                                            <DropdownMenuItem onClick={() => handleEditMessage(m.content, m.id)}>
                                                                <Pencil className="h-4 w-4 mr-2" /> Edit
                                                            </DropdownMenuItem>
                                                            <DropdownMenuItem
                                                                variant="destructive"
                                                                onClick={() => handleDeleteMessageClick(m.id)}
                                                            >
                                                                <Trash className="h-4 w-4 mr-2" /> Delete
                                                            </DropdownMenuItem>
                                                        </DropdownMenuContent>
                                                    </DropdownMenu>
                                                </div>
                                            )}
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
                {editingMessageId && (
                    <div className="mb-2 flex items-center gap-2 text-sm text-muted-foreground">
                        <Pencil className="h-4 w-4" />
                        <span>Editing message</span>
                        <Button
                            size="sm"
                            variant="outline"
                            className="h-6 px-2 text-muted-foreground hover:text-foreground"
                            onClick={handleCancelEdit}
                        >
                            <X className="h-4 w-4 mr-1" />
                            Cancel
                        </Button>
                    </div>
                )}
                <div className="flex items-center gap-3 bg-muted rounded-full px-5 py-3">
                    <Input
                        placeholder={editingMessageId ? "Edit message..." : "Write a message..."}
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
                    {!editingMessageId && (
                        <Button
                            size="icon"
                            variant="ghost"
                            className="h-8 w-8 text-muted-foreground hover:text-foreground shrink-0"
                            onClick={() => fileInputRef.current?.click()}
                            title={selectedFile ? selectedFile.name : "Attach a file"}
                        >
                            <ImageIcon className="h-5 w-5" />
                        </Button>
                    )}
                    {editingMessageId ? (
                        <Button
                            size="icon"
                            variant="ghost"
                            className="h-8 w-8 text-muted-foreground hover:text-foreground shrink-0"
                            onClick={handleSend}
                            disabled={!messageText.trim()}
                            title="Save edit"
                        >
                            <Pencil className="h-5 w-5" />
                        </Button>
                    ) : (
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
                    )}
                </div>
                {selectedFile && (
                    <div className="px-2 pt-2 flex items-center gap-2">
                        <span className="text-xs text-muted-foreground truncate flex-1 min-w-0" title={selectedFile.name}>
                            Attached: {selectedFile.name}
                        </span>
                        <Button
                            size="icon"
                            variant="ghost"
                            className="h-6 w-6 shrink-0 text-muted-foreground hover:text-foreground"
                            onClick={() => {
                                setSelectedFile(null)
                                if (fileInputRef.current) fileInputRef.current.value = ""
                            }}
                            title="Remove attachment"
                        >
                            <X className="h-4 w-4" />
                        </Button>
                    </div>
                )}
            </div>
        </div>
    )
}
