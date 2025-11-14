import { Plus } from "lucide-react"
import { Input } from "@/components/ui/input"
import { Button } from "@/components/ui/button"
import { Checkbox } from "@/components/ui/checkbox"
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select"
import { useNavigate } from "react-router-dom"
import type { DiscoverFiltersProps } from "./types"

export function DiscoverFilters({
    groupSearch,
    onGroupSearchChange,
    showPending,
    onShowPendingChange,
}: DiscoverFiltersProps) {
    const navigate = useNavigate()

    return (
        <div className="shrink-0 mb-6 space-y-4">
            {/* Search and Filters */}
            <Input
                placeholder="Search groups..."
                value={groupSearch}
                onChange={(e) => onGroupSearchChange(e.target.value)}
                className="border-border text-foreground placeholder:text-muted-foreground h-12"
            />

            <div className="flex items-center justify-between gap-4">
                <div className="flex items-center gap-4">
                    <Select defaultValue="20">
                        <SelectTrigger className="w-[140px] bg-card border-border text-foreground">
                            <SelectValue placeholder="View"/>
                        </SelectTrigger>
                        <SelectContent className="bg-card border-border text-foreground">
                            <SelectItem value="10">View: 10</SelectItem>
                            <SelectItem value="20">View: 20</SelectItem>
                            <SelectItem value="50">View: 50</SelectItem>
                        </SelectContent>
                    </Select>

                    <Select defaultValue="relevance">
                        <SelectTrigger className="w-[200px] bg-card border-border text-foreground">
                            <SelectValue placeholder="Sort by"/>
                        </SelectTrigger>
                        <SelectContent className="bg-card border-border text-foreground">
                            <SelectItem value="relevance">Sort by: Relevance</SelectItem>
                            <SelectItem value="newest">Sort by: Newest</SelectItem>
                            <SelectItem value="popular">Sort by: Popular</SelectItem>
                        </SelectContent>
                    </Select>

                    <label className="flex items-center gap-2 text-foreground cursor-pointer">
                        <Checkbox
                            checked={showPending}
                            onCheckedChange={(checked) => onShowPendingChange(checked as boolean)}
                            className="data-[state=checked]:bg-primary data-[state=checked]:border-primary"
                        />
                        <span>Only Show Pending</span>
                    </label>
                </div>

                <Button
                    className="bg-primary hover:bg-primary/90 text-primary-foreground h-10 px-6 font-medium"
                    onClick={() => navigate("/groups/new")}
                >
                    <Plus className="h-5 w-5 mr-2"/>
                    Create group
                </Button>
            </div>
        </div>
    )
}

