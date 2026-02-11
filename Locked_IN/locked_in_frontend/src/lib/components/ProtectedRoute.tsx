"use client"

import { Navigate } from "react-router-dom"
import { useAuthStore } from "@/stores/authStore"

type ProtectedRouteProps = {
    children: React.ReactNode
}

export default function ProtectedRoute({ children }: ProtectedRouteProps) {
    const isLoggedIn = useAuthStore((state) => state.isLoggedIn)
    const isInitialized = useAuthStore((state) => state.isInitialized)

    if (!isInitialized) {
        return (
            <div className="relative w-full min-h-full overflow-y-auto bg-background">
                <div className="relative z-10 min-h-full flex items-center justify-center px-6 py-12">
                    <div className="text-center">
                        <p className="text-muted-foreground">Loading...</p>
                    </div>
                </div>
            </div>
        )
    }

    if (!isLoggedIn) {
        return <Navigate to="/login" replace />
    }

    return <>{children}</>
}
