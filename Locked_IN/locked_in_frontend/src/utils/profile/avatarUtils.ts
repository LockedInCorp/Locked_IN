import { apiClient } from '@/api/apiClient.ts';

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
        const parts = trimmedUrl.split('/');
        let apiEndpoint = '';
        
        if (parts.length >= 2) {
            const prefix = parts[0];
            const filename = parts.slice(1).join('/'); // Join back in case filename contains slashes
            apiEndpoint = `/File/${prefix}/${filename}`;
        }

        const response = await apiClient.get(apiEndpoint, {
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
