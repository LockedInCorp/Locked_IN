import { Routes, Route, useNavigate } from "react-router-dom"
import { GroupView } from "@/pages/GroupView"
import GroupCreation from "@/pages/GroupCreation"
import GroupDiscover from "@/pages/GroupDiscover"
import { User } from "lucide-react"
import { Avatar, AvatarFallback, AvatarImage } from "@/components/ui/avatar"
import { Button } from "@/components/ui/button"

function App() {
    const navigate = useNavigate();

    return (
        <div className="h-dvh bg-background flex flex-col overflow-hidden">
            <header className="flex items-center justify-between border-b border-border bg-card px-6 py-4 flex-shrink-0">
                <div className="flex items-center gap-6">
                    <h1
                        onClick={() => navigate("/")}
                        className="text-xl font-bold text-primary cursor-pointer hover:opacity-80 transition-opacity"
                    >
                        Locked IN!
                    </h1>

                    <nav className="flex items-center gap-1">
                        <Button
                            variant="ghost"
                            onClick={() => navigate("/groups")}
                            className="text-muted-foreground hover:text-foreground hover:bg-transparent dark:hover:bg-transparent focus-visible:ring-0 focus-visible:ring-offset-0"
                        >
                            Discover
                        </Button>

                        <Button
                            variant="ghost"
                            onClick={() => navigate("/")}
                            className="text-muted-foreground hover:text-foreground hover:bg-transparent dark:hover:bg-transparent focus-visible:ring-0 focus-visible:ring-offset-0"
                        >
                            MyGroups
                        </Button>
                    </nav>
                </div>

                <div className="flex items-center gap-3">
                    <User className="size-5 text-muted-foreground" />
                    <Avatar className="h-10 w-10 cursor-pointer hover:ring-2 hover:ring-ring transition-all">
                        <AvatarImage src="/diverse-user-avatars.png" />
                        <AvatarFallback>U</AvatarFallback>
                    </Avatar>
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
