import { useEffect } from 'react';
import { useAuthStore } from '@/stores/authStore';
import { persist } from '@/utils/auth/persistance';
import { getUserProfile } from '@/api/api';
import { getImageUrl, extractAvatarPath } from '@/utils/imageUtils';

export function useAuthInit() {
  const { setUser } = useAuthStore();

  useEffect(() => {
    const userData = persist.getUserData();

    if (userData) {
      setUser(userData);
      
      getUserProfile(parseInt(userData.id))
        .then(async (profile) => {
          if (profile) {
            const avatarPath = extractAvatarPath(profile);
            const avatarUrl = getImageUrl(avatarPath);
            const updatedUserData = {
              ...userData,
              nickname: profile.username,
              email: profile.email,
              avatarUrl: avatarUrl || userData.avatarUrl,
            };
            persist.setUserData(updatedUserData);
            setUser(updatedUserData);
          }
        })
        .catch((error) => {
          console.error('Failed to refresh user profile:', error);
          persist.clearUserData();
          setUser(null);
        });
    }
  }, [setUser]);
}
