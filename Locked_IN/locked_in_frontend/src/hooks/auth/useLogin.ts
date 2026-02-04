import { useMutation } from '@tanstack/react-query';
import { loginUser } from '@/api/api';
import type { LoginRequest, UserProfileDto } from '@/api/types';
import { useAuthStore } from '@/stores/authStore';
import { useNavigate } from 'react-router-dom';
import { extractAvatarPath, getImageUrl } from '@/utils/imageUtils';
import { persist } from '@/utils/auth/persistance';

export function useLogin() {
  const navigate = useNavigate();
  const { setUser, resetLoginForm } = useAuthStore();

  return useMutation<UserProfileDto, Error, LoginRequest>({
    mutationFn: loginUser,
    onSuccess: async (data) => {
      if (data) {
        const avatarUrl = getImageUrl(extractAvatarPath(data as any))
        
        const userData = {
          id: data.id.toString(),
          email: data.email,
          nickname: data.username,
          avatarUrl: avatarUrl,
        };
        
        persist.setUserData(userData);
        setUser(userData);
        
        resetLoginForm();
        
        navigate('/groups');
      }
    },
  });
}
