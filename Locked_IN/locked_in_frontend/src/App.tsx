import { Routes, Route, useNavigate } from "react-router-dom"
import { GroupView } from "@/pages/GroupView"
import GroupCreation from "@/pages/GroupCreation"
import GroupDiscover from "@/pages/GroupDiscover"
import { User } from "lucide-react"
import { Avatar, AvatarFallback, AvatarImage } from "@/components/ui/avatar"

function App() {
    const navigate = useNavigate();

    return (
        <div className="h-dvh bg-background flex flex-col overflow-hidden">
            <header className="flex items-center justify-between border-b border-[#2b2d31] bg-[#1a1d21] px-6 py-4 flex-shrink-0">
                <div className="flex items-center gap-6">
                    <h1
                        onClick={() => navigate("/")}
                        className="text-xl font-bold text-primary cursor-pointer"
                    >
                        Locked IN!
                    </h1>

                    <nav className="flex items-center gap-6 text-sm">
                        <button
                            onClick={() => navigate("/groups")}
                            className="text-gray-300 transition-colors hover:text-white"
                        >
                            Discover
                        </button>

                        <button
                            onClick={() => navigate("/")}
                            className="text-gray-300 transition-colors hover:text-white"
                        >
                            MyGroups
                        </button>
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

            <main className="flex flex-1 min-h-0 overflow-hidden">
                <Routes>
                    <Route path="/" element={<GroupView />} />
                    <Route path="/groups" element={<GroupDiscover />} />
                    <Route path="/groups/new" element={<GroupCreation />} />
                </Routes>
            </main>
        </div>
    )
}

export default App
