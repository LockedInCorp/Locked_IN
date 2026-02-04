const USER_DATA_KEY = 'auth_user_data';

const getCookieOptions = () => {
  const isProduction = import.meta.env.PROD;
  const isHttps = typeof window !== 'undefined' && window.location.protocol === 'https:';
  
  return {
    maxAge: 7 * 24 * 60 * 60,
    path: '/',
    sameSite: 'Lax' as const,
    secure: isProduction || isHttps,
  };
};

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
  
  const encodedValue = encodeURIComponent(value);
  let cookieString = `${name}=${encodedValue}`;
  cookieString += `; max-age=${options.maxAge}`;
  cookieString += `; path=${options.path}`;
  cookieString += `; SameSite=${options.sameSite}`;
  
  const isProduction = import.meta.env.PROD;
  const isSecure = options.secure || isProduction;
  
  if (isSecure) {
    cookieString += '; Secure';
  }
  
  document.cookie = cookieString;
};

const removeCookie = (name: string, options: ReturnType<typeof getCookieOptions>): void => {
  if (typeof document === 'undefined') return;
  
  const isProduction = import.meta.env.PROD;
  const isSecure = options.secure || isProduction;
  
  let cookieString = `${name}=; expires=Thu, 01 Jan 1970 00:00:00 UTC`;
  cookieString += `; path=${options.path}`;
  cookieString += `; SameSite=${options.sameSite}`;
  
  if (isSecure) {
    cookieString += '; Secure';
  }
  
  document.cookie = cookieString;
};

export const persist = {
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

  clearUserData: (): void => {
    persist.removeUserData();
  },
};
