"use client"

import { useEffect } from "react"
import { useSearchParams, useNavigate } from "react-router-dom"
import RegisterPart1 from "@/custom_components/register/RegisterPart1"
import RegisterPart2 from "@/custom_components/register/RegisterPart2"
import { useAuthStore } from "@/stores/authStore"
import { useRegistrationForm } from "@/hooks/useRegistrationForm"

export default function Register() {
    const [searchParams] = useSearchParams()
    const navigate = useNavigate()
    const stepFromUrl = searchParams.get("step") === "2" ? 2 : 1
    
    const { registerStep, setRegisterStep } = useAuthStore()
    
    const {
        errorMessage,
        part1Errors,
        part2Errors,
        isSubmittingPart2,
        registerEmail,
        registerNickname,
        registerPassword,
        registerRepeatPassword,
        registerAvatarPreview,
        registerGameProfiles,
        handleAvatarChange,
        handleEmailChange,
        handleNicknameChange,
        handlePasswordChange,
        handleRepeatPasswordChange,
        handleNextPart1,
        handleBack,
        handleGameProfilesChange,
        handleNextPart2,
    } = useRegistrationForm()

    useEffect(() => {
        const urlStep = searchParams.get("step")
        if (!urlStep) {
            navigate("/register?step=1", { replace: true })
        } else if (stepFromUrl !== registerStep) {
            setRegisterStep(stepFromUrl)
        }
    }, [searchParams, stepFromUrl, registerStep, setRegisterStep, navigate])

    return (
        <div className="relative w-full min-h-full overflow-y-auto bg-background">
            {/* Main content */}
            <div className="relative z-10 min-h-full flex items-center justify-center px-6 py-12">
                <div className="w-full max-w-3xl">
                    {/* Register Card */}
                    <div className="rounded-lg border border-border bg-card p-8 shadow-lg">
                    {/* Title */}
                    <h1 className="text-3xl font-bold text-primary mb-2">Register</h1>
                    
                    {/* Error message */}
                    {errorMessage && (
                        <div className="mb-4 p-3 rounded-md bg-destructive/10 border border-destructive text-destructive text-sm">
                            {errorMessage}
                        </div>
                    )}
                    
                    {registerStep === 1 ? (
                            <RegisterPart1
                                email={registerEmail}
                                nickname={registerNickname}
                                password={registerPassword}
                                repeatPassword={registerRepeatPassword}
                                avatarUrl={registerAvatarPreview || undefined}
                                avatarFallback={registerNickname.charAt(0).toUpperCase() || "U"}
                                onEmailChange={handleEmailChange}
                                onNicknameChange={handleNicknameChange}
                                onPasswordChange={handlePasswordChange}
                                onRepeatPasswordChange={handleRepeatPasswordChange}
                                onAvatarChange={handleAvatarChange}
                                onNext={handleNextPart1}
                                errors={part1Errors}
                                isLoading={false}
                            />
                        ) : (
                            <RegisterPart2
                                gameProfiles={registerGameProfiles}
                                onGameProfilesChange={handleGameProfilesChange}
                                onNext={handleNextPart2}
                                onBack={handleBack}
                                isLoading={isSubmittingPart2}
                                errors={part2Errors}
                            />
                        )}
                    </div>
                </div>
            </div>
        </div>
    )
}
