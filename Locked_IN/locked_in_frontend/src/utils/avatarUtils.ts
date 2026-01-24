export interface AvatarData {
    fileName?: string
    contentType?: string
    length?: number
    data?: string
    [key: string]: any
}

export const convertAvatarToUrl = async (
    avatar: File | string | AvatarData | null | undefined
): Promise<string | undefined> => {
    if (!avatar) return undefined

    if (typeof avatar === 'string') {
        if (avatar.startsWith('data:')) {
            return avatar
        }
        if (avatar.startsWith('http://') || avatar.startsWith('https://')) {
            return avatar
        }
        const contentType = 'image/jpeg'
        return `data:${contentType};base64,${avatar}`
    }

    if (avatar instanceof File) {
        return URL.createObjectURL(avatar)
    }

    if (typeof avatar === 'object' && avatar !== null) {
        const avatarObj = avatar as AvatarData

        if (avatarObj.data && typeof avatarObj.data === 'string') {
            const contentType = avatarObj.contentType || 'image/jpeg'
            if (avatarObj.data.startsWith('data:')) {
                return avatarObj.data
            }
            return `data:${contentType};base64,${avatarObj.data}`
        }

        if (avatarObj.fileName && !avatarObj.data) {
            if (avatarObj.fileName.startsWith('http://') || avatarObj.fileName.startsWith('https://')) {
                return avatarObj.fileName
            }
            return undefined
        }

        if (avatarObj.content && typeof avatarObj.content === 'string') {
            const contentType = avatarObj.contentType || 'image/jpeg'
            return `data:${contentType};base64,${avatarObj.content}`
        }
    }

    return undefined
}

export const extractAvatarFromResponse = async (
    response: any,
    apiBaseUrl: string = 'http://localhost:5122/api'
): Promise<string | undefined> => {
    let avatarUrl: string | undefined

    if (response?.AvatarURL && typeof response.AvatarURL === 'string' && response.AvatarURL.trim()) {
        avatarUrl = response.AvatarURL.trim()
    } else if (response?.avatarURL && typeof response.avatarURL === 'string' && response.avatarURL.trim()) {
        avatarUrl = response.avatarURL.trim()
    } else if (response?.data?.AvatarURL && typeof response.data.AvatarURL === 'string' && response.data.AvatarURL.trim()) {
        avatarUrl = response.data.AvatarURL.trim()
    } else if (response?.data?.avatarURL && typeof response.data.avatarURL === 'string' && response.data.avatarURL.trim()) {
        avatarUrl = response.data.avatarURL.trim()
    } else if (response?.avatar) {
        return await convertAvatarToUrl(response.avatar)
    } else if (response?.data?.avatar) {
        return await convertAvatarToUrl(response.data.avatar)
    }

    if (avatarUrl && avatarUrl.trim()) {
        return await fetchAvatarFile(avatarUrl, apiBaseUrl)
    }

    return undefined
}

export const fetchAvatarFile = async (
    avatarUrl: string,
    apiBaseUrl: string = 'http://localhost:5122/api'
): Promise<string | undefined> => {
    if (avatarUrl.startsWith('http://') || avatarUrl.startsWith('https://')) {
        return avatarUrl
    }

    if (avatarUrl.startsWith('data:')) {
        return avatarUrl
    }

    const encodedFilename = encodeURIComponent(avatarUrl)
    const response = await fetch(`${apiBaseUrl}/file/avatar/${encodedFilename}`, {
        credentials: 'include',
    })

    if (response.ok) {
        const blob = await response.blob()
        return URL.createObjectURL(blob)
    }

    return undefined
}
