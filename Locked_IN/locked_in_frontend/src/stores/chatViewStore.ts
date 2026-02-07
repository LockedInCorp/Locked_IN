import { create } from "zustand"
import type { ChatType } from "@/api/types"

export type Message = {
    id: number
    sender: string
    content: string
    isCurrentUser: boolean
}

interface ChatViewState {
    selectedGroupId: string | null
    chatType: ChatType | null
    
    messages: Message[]
    
    membersExpanded: boolean
    requestsExpanded: boolean
    
    setSelectedGroupId: (id: string | null) => void
    setChatType: (type: ChatType | null) => void
    setMessages: (messages: Message[]) => void
    addMessage: (message: Message) => void
    setMembersExpanded: (expanded: boolean) => void
    setRequestsExpanded: (expanded: boolean) => void
    toggleMembersExpanded: () => void
    toggleRequestsExpanded: () => void
}

const initialMessages: Message[] = [
    { id: 1, sender: "You", content: "Hey Jude!", isCurrentUser: true },
    { id: 2, sender: "Aaron", content: "Don't make it bad!", isCurrentUser: false },
    { id: 3, sender: "You", content: "Take a sad song!", isCurrentUser: true },
    { id: 4, sender: "Aaron", content: "And make it better!", isCurrentUser: false },
]

export const useChatViewStore = create<ChatViewState>((set) => ({
    selectedGroupId: null,
    chatType: null,
    messages: initialMessages,
    membersExpanded: true,
    requestsExpanded: true,
    
    setSelectedGroupId: (id) => set({ selectedGroupId: id }),
    setChatType: (type) => set({ chatType: type }),
    setMessages: (messages) => set({ messages }),
    addMessage: (message) => set((state) => ({
        messages: [...state.messages, message]
    })),
    setMembersExpanded: (expanded) => set({ membersExpanded: expanded }),
    setRequestsExpanded: (expanded) => set({ requestsExpanded: expanded }),
    toggleMembersExpanded: () => set((state) => ({
        membersExpanded: !state.membersExpanded
    })),
    toggleRequestsExpanded: () => set((state) => ({
        requestsExpanded: !state.requestsExpanded
    }))
}))
