"use client"

import { useState } from "react"
import { useNavigate } from "react-router-dom"
import RegisterPart1 from "@/custom_components/register/RegisterPart1"
import RegisterPart2 from "@/custom_components/register/RegisterPart2"
import type { GameProfile } from "@/custom_components/profile/ProfileFields"

export default function Register() {
    const navigate = useNavigate()
    const [step, setStep] = useState<1 | 2>(1)
    
    // Part 1 data
    const [email, setEmail] = useState("")
    const [nickname, setNickname] = useState("")
    const [password, setPassword] = useState("")
    const [repeatPassword, setRepeatPassword] = useState("")
    const [avatarFile, setAvatarFile] = useState<File | null>(null)
    const [avatarPreview, setAvatarPreview] = useState<string | null>(null)

    // Part 2 data
    const [gameProfiles, setGameProfiles] = useState<GameProfile[]>([])

    const handleAvatarChange = (file: File | null) => {
        setAvatarFile(file)
        if (file) {
            const reader = new FileReader()
            reader.onloadend = () => {
                setAvatarPreview(reader.result as string)
            }
            reader.readAsDataURL(file)
        } else {
            setAvatarPreview(null)
        }
    }

    const handleNextPart1 = () => {
        if (email.trim() && nickname.trim() && password.trim() && repeatPassword.trim() && password === repeatPassword) {
            setStep(2)
        }
    }

    const handleNextPart2 = async () => {
        // TODO: Implement registration API call
        // Example:
        // try {
        //     const response = await fetch('/api/register', {
        //         method: 'POST',
        //         headers: { 'Content-Type': 'application/json' },
        //         body: JSON.stringify({
        //             email,
        //             nickname,
        //             password,
        //             avatarFile,
        //             gameProfiles
        //         })
        //     })
        //     if (response.ok) {
        //         // Redirect after successful registration
        //         navigate("/groups")
        //     } else {
        //         // Handle registration error
        //         console.error("Registration failed")
        //     }
        // } catch (error) {
        //     console.error("Registration error:", error)
        // }
        
        // For now, just redirect to groups/discover page
        console.log("Registering user:", {
            email,
            nickname,
            password,
            avatarFile,
            gameProfiles
        })
        
        // Redirect to groups/discover page after registration
        navigate("/groups")
    }

    return (
        <div className="relative w-full min-h-full overflow-y-auto bg-background">
            {/* Main content */}
            <div className="relative z-10 min-h-full flex items-center justify-center px-6 py-12">
                <div className="w-full max-w-3xl">
                    {/* Register Card */}
                    <div className="rounded-lg border border-border bg-card p-8 shadow-lg">
                        {/* Title */}
                        <h1 className="text-3xl font-bold text-primary mb-2">Register</h1>
                        
                        {step === 1 ? (
                            <RegisterPart1
                                email={email}
                                nickname={nickname}
                                password={password}
                                repeatPassword={repeatPassword}
                                avatarUrl={avatarPreview || undefined}
                                avatarFallback={nickname.charAt(0).toUpperCase() || "U"}
                                onEmailChange={setEmail}
                                onNicknameChange={setNickname}
                                onPasswordChange={setPassword}
                                onRepeatPasswordChange={setRepeatPassword}
                                onAvatarChange={handleAvatarChange}
                                onNext={handleNextPart1}
                            />
                        ) : (
                            <RegisterPart2
                                gameProfiles={gameProfiles}
                                onGameProfilesChange={setGameProfiles}
                                onNext={handleNextPart2}
                            />
                        )}
                    </div>
                </div>
            </div>
        </div>
    )
}
