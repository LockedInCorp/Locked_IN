"use client"

import { useNavigate } from "react-router-dom"
import RegisterPart1 from "@/custom_components/register/RegisterPart1"
import RegisterPart2 from "@/custom_components/register/RegisterPart2"
import { useAuthStore } from "@/stores/authStore"

export default function Register() {
    const navigate = useNavigate()
    const {
        registerStep,
        registerEmail,
        registerNickname,
        registerPassword,
        registerRepeatPassword,
        registerAvatarFile,
        registerAvatarPreview,
        registerGameProfiles,
        setRegisterStep,
        setRegisterEmail,
        setRegisterNickname,
        setRegisterPassword,
        setRegisterRepeatPassword,
        setRegisterAvatarFile,
        setRegisterAvatarPreview,
        setRegisterGameProfiles
    } = useAuthStore()

    const handleAvatarChange = (file: File | null) => {
        setRegisterAvatarFile(file)
        if (file) {
            const reader = new FileReader()
            reader.onloadend = () => {
                setRegisterAvatarPreview(reader.result as string)
            }
            reader.readAsDataURL(file)
        } else {
            setRegisterAvatarPreview(null)
        }
    }

    const handleNextPart1 = () => {
        if (registerEmail.trim() && registerNickname.trim() && registerPassword.trim() && registerRepeatPassword.trim() && registerPassword === registerRepeatPassword) {
            setRegisterStep(2)
        }
    }

    const handleNextPart2 = async () => {
        // TODO: Implement registration API call
        
        console.log("Registering user:", {
            email: registerEmail,
            nickname: registerNickname,
            password: registerPassword,
            avatarFile: registerAvatarFile,
            gameProfiles: registerGameProfiles
        })
        
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
                        
                        {registerStep === 1 ? (
                            <RegisterPart1
                                email={registerEmail}
                                nickname={registerNickname}
                                password={registerPassword}
                                repeatPassword={registerRepeatPassword}
                                avatarUrl={registerAvatarPreview || undefined}
                                avatarFallback={registerNickname.charAt(0).toUpperCase() || "U"}
                                onEmailChange={setRegisterEmail}
                                onNicknameChange={setRegisterNickname}
                                onPasswordChange={setRegisterPassword}
                                onRepeatPasswordChange={setRegisterRepeatPassword}
                                onAvatarChange={handleAvatarChange}
                                onNext={handleNextPart1}
                            />
                        ) : (
                            <RegisterPart2
                                gameProfiles={registerGameProfiles}
                                onGameProfilesChange={setRegisterGameProfiles}
                                onNext={handleNextPart2}
                            />
                        )}
                    </div>
                </div>
            </div>
        </div>
    )
}
