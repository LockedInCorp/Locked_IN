import type { GroupCard } from "./types"
import { GroupCard as GroupCardComponent } from "./GroupCard"
import { Button } from "@/components/ui/button"
import { ChevronLeft, ChevronRight } from "lucide-react"

interface GroupCardGridProps {
    groups: GroupCard[]
    currentPage: number
    totalPages: number
    onPageChange: (page: number) => void
    onRefresh?: () => void
}

export function GroupCardGrid({ groups, currentPage, totalPages, onPageChange, onRefresh }: GroupCardGridProps) {
    const handlePreviousPage = () => {
        if (currentPage > 1) {
            onPageChange(currentPage - 1)
        }
    }

    const handleNextPage = () => {
        if (currentPage < totalPages) {
            onPageChange(currentPage + 1)
        }
    }

    const getPageNumbers = () => {
        const pages: (number | string)[] = []
        
        if (totalPages <= 7) {
            for (let i = 1; i <= totalPages; i++) {
                pages.push(i)
            }
        } else {
            if (currentPage <= 3) {
                pages.push(1, 2, 3, "...", totalPages - 1, totalPages)
            } else if (currentPage >= totalPages - 2) {
                pages.push(1, 2, "...", totalPages - 2, totalPages - 1, totalPages)
            } else {
                pages.push(1, "...", currentPage - 1, currentPage, currentPage + 1, "...", totalPages)
            }
        }
        
        return pages
    }

    return (
        <div className="flex-1 flex flex-col overflow-hidden w-full min-w-0">
            <div className="flex-1 overflow-y-auto pr-2 w-full min-w-0">
                <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 2xl:grid-cols-5 gap-4 w-full">
                    {groups.map((group) => (
                        <GroupCardComponent key={group.id} group={group} onUpdate={onRefresh} />
                    ))}
                </div>
            </div>
            
            {/* Pagination */}
            <div className="flex justify-center items-center gap-2 mt-6 pb-4">
                <Button
                    variant="outline"
                    size="icon"
                    onClick={handlePreviousPage}
                    disabled={currentPage === 1}
                    className="h-9 w-9 border-border text-foreground hover:bg-muted"
                >
                    <ChevronLeft className="h-4 w-4" />
                </Button>

                {getPageNumbers().map((page, index) => (
                    page === "..." ? (
                        <span key={`ellipsis-${index}`} className="px-2 text-foreground">
                            ...
                        </span>
                    ) : (
                        <Button
                            key={page}
                            onClick={() => onPageChange(page as number)}
                            className={`h-9 w-9 px-0 rounded-md font-medium transition-all ${
                                currentPage === page
                                    ? "bg-primary hover:bg-primary/90 text-primary-foreground"
                                    : "bg-muted hover:bg-muted/80 text-muted-foreground border border-border"
                            }`}
                        >
                            {page}
                        </Button>
                    )
                ))}

                <Button
                    variant="outline"
                    size="icon"
                    onClick={handleNextPage}
                    disabled={currentPage === totalPages}
                    className="h-9 w-9 border-border text-foreground hover:bg-muted"
                >
                    <ChevronRight className="h-4 w-4" />
                </Button>
            </div>
        </div>
    )
}
