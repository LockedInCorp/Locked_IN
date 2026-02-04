import { useEffect } from 'react';
import { useAuthStore } from '@/stores/authStore';
import { authStorage } from '@/utils/auth/authStorage';
import { getUserProfile } from '@/api/api';
import { extractAvatarFromResponse } from '@/utils/profile/avatarUtils';

export function useAuthInit() {
  const { setUser } = useAuthStore();

  useEffect(() => {
    const token = authStorage.getToken();
    const userData = authStorage.getUserData();

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
            authStorage.setUserData(updatedUserData);
            setUser(updatedUserData);
          }
        })
        .catch((error) => {
          console.error('Failed to refresh user profile:', error);
        });
    } else if (token && !userData) {
      authStorage.removeToken();
      setUser(null);
    }
  }, [setUser]);
}
