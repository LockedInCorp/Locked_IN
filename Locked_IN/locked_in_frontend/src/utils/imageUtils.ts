const MINIO_URL = 'http://localhost:9000';

export const getImageUrl = (path: string | undefined): string | undefined => {
  if (!path) return undefined;

  if (path.startsWith('http') || path.startsWith('https') || path.startsWith('data:') || path.startsWith('blob:')) {
    return path;
  }

  const cleanPath = path.startsWith('/') ? path.slice(1) : path;

  return `${MINIO_URL}/${cleanPath}`;
};

export const extractAvatarPath = (response: any): string | undefined => {
  return response?.AvatarURL ||
      response?.avatarURL ||
      response?.data?.AvatarURL ||
      response?.data?.avatarURL;
};