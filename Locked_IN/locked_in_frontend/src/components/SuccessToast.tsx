import { useEffect } from "react"
import { CheckCircle } from "lucide-react"
import { cn } from "@/lib/cn"

type SuccessToastProps = {
    open: boolean
    message: string
    onClose: () => void
    duration?: number
    className?: string
}

export function SuccessToast({
    open,
    message,
    onClose,
    duration = 3000,
    className
}: SuccessToastProps) {
    useEffect(() => {
        if (!open) return
        const timer = setTimeout(onClose, duration)
        return () => clearTimeout(timer)
    }, [open, duration, onClose])

    if (!open) return null

    return (
        <div
            role="alert"
            className={cn(
                "fixed bottom-6 left-1/2 -translate-x-1/2 z-50",
                "flex items-center gap-3 px-4 py-3 rounded-lg border shadow-lg",
                "bg-card text-card-foreground border-border",
                "animate-in slide-in-from-bottom-4 fade-in duration-300",
                className
            )}
        >
            <CheckCircle className="h-5 w-5 shrink-0 text-green-600 dark:text-green-500" />
            <span className="text-sm font-medium">{message}</span>
        </div>
    )
}
