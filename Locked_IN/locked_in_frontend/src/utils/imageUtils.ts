import { API_BASE_URL } from '@/api/apiClient';

export const getImageUrl = (path: string | undefined): string | undefined => {
  if (!path) return undefined;

  if (path.startsWith('http://') || path.startsWith('https://') || path.startsWith('data:') || path.startsWith('blob:')) {
    return path;
  }

  // Ensure path starts with a slash for the URL construction
  const normalizedPath = path.startsWith('/') ? path : `/${path}`;
  
  // Construct the URL: API_BASE_URL/File/path
  // API_BASE_URL is 'http://localhost:5122/api'
  // Resulting URL: 'http://localhost:5122/api/File/prefix/filename.ext'
  return `${API_BASE_URL}/File${normalizedPath}`;
};

/**
 * Extracts avatar URL/path from various possible response structures
 */
export const extractAvatarPath = (response: any): string | undefined => {
  return response?.AvatarURL || 
         response?.avatarURL || 
         response?.data?.AvatarURL || 
         response?.data?.avatarURL;
};
