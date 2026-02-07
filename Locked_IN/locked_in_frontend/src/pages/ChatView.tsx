"use client"
import { useParams } from "react-router-dom"
import { useEffect } from "react"
import { useChatViewStore } from "@/stores/chatViewStore.ts"
import { ChatsList } from "../custom_components/chatView/ChatsList.tsx"
import { ChatArea } from "../custom_components/chatView/ChatArea.tsx"
import { ChatInfo } from "../custom_components/chatView/ChatInfo.tsx"

export function ChatView() {
    const { chatId } = useParams<{ chatId: string }>()
    const { setSelectedGroupId } = useChatViewStore()

    useEffect(() => {
        setSelectedGroupId(chatId || null)
    }, [chatId, setSelectedGroupId])

    return (
        <>
            {/* Left Panel - Groups List */}
            <div className="w-[440px] border-r border-border flex-shrink-0">
                <ChatsList/>
            </div> 

            {/* Center Panel - Chat Area */}
            <div className="flex-1 flex flex-col min-w-0">
                {chatId ? <ChatArea/> : (
                    <div className="flex-1 flex items-center justify-center text-muted-foreground">
                        Select a chat to start messaging
                    </div>
                )}
            </div>

            {/* Right Panel - Group Info */}
            <div className="w-[320px] border-l border-border flex-shrink-0 h-full flex flex-col">
                {chatId ? <ChatInfo/> : (
                    <div className="flex-1 flex items-center justify-center text-muted-foreground">
                        Select a chat to see group info
                    </div>
                )}
            </div>
        </>
    )
}
