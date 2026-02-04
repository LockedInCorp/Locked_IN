const TOKEN_KEY = 'auth_token';
const USER_DATA_KEY = 'auth_user_data';

const getCookieOptions = () => ({
  maxAge: 7 * 24 * 60 * 60,
  path: '/',
  sameSite: 'Lax' as const,
  secure: typeof window !== 'undefined' && window.location.protocol === 'https:',
});

export interface StoredUserData {
  id: string;
  email: string;
  nickname: string;
  avatarUrl?: string;
}

const getCookie = (name: string): string | null => {
  if (typeof document === 'undefined') return null;
  const nameEQ = name + '=';
  const cookies = document.cookie.split(';');
  for (let i = 0; i < cookies.length; i++) {
    let cookie = cookies[i];
    while (cookie.charAt(0) === ' ') {
      cookie = cookie.substring(1, cookie.length);
    }
    if (cookie.indexOf(nameEQ) === 0) {
      return decodeURIComponent(cookie.substring(nameEQ.length, cookie.length));
    }
  }
  return null;
};

const setCookie = (name: string, value: string, options: ReturnType<typeof getCookieOptions>): void => {
  if (typeof document === 'undefined') return;
  let cookieString = `${name}=${encodeURIComponent(value)}`;
  cookieString += `; max-age=${options.maxAge}`;
  cookieString += `; path=${options.path}`;
  cookieString += `; SameSite=${options.sameSite}`;
  if (options.secure) {
    cookieString += '; Secure';
  }
  document.cookie = cookieString;
};

const removeCookie = (name: string, options: ReturnType<typeof getCookieOptions>): void => {
  if (typeof document === 'undefined') return;
  let cookieString = `${name}=; expires=Thu, 01 Jan 1970 00:00:00 UTC`;
  cookieString += `; path=${options.path}`;
  cookieString += `; SameSite=${options.sameSite}`;
  if (options.secure) {
    cookieString += '; Secure';
  }
  document.cookie = cookieString;
};

export const authStorage = {
  getToken: (): string | null => {
    if (typeof window === 'undefined') return null;
    return getCookie(TOKEN_KEY);
  },

  setToken: (token: string): void => {
    if (typeof window === 'undefined') return;
    setCookie(TOKEN_KEY, token, getCookieOptions());
  },

  removeToken: (): void => {
    if (typeof window === 'undefined') return;
    removeCookie(TOKEN_KEY, getCookieOptions());
  },

  hasToken: (): boolean => {
    return authStorage.getToken() !== null;
  },

  getUserData: (): StoredUserData | null => {
    if (typeof window === 'undefined') return null;
    const data = getCookie(USER_DATA_KEY);
    if (!data) return null;
    try {
      return JSON.parse(data) as StoredUserData;
    } catch {
      return null;
    }
  },

  setUserData: (userData: StoredUserData): void => {
    if (typeof window === 'undefined') return;
    setCookie(USER_DATA_KEY, JSON.stringify(userData), getCookieOptions());
  },

  removeUserData: (): void => {
    if (typeof window === 'undefined') return;
    removeCookie(USER_DATA_KEY, getCookieOptions());
  },

  clear: (): void => {
    authStorage.removeToken();
    authStorage.removeUserData();
  },
};
