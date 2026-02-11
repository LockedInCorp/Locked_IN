"use client"

import { useState } from "react"
import { Link } from "react-router-dom"
import { Button } from "@/lib/components/ui/button"
import { Input } from "@/lib/components/ui/input"
import { Label } from "@/lib/components/ui/label"
import { useAuthStore } from "@/stores/authStore"
import { useLogin } from "@/hooks/auth/useLogin"

export default function Login() {
    const [errorMessage, setErrorMessage] = useState<string | null>(null)
    const { loginEmail, loginPassword, setLoginEmail, setLoginPassword } = useAuthStore()
    const loginMutation = useLogin()

    const handleLogin = async () => {
        setErrorMessage(null)
        
        if (!loginEmail.trim() || !loginPassword.trim()) {
            setErrorMessage("Please enter both email and password")
            return
        }

        loginMutation.mutate(
            {
                username: loginEmail.trim(),
                password: loginPassword,
            },
            {
                onError: (error) => {
                    setErrorMessage(error.message || "Login failed. Please check your credentials.")
                },
            }
        )
    }

    const canLogin = loginEmail.trim() && loginPassword.trim() && !loginMutation.isPending

    return (
        <div className="relative w-full min-h-full overflow-y-auto bg-background">
            {/* Main content */}
            <div className="relative z-10 min-h-full flex items-center justify-center px-6 py-12">
                <div className="w-full max-w-md">
                    {/* Login Card */}
                    <div className="rounded-lg border border-border bg-card p-8 shadow-lg">
                        {/* Title */}
                        <h1 className="text-3xl font-bold text-primary mb-6">Log In</h1>
                        
                        {/* Error message */}
                        {errorMessage && (
                            <div className="mb-4 p-3 rounded-md bg-destructive/10 border border-destructive text-destructive text-sm">
                                {errorMessage}
                            </div>
                        )}
                        
                        <div className="space-y-6">
                            {/* Email */}
                            <div className="space-y-2">
                                <Label htmlFor="email" className="text-sm text-muted-foreground">Email</Label>
                                <Input
                                    id="email"
                                    type="email"
                                    value={loginEmail}
                                    onChange={(e) => setLoginEmail(e.target.value)}
                                    placeholder="example@abc.com"
                                    className="border-border bg-card text-foreground placeholder:text-muted-foreground"
                                />
                            </div>

                            {/* Password */}
                            <div className="space-y-2">
                                <Label htmlFor="password" className="text-sm text-muted-foreground">Password</Label>
                                <Input
                                    id="password"
                                    type="password"
                                    value={loginPassword}
                                    onChange={(e) => setLoginPassword(e.target.value)}
                                    placeholder="Enter your password"
                                    className="border-border bg-card text-foreground placeholder:text-muted-foreground"
                                />
                            </div>

                            {/* Login Button */}
                            <div className="flex justify-center pt-4">
                                <Button
                                    type="button"
                                    onClick={handleLogin}
                                    disabled={!canLogin}
                                    className="bg-primary px-8 py-2 text-base font-semibold text-primary-foreground hover:bg-primary/90 cursor-pointer disabled:opacity-50 disabled:cursor-not-allowed"
                                >
                                    {loginMutation.isPending ? "Logging in..." : "Log In"}
                                </Button>
                            </div>

                            {/* Register Link */}
                            <div className="text-center pt-2">
                                <span className="text-sm text-muted-foreground">
                                    Don't have an account?{" "}
                                    <Link to="/register" className="text-primary underline hover:text-primary/80">
                                        Sign Up
                                    </Link>
                                </span>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    )
}
