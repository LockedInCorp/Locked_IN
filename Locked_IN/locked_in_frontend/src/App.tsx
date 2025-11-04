import { Routes, Route } from 'react-router-dom'
import { GroupView } from './pages/GroupView'
import GroupCreation from "@/pages/GroupCreation.tsx";
import {User} from "lucide-react";
import {Avatar, AvatarFallback, AvatarImage} from "@/components/ui/avatar.tsx";

function App() {
    return (
        <div className="min-h-screen bg-background">
            {/* Header */}
            <header className="flex items-center justify-between border-b border-[#2b2d31] bg-[#1a1d21] px-6 py-4">
                <div className="flex items-center gap-6">
                    <h1 className="text-xl font-bold text-primary">Locked IN!</h1>
                    <nav className="flex items-center gap-6 text-sm">
                        <button className="text-gray-300 transition-colors hover:text-white">Discover</button>
                        <button className="text-gray-300 transition-colors hover:text-white">MyGroups</button>
                    </nav>
                </div>
                <div className="flex items-center gap-3">
                    <User className="size-5 text-gray-400" />
                    <div className="size-10 overflow-hidden rounded-full">
                        <Avatar className="h-11 w-10">
                            <AvatarImage src="/diverse-user-avatars.png" />
                            <AvatarFallback>U</AvatarFallback>
                        </Avatar>
                    </div>
                </div>
            </header>

            <Routes>
                {/*<Route path="/" element={<Navigate to="/groups" replace />} />*/}
                <Route path="/" element={<GroupView />} />
                <Route path="/creation" element={<GroupCreation />} />
            </Routes>
        </div>
    )
}

export default App