"use client"

import { MoreVertical, ImageIcon, Send } from "lucide-react"
import { Avatar, AvatarFallback, AvatarImage } from "@/components/ui/avatar"
import { Button } from "@/components/ui/button"
import { Input } from "@/components/ui/input"

const messages = [
    {
        id: 1,
        sender: "You",
        content: "Hey Jude!",
        isCurrentUser: true,
    },
    {
        id: 2,
        sender: "Aaron",
        content: "Don't make it bad!",
        isCurrentUser: false,
    },
    {
        id: 3,
        sender: "You",
        content: "Take a sad song!",
        isCurrentUser: true,
    },
    {
        id: 4,
        sender: "Aaron",
        content: "And make it better!",
        isCurrentUser: false,
    },
]

export function ChatArea() {
    return (
        <div className="flex flex-col h-full">
            {/* Chat Header */}
            <div className="flex items-center justify-between px-6 py-4 border-b border-border">
                <div className="flex items-center gap-3">
                    <Avatar className="h-10 w-10">
                        <AvatarImage src="/diverse-group-avatars.png" />
                        <AvatarFallback>G</AvatarFallback>
                    </Avatar>
                    <div>
                        <h2 className="font-semibold text-foreground">Group name</h2>
                        <p className="text-xs text-muted-foreground">icon</p>
                    </div>
                </div>
                <Button size="icon" variant="ghost" className="text-foreground">
                    <MoreVertical className="h-5 w-5" />
                </Button>
            </div>

            {/* Messages Area */}
            <div className="flex-1 overflow-y-auto px-6 py-6 space-y-4">
                {messages.map((message) => (
                    <div key={message.id} className={`flex gap-3 ${message.isCurrentUser ? "justify-end" : "justify-start"}`}>
                        {!message.isCurrentUser && (
                            <Avatar className="h-10 w-10 flex-shrink-0">
                                <AvatarFallback className="bg-muted text-foreground">A</AvatarFallback>
                            </Avatar>
                        )}
                        <div className={`flex flex-col ${message.isCurrentUser ? "items-end" : "items-start"}`}>
                            <span className="text-xs text-primary mb-1">{message.sender}</span>
                            <div
                                className={`px-4 py-2 rounded-2xl max-w-md ${
                                    message.isCurrentUser ? "bg-muted text-foreground" : "bg-muted text-foreground"
                                }`}
                            >
                                {message.content}
                            </div>
                        </div>
                    </div>
                ))}
            </div>

            {/* Message Input */}
            <div className="px-6 py-4 border-t border-border">
                <div className="flex items-center gap-3 bg-muted rounded-full px-5 py-3">
                    <Input
                        placeholder="Write a message..."
                        className="flex-1 bg-transparent border-0 focus-visible:ring-0 focus-visible:ring-offset-0 text-foreground placeholder:text-muted-foreground"
                    />
                    <Button
                        size="icon"
                        variant="ghost"
                        className="h-8 w-8 text-muted-foreground hover:text-foreground flex-shrink-0"
                    >
                        <ImageIcon className="h-5 w-5" />
                    </Button>
                    <Button
                        size="icon"
                        variant="ghost"
                        className="h-8 w-8 text-muted-foreground hover:text-foreground flex-shrink-0"
                    >
                        <Send className="h-5 w-5" />
                    </Button>
                </div>
            </div>
        </div>
    )
}
