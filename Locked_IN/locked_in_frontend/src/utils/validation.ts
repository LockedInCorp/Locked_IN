export type Part1Errors = {
    email?: string
    nickname?: string
    password?: string
    repeatPassword?: string
}

export type Part2Errors = {
    [gameName: string]: {
        inGameNickname?: string
        experience?: string
    }
}

export const validateEmailFormat = (email: string): string | undefined => {
    if (!email.trim()) {
        return "Email is required"
    }
    if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email.trim())) {
        return "Please enter a valid email address"
    }
    return undefined
}

export const validateNicknameFormat = (nickname: string): string | undefined => {
    if (!nickname.trim()) {
        return "Nickname is required"
    }
    if (nickname.trim().length < 3) {
        return "Nickname must be at least 3 characters long"
    }
    return undefined
}

export const validatePasswordFormat = (password: string): string | undefined => {
    if (!password.trim()) {
        return "Password is required"
    }
    if (password.length < 6) {
        return "Password must be at least 6 characters long"
    }
    
    const errors: string[] = []
    
    if (!/[A-Z]/.test(password)) {
        errors.push("Passwords must have at least one uppercase ('A'-'Z')")
    }
    
    if (!/[0-9]/.test(password)) {
        errors.push("Passwords must have at least one digit ('0'-'9')")
    }
    
    if (!/[^a-zA-Z0-9]/.test(password)) {
        errors.push("Passwords must have at least one non alphanumeric character")
    }
    
    if (errors.length > 0) {
        return errors.join('. ')
    }
    
    return undefined
}

export const parseBackendError = (errorMessage: string): Part1Errors => {
    const errors: Part1Errors = {}
    const lowerMessage = errorMessage.toLowerCase()

    if (lowerMessage.includes("username") || lowerMessage.includes("user name")) {
        errors.nickname = errorMessage
    } else if (lowerMessage.includes("email") || lowerMessage.includes("e-mail")) {
        errors.email = errorMessage
    } else if (lowerMessage.includes("password")) {
        errors.password = errorMessage
    }

    return errors
}
