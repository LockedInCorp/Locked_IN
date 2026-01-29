import { useEffect } from 'react';
import { useAuthStore } from '@/stores/authStore';
import { tokenStorage } from '@/utils/auth/tokenStorage';

export function useAuthInit() {
  const { setUser } = useAuthStore();

  useEffect(() => {
    const token = tokenStorage.getToken();
    const userData = tokenStorage.getUserData();

    if (token && userData) {
      setUser(userData);
    } else if (token && !userData) {
      tokenStorage.removeToken();
    }
  }, [setUser]);
}
