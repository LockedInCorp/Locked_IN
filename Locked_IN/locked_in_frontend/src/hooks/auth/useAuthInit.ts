import { useEffect } from 'react';
import { useAuthStore } from '@/stores/authStore';
import { tokenStorage } from '@/utils/auth/tokenStorage';
import { getUserProfile } from '@/utils/profile/profileApi';
import { extractAvatarFromResponse } from '@/utils/profile/avatarUtils';

export function useAuthInit() {
  const { setUser } = useAuthStore();

  useEffect(() => {
    const token = tokenStorage.getToken();
    const userData = tokenStorage.getUserData();

    if (token && userData) {
      setUser(userData);
      
      getUserProfile(parseInt(userData.id))
        .then(async (response) => {
          if (response.success && response.data) {
            const avatarUrl = await extractAvatarFromResponse(response.data as any);
            const updatedUserData = {
              ...userData,
              nickname: response.data.username,
              email: response.data.email,
              avatarUrl: avatarUrl || userData.avatarUrl,
            };
            tokenStorage.setUserData(updatedUserData);
            setUser(updatedUserData);
          }
        })
        .catch(() => {
        });
    } else if (token && !userData) {
      tokenStorage.removeToken();
    }
  }, [setUser]);
}
