import { apiClient } from '@/lib/apiClient';

export const extractAvatarFromResponse = async (
    response: any
): Promise<string | undefined> => {
    const avatarUrl = response?.AvatarURL || response?.avatarURL || response?.data?.AvatarURL || response?.data?.avatarURL;
    
    if (!avatarUrl || typeof avatarUrl !== 'string' || !avatarUrl.trim()) {
        return undefined;
    }

    const trimmedUrl = avatarUrl.trim();

    if (trimmedUrl.startsWith('http://') || trimmedUrl.startsWith('https://') || trimmedUrl.startsWith('data:')) {
        return trimmedUrl;
    }

    try {
        const encodedFilename = encodeURIComponent(trimmedUrl);
        const response = await apiClient.get(`/file/avatar/${encodedFilename}`, {
            responseType: 'blob',
        });

        if (response.data) {
            return URL.createObjectURL(response.data);
        }
    } catch (error) {
        if (import.meta.env.DEV) {
            console.warn('Failed to fetch avatar:', error);
        }
    }

    return undefined;
}
