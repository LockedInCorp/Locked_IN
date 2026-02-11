import { useNavigate } from "react-router-dom"
import { Button } from "@/lib/components/ui/button"
import { Users, Tags, Filter, Gamepad2 } from "lucide-react"

export default function Home() {
    const navigate = useNavigate()

    const features = [
        {
            icon: <Users className="size-7" />,
            title: "Find Groups for Specific Games",
            description: "Discover gaming groups tailored to your favorite games. Connect with players who share your passion and skill level."
        },
        {
            icon: <Tags className="size-7" />,
            title: "Tag System",
            description: "Use tags to find groups that match your playstyle - whether you're looking for competitive play, casual fun, or new players."
        },
        {
            icon: <Filter className="size-7" />,
            title: "Advanced Filters",
            description: "Filter groups by game, skill level, region, and more. Find exactly what you're looking for with powerful search tools."
        },
        {
            icon: <Gamepad2 className="size-7" />,
            title: "Multiple Games Supported",
            description: "Whether you're into FPS, MOBA, RPG, or any other genre, find and create groups for all your favorite games."
        }
    ]

    return (
        <div className="relative w-full min-h-full overflow-y-auto bg-background">
            {/* Background texture effect */}
            <div className="absolute inset-0 opacity-[0.03] min-h-full">
                <div 
                    className="absolute inset-0 min-h-full"
                    style={{
                        backgroundImage: `url("data:image/svg+xml,%3Csvg width='60' height='60' viewBox='0 0 60 60' xmlns='http://www.w3.org/2000/svg'%3E%3Cg fill='none' fill-rule='evenodd'%3E%3Cg fill='%23ffffff' fill-opacity='0.05'%3E%3Cpath d='M36 34v-4h-2v4h-4v2h4v4h2v-4h4v-2h-4zm0-30V0h-2v4h-4v2h4v4h2V6h4V4h-4zM6 34v-4H4v4H0v2h4v4h2v-4h4v-2H6zM6 4V0H4v4H0v2h4v4h2V6h4V4H6z'/%3E%3C/g%3E%3C/g%3E%3C/svg%3E")`,
                    }}
                />
            </div>

            <div className="relative min-h-full">
                {/* Light source effect from right */}
                <div 
                    className="absolute top-0 right-0 bottom-0 w-[60%] pointer-events-none opacity-10"
                    style={{
                        background: 'radial-gradient(ellipse 100% 200% at top right, var(--primary), transparent 85%)'
                    }}
                />

                {/* Main content */}
                <div className="relative z-10 min-h-full flex items-center justify-between px-12 py-16">
                {/* Left content box */}
                <div className="max-w-2xl space-y-8 w-full">
                    <div className="bg-card rounded-2xl p-10 border border-border shadow-lg">
                        <h1 className="text-6xl font-bold mb-6 leading-tight">
                            <span className="text-foreground">Find </span>
                            <span className="text-primary">friends</span>
                            <br />
                            <span className="text-foreground">and more!</span>
                        </h1>
                        
                        <p className="text-muted-foreground text-lg mb-8 leading-relaxed">
                            Connect with gamers who share your passion. Discover groups, join communities, 
                            and find your perfect gaming squad. Whether you're looking for competitive teammates 
                            or casual players, "Locked IN!" helps you find the right people for every game.
                        </p>

                        <div className="flex gap-4">
                            <Button 
                                size="lg"
                                onClick={() => navigate("/groups")}
                                className="px-8 py-6 text-lg"
                            >
                                Discover Groups
                            </Button>
                            <Button 
                                size="lg"
                                variant="outline"
                                onClick={() => navigate("/groups/new")}
                                className="px-8 py-6 text-lg"
                            >
                                Create Group
                            </Button>
                        </div>
                    </div>

                    {/* Features grid */}
                    <div className="grid grid-cols-2 gap-4 mt-8">
                        {features.map((feature, index) => (
                            <div 
                                key={index}
                                className="bg-card rounded-xl p-5 border border-border hover:border-primary/50 transition-colors"
                            >
                                <div className="text-primary mb-3">
                                    {feature.icon}
                                </div>
                                <h3 className="text-foreground font-semibold mb-2 text-base">
                                    {feature.title}
                                </h3>
                                <p className="text-muted-foreground text-sm leading-relaxed">
                                    {feature.description}
                                </p>
                            </div>
                        ))}
                    </div>
                </div>

                <div className="flex-1 flex items-center justify-center max-w-2xl">
                    <div className="relative">
                        <div className="w-96 h-96 rounded-full bg-muted border-4 border-border flex items-center justify-center">
                            <Gamepad2 className="size-48 text-muted-foreground opacity-20" />
                        </div>
                        <div className="absolute -left-20 top-1/2 -translate-y-1/2 w-64 h-64 bg-foreground/5 blur-3xl rounded-full" />
                    </div>
                </div>
                </div>
            </div>
        </div>
    )
}

