"use client"
import { useParams } from "react-router-dom"
import { useEffect } from "react"
import { useGroupViewStore } from "@/stores/groupViewStore.ts"
import { GroupsList } from "../custom_components/groupView/GroupsList.tsx"
import { ChatArea } from "../custom_components/groupView/ChatArea.tsx"
import { GroupInfo } from "../custom_components/groupView/GroupInfo.tsx"

export function GroupView() {
    const { chatId } = useParams<{ chatId: string }>()
    const { setSelectedGroupId } = useGroupViewStore()

    useEffect(() => {
        setSelectedGroupId(chatId || null)
    }, [chatId, setSelectedGroupId])

    return (
        <>
            {/* Left Panel - Groups List */}
            <div className="w-[440px] border-r border-border flex-shrink-0">
                <GroupsList/>
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
                {chatId ? <GroupInfo/> : (
                    <div className="flex-1 flex items-center justify-center text-muted-foreground">
                        Select a chat to see group info
                    </div>
                )}
            </div>
        </>
    )
}
