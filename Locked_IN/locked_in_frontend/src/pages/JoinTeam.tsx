"use client"

import { useEffect, useState, useRef } from "react"
import { useSearchParams, useNavigate } from "react-router-dom"
import { joinWithToken } from "@/api/api"
import { useQueryClient } from "@tanstack/react-query"
import { Loader2, AlertCircle, CheckCircle2 } from "lucide-react"
import { Button } from "@/lib/components/ui/button"

export default function JoinTeam() {
    const [searchParams] = useSearchParams()
    const navigate = useNavigate()
    const queryClient = useQueryClient()
    const token = searchParams.get("token")

    const [status, setStatus] = useState<'loading' | 'success' | 'error'>('loading')
    const [error, setError] = useState<string | null>(null)
    const joinAttempted = useRef(false)

    useEffect(() => {
        const join = async () => {
            if (!token || joinAttempted.current) {
                if (!token) {
                    setStatus('error')
                    setError('No invitation token provided.')
                }
                return
            }

            joinAttempted.current = true
            try {
                const response = await joinWithToken(token)
                setStatus('success')

                await queryClient.invalidateQueries({ queryKey: ["userChats"] })

                const teamChat = response.chatId;
                
                setTimeout(() => {
                    if (teamChat) {
                        navigate(`/my-groups/${teamChat}`)
                    } else {
                        navigate("/my-groups")
                    }
                }, 1500)
            } catch (err: any) {
                setStatus('error')
                setError(err.message || 'Failed to join the team.')
            }
        }

        join()
    }, [token, navigate, queryClient])

    return (
        <div className="flex flex-col items-center justify-center min-h-[60vh] w-full p-4">
            <div className="w-full max-w-md p-8 bg-card rounded-xl border border-border shadow-sm flex flex-col items-center text-center">
                {status === 'loading' && (
                    <>
                        <Loader2 className="h-12 w-12 text-primary animate-spin mb-4" />
                        <h2 className="text-2xl font-bold mb-2">Joining Team...</h2>
                        <p className="text-muted-foreground">Please wait while we process your invitation.</p>
                    </>
                )}

                {status === 'success' && (
                    <>
                        <CheckCircle2 className="h-12 w-12 text-green-500 mb-4" />
                        <h2 className="text-2xl font-bold mb-2">Successfully Joined!</h2>
                        <p className="text-muted-foreground mb-6">You've been added to the team. Redirecting you to the chat...</p>
                        <Loader2 className="h-5 w-5 text-primary animate-spin" />
                    </>
                )}

                {status === 'error' && (
                    <>
                        <AlertCircle className="h-12 w-12 text-destructive mb-4" />
                        <h2 className="text-2xl font-bold mb-2">Joining Failed</h2>
                        <p className="text-destructive mb-6 font-medium">{error}</p>
                        <div className="flex gap-4">
                            <Button variant="outline" onClick={() => navigate("/")}>Go Home</Button>
                            <Button onClick={() => navigate("/groups")}>Browse Groups</Button>
                        </div>
                    </>
                )}
            </div>
        </div>
    )
}
