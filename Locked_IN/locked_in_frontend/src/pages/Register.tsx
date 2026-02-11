"use client"

import RegisterPart1 from "@/components/register/RegisterPart1"
import { useRegister } from "@/hooks/auth/useRegister"

export default function Register() {
    const {
        errorMessage,
        part1Errors,
        isSubmittingPart1,
        registerEmail,
        registerNickname,
        registerPassword,
        registerRepeatPassword,
        registerAvatarPreview,
        handleAvatarChange,
        handleEmailChange,
        handleNicknameChange,
        handlePasswordChange,
        handleRepeatPasswordChange,
        handleNextPart1,
    } = useRegister()

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
                            isLoading={isSubmittingPart1}
                        />
                    </div>
                </div>
            </div>
        </div>
    )
}
