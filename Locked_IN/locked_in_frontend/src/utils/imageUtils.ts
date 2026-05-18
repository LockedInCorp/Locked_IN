const DEV_MINIO_URL = 'http://localhost:9000';

const R2_BUCKET_URLS: Record<string, string> = {
  useravatars: 'https://pub-66fe682b4c0144c7aed613a24c0c44a8.r2.dev',
  teamicons: 'https://pub-776ececced984915b6600d999b0062ea.r2.dev',
  attachments: 'https://pub-ae0d5cdffc89413e9d4b688c7ac98fdf.r2.dev',
};

export const getImageUrl = (path: string | undefined): string | undefined => {
  if (!path) return undefined;

  if (path.startsWith('http') || path.startsWith('https') || path.startsWith('data:') || path.startsWith('blob:')) {
    return path;
  }

  const cleanPath = path.startsWith('/') ? path.slice(1) : path;

  if (import.meta.env.PROD) {
    const [bucket, ...rest] = cleanPath.split('/');
    const objectKey = rest.join('/');
    const baseUrl = R2_BUCKET_URLS[bucket];
    return baseUrl ? `${baseUrl}/${objectKey}` : undefined;
  }

  return `${DEV_MINIO_URL}/${cleanPath}`;
};

export const extractAvatarPath = (response: any): string | undefined => {
  return response?.AvatarURL ||
      response?.avatarURL ||
      response?.data?.AvatarURL ||
      response?.data?.avatarURL;
};