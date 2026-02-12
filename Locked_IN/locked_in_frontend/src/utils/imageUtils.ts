import { API_BASE_URL } from '@/api/apiClient';

export const getImageUrl = (path: string | undefined): string | undefined => {
  if (!path) return undefined;

  if (path.startsWith('http://') || path.startsWith('https://') || path.startsWith('data:') || path.startsWith('blob:')) {
    return path;
  }


  const normalizedPath = path.startsWith('/') ? path : `/${path}`;
  
  return `${API_BASE_URL}/File${normalizedPath}`;
};

export const extractAvatarPath = (response: any): string | undefined => {
  return response?.AvatarURL || 
         response?.avatarURL || 
         response?.data?.AvatarURL || 
         response?.data?.avatarURL;
};
