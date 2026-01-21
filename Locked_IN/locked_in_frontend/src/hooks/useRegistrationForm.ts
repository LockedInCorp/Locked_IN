import { useState, useCallback } from "react"
import { useNavigate } from "react-router-dom"
import { useAuthStore } from "@/stores/authStore"
import { validateEmailFormat, validateNicknameFormat, validatePasswordFormat, parseBackendError, type Part1Errors, type Part2Errors } from "@/utils/validation"
import type { GameProfile } from "@/stores/authStore"

export function useRegistrationForm() {
    const navigate = useNavigate()
    const [errorMessage, setErrorMessage] = useState<string | null>(null)
    const [part1Errors, setPart1Errors] = useState<Part1Errors>({})
    const [part2Errors, setPart2Errors] = useState<Part2Errors>({})
    const [isSubmittingPart2, setIsSubmittingPart2] = useState(false)

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
        setRegisterGameProfiles,
        setUser,
        resetRegisterForm
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

    const handleEmailChange = useCallback((value: string) => {
        setRegisterEmail(value)
        
        const formatError = validateEmailFormat(value)
        if (formatError) {
            setPart1Errors(prev => ({ ...prev, email: formatError }))
        } else {
            setPart1Errors(prev => ({ ...prev, email: undefined }))
        }
    }, [setRegisterEmail])

    const handleNicknameChange = useCallback((value: string) => {
        setRegisterNickname(value)
        
        const formatError = validateNicknameFormat(value)
        if (formatError) {
            setPart1Errors(prev => ({ ...prev, nickname: formatError }))
        } else {
            setPart1Errors(prev => ({ ...prev, nickname: undefined }))
        }
    }, [setRegisterNickname])

    const handlePasswordChange = (value: string) => {
        setRegisterPassword(value)
        
        const formatError = validatePasswordFormat(value)
        if (formatError) {
            setPart1Errors(prev => ({ ...prev, password: formatError }))
        } else {
            setPart1Errors(prev => ({ ...prev, password: undefined }))
        }

        if (part1Errors.repeatPassword && value === registerRepeatPassword) {
            setPart1Errors(prev => ({ ...prev, repeatPassword: undefined }))
        } else if (registerRepeatPassword && value !== registerRepeatPassword) {
            setPart1Errors(prev => ({ ...prev, repeatPassword: "Passwords do not match" }))
        }
    }

    const handleRepeatPasswordChange = (value: string) => {
        setRegisterRepeatPassword(value)
        
        if (!value.trim()) {
            setPart1Errors(prev => ({ ...prev, repeatPassword: "Please repeat your password" }))
        } else if (value !== registerPassword) {
            setPart1Errors(prev => ({ ...prev, repeatPassword: "Passwords do not match" }))
        } else {
            setPart1Errors(prev => ({ ...prev, repeatPassword: undefined }))
        }
    }

    const validatePart1Format = (): boolean => {
        const errors: Part1Errors = {}

        const emailError = validateEmailFormat(registerEmail)
        if (emailError) {
            errors.email = emailError
        }

        const nicknameError = validateNicknameFormat(registerNickname)
        if (nicknameError) {
            errors.nickname = nicknameError
        }

        const passwordError = validatePasswordFormat(registerPassword)
        if (passwordError) {
            errors.password = passwordError
        }

        if (registerPassword !== registerRepeatPassword) {
            errors.repeatPassword = "Passwords do not match"
        } else if (!registerRepeatPassword.trim()) {
            errors.repeatPassword = "Please repeat your password"
        }

        if (Object.keys(errors).length > 0) {
            setPart1Errors(errors)
            return false
        }

        return true
    }

    const handleNextPart1 = () => {
        setErrorMessage(null)
        
        if (!validatePart1Format()) {
            return
        }

        if (part1Errors.email || part1Errors.nickname || part1Errors.password || part1Errors.repeatPassword) {
            return
        }

        if (!registerEmail.trim() || !registerNickname.trim() || !registerPassword.trim() || !registerRepeatPassword.trim()) {
            return
        }

        setRegisterStep(2)
        navigate("/register?step=2", { replace: true })
    }

    const handleBack = () => {
        setErrorMessage(null)
        setPart1Errors({})
        setRegisterStep(1)
        navigate("/register?step=1", { replace: true })
    }

    const validatePart2 = (): boolean => {
        const errors: Part2Errors = {}

        registerGameProfiles.forEach(profile => {
            const profileErrors: {
                inGameNickname?: string
                experience?: string
            } = {}

            if (!profile.inGameNickname?.trim()) {
                profileErrors.inGameNickname = "In-game nickname is required"
            }

            if (!profile.experience) {
                profileErrors.experience = "Please select your experience level"
            }

            if (Object.keys(profileErrors).length > 0) {
                errors[profile.gameName] = profileErrors
            }
        })

        setPart2Errors(errors)
        return Object.keys(errors).length === 0
    }

    const handleGameProfilesChange = (profiles: GameProfile[]) => {
        setRegisterGameProfiles(profiles)
        
        const newErrors: Part2Errors = {}
        Object.keys(part2Errors).forEach(gameName => {
            const profile = profiles.find(p => p.gameName === gameName)
            if (!profile) {
                return
            }
            
            const gameErrors: { inGameNickname?: string; experience?: string } = {}
            if (part2Errors[gameName]?.inGameNickname && !profile.inGameNickname?.trim()) {
                gameErrors.inGameNickname = part2Errors[gameName].inGameNickname
            }
            if (part2Errors[gameName]?.experience && !profile.experience) {
                gameErrors.experience = part2Errors[gameName].experience
            }
            
            if (Object.keys(gameErrors).length > 0) {
                newErrors[gameName] = gameErrors
            }
        })
        setPart2Errors(newErrors)
    }

    const handleNextPart2 = async () => {
        setErrorMessage(null)
        setPart1Errors({})

        if (registerPassword !== registerRepeatPassword) {
            setPart1Errors({ repeatPassword: "Passwords do not match" })
            setRegisterStep(1)
            navigate("/register?step=1", { replace: true })
            return
        }

        if (!validatePart2()) {
            return
        }

        setIsSubmittingPart2(true)
        try {
            const formData = new FormData()
            formData.append('username', registerNickname)
            formData.append('email', registerEmail)
            formData.append('password', registerPassword)
            
            if (registerAvatarFile) {
                formData.append('avatar', registerAvatarFile)
            }

            const response = await fetch('http://localhost:5122/api/user/register', {
                method: 'POST',
                body: formData,
                credentials: 'include',
            })

            if (!response.ok) {
                const errorData = await response.json().catch(() => ({ 
                    message: 'Registration failed' 
                }))
                const errorMessage = errorData.message || errorData.Message || 'Registration failed'
                
                const fieldErrors = parseBackendError(errorMessage)
                
                if (Object.keys(fieldErrors).length > 0) {
                    setPart1Errors(fieldErrors)
                    setRegisterStep(1)
                    navigate("/register?step=1", { replace: true })
                } else {
                    setErrorMessage(errorMessage)
                }
                return
            }

            const responseData = await response.json()
            if (responseData.success && responseData.data) {
                setUser({
                    id: responseData.data.id.toString(),
                    email: responseData.data.email,
                    nickname: responseData.data.username,
                    avatarUrl: typeof responseData.data.avatar === 'string' ? responseData.data.avatar : undefined,
                })
                resetRegisterForm()
                navigate('/groups')
            } else {
                setErrorMessage(responseData.message || 'Registration failed')
            }
        } catch (error) {
            const errorMessage = error instanceof Error ? error.message : "Registration failed. Please try again."
            setErrorMessage(errorMessage)
        } finally {
            setIsSubmittingPart2(false)
        }
    }

    return {
        errorMessage,
        part1Errors,
        part2Errors,
        isSubmittingPart2,
        registerStep,
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
    }
}
