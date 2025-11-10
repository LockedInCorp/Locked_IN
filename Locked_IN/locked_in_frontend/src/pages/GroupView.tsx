"use client"
import { GroupsList } from "../custom_components/groupView/GroupsList.tsx"
import { ChatArea } from "../custom_components/groupView/ChatArea.tsx"
import { GroupInfo } from "../custom_components/groupView/GroupInfo.tsx"

export function GroupView() {
    return (
        <>
            {/* Left Panel - Groups List */}
            <div className="w-[440px] border-r border-border flex-shrink-0">
                <GroupsList/>
            </div>

            {/* Center Panel - Chat Area */}
            <div className="flex-1 flex flex-col min-w-0">
                <ChatArea/>
            </div>

            {/* Right Panel - Group Info */}
            <div className="w-[320px] border-l border-border flex-shrink-0">
                <GroupInfo/>
            </div>
        </>
    )
}
