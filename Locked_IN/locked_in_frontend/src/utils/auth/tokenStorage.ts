const TOKEN_KEY = 'auth_token';
const USER_DATA_KEY = 'auth_user_data';

export interface StoredUserData {
  id: string;
  email: string;
  nickname: string;
  avatarUrl?: string;
}

export const tokenStorage = {
  getToken: (): string | null => {
    if (typeof window === 'undefined') return null;
    return localStorage.getItem(TOKEN_KEY);
  },

  setToken: (token: string): void => {
    if (typeof window === 'undefined') return;
    localStorage.setItem(TOKEN_KEY, token);
  },

  removeToken: (): void => {
    if (typeof window === 'undefined') return;
    localStorage.removeItem(TOKEN_KEY);
  },

  hasToken: (): boolean => {
    return tokenStorage.getToken() !== null;
  },

  getUserData: (): StoredUserData | null => {
    if (typeof window === 'undefined') return null;
    const data = localStorage.getItem(USER_DATA_KEY);
    if (!data) return null;
    try {
      return JSON.parse(data) as StoredUserData;
    } catch {
      return null;
    }
  },

  setUserData: (userData: StoredUserData): void => {
    if (typeof window === 'undefined') return;
    localStorage.setItem(USER_DATA_KEY, JSON.stringify(userData));
  },

  removeUserData: (): void => {
    if (typeof window === 'undefined') return;
    localStorage.removeItem(USER_DATA_KEY);
  },

  clear: (): void => {
    tokenStorage.removeToken();
    tokenStorage.removeUserData();
  },
};
