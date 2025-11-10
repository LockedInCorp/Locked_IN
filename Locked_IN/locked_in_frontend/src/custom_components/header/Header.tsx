import { useNavigate } from "react-router-dom"
import { User } from "lucide-react"
import { Avatar, AvatarFallback, AvatarImage } from "@/components/ui/avatar"
import { Button } from "@/components/ui/button"

export function Header() {
    const navigate = useNavigate();

    return (
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
    )
}

