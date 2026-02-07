import { useState, useCallback } from "react"
import { useNavigate } from "react-router-dom"
import { useAuthStore } from "@/stores/authStore"
import { validateEmailFormat, validateNicknameFormat, validatePasswordFormat, parseBackendError, type Part1Errors, type Part2Errors } from "@/utils/validation"
import { registerUser, loginUser } from "@/api/api"
import { extractAvatarPath, getImageUrl } from "@/utils/imageUtils"
import { persist } from "@/utils/auth/persistance"

export function useRegister() {
    const navigate = useNavigate()
    const [errorMessage, setErrorMessage] = useState<string | null>(null)
    const [part1Errors, setPart1Errors] = useState<Part1Errors>({})
    const [part2Errors, setPart2Errors] = useState<Part2Errors>({})
    const [isSubmittingPart1, setIsSubmittingPart1] = useState(false)

    const {
        registerEmail,
        registerNickname,
        registerPassword,
        registerRepeatPassword,
        registerAvatarFile,
        registerAvatarPreview,
        setRegisterEmail,
        setRegisterNickname,
        setRegisterPassword,
        setRegisterRepeatPassword,
        setRegisterAvatarFile,
        setRegisterAvatarPreview,
        resetRegisterForm,
        setUser
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

    const handleNextPart1 = async () => {
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

        setIsSubmittingPart1(true)
        try {
            const responseData = await registerUser({
                username: registerNickname,
                email: registerEmail,
                password: registerPassword,
                avatar: registerAvatarFile || null
            })

            const userId = responseData.id || (responseData as any).userId
            if (userId !== undefined && userId !== null) {
                try {
                    const loginData = await loginUser({
                        username: registerEmail,
                        password: registerPassword
                    })

                    if (loginData) {
                        const avatarUrl = getImageUrl(extractAvatarPath(loginData as any))
                        
                        const userData = {
                            id: loginData.id.toString(),
                            email: loginData.email,
                            nickname: loginData.username,
                            avatarUrl: avatarUrl,
                        }
                        
                        persist.setUserData(userData)
                        setUser(userData)
                        resetRegisterForm()
                        
                        navigate("/profile/game-profiles")
                    }
                } catch (loginError: any) {
                    setErrorMessage('Registration succeeded but auto-login failed. Please log in manually.')
                }
            } else {
                setErrorMessage('Registration succeeded but user ID not found')
            }
        } catch (error: any) {
            const errorMessage = error instanceof Error ? error.message : 'Registration failed'
            
            const fieldErrors = parseBackendError(errorMessage)
            
            if (Object.keys(fieldErrors).length > 0) {
                setPart1Errors(fieldErrors)
            } else {
                setErrorMessage(errorMessage)
            }
        } finally {
            setIsSubmittingPart1(false)
        }
    }


    return {
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
    }
}
