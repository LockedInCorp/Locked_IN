import { useNavigate } from "react-router-dom"
import { Button } from "@/components/ui/button"
import { Avatar, AvatarFallback, AvatarImage } from "@/components/ui/avatar"
import { useAuthStore } from "@/stores/authStore"

export function Header() {
    const navigate = useNavigate();
    const isLoggedIn = useAuthStore((state) => state.isLoggedIn);

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
                        onClick={() => navigate("/my-groups")}
                        className="text-muted-foreground hover:text-foreground hover:bg-transparent dark:hover:bg-transparent focus-visible:ring-0 focus-visible:ring-offset-0"
                    >
                        MyGroups
                    </Button>
                </nav>
            </div>

            <div className="flex items-center gap-3">
                {isLoggedIn ? (
                    <>
                        <Avatar 
                            className="h-10 w-10 cursor-pointer hover:ring-2 hover:ring-ring transition-all"
                            onClick={() => navigate("/profile")}
                        >
                            <AvatarImage src="/assets/diverse-user-avatars.png" />
                            <AvatarFallback>U</AvatarFallback>
                        </Avatar>
                    </>
                   
                ) : (
                    <>
                        <Button
                            variant="outline"
                            onClick={() => navigate("/login")}
                            className="border-primary text-primary hover:bg-primary/10"
                        >
                            Lock In
                        </Button>
                        <Button
                            onClick={() => navigate("/register")}
                            className="bg-primary text-primary-foreground hover:bg-primary/90"
                        >
                            Sign Up
                        </Button>
                    </>
                )}
            </div>
        </header>
    )
}

