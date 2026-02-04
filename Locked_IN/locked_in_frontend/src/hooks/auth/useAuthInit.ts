import { useEffect } from 'react';
import { useAuthStore } from '@/stores/authStore';
import { tokenStorage } from '@/utils/auth/cookieStorage';
import { getUserProfile } from '@/api/api';
import { extractAvatarFromResponse } from '@/utils/profile/avatarUtils';

export function useAuthInit() {
  const { setUser } = useAuthStore();

  useEffect(() => {
    const token = tokenStorage.getToken();
    const userData = tokenStorage.getUserData();

    if (token && userData) {
      setUser(userData);
      
      getUserProfile(parseInt(userData.id))
        .then(async (profile) => {
          if (profile) {
            const avatarUrl = await extractAvatarFromResponse(profile as any);
            const updatedUserData = {
              ...userData,
              nickname: profile.username,
              email: profile.email,
              avatarUrl: avatarUrl || userData.avatarUrl,
            };
            tokenStorage.setUserData(updatedUserData);
            setUser(updatedUserData);
          }
        })
        .catch((error) => {
          console.error('Failed to refresh user profile:', error);
        });
    } else if (token && !userData) {
      tokenStorage.removeToken();
      setUser(null);
    }
  }, [setUser]);
}
