import { useMutation } from '@tanstack/react-query';
import { loginUser } from '@/api/api';
import type { LoginRequest, UserProfileDto } from '@/api/types';
import { useAuthStore } from '@/stores/authStore';
import { useNavigate } from 'react-router-dom';
import { extractAvatarFromResponse } from '@/utils/profile/avatarUtils';
import { tokenStorage } from '@/utils/auth/tokenStorage';

export function useLogin() {
  const navigate = useNavigate();
  const { setUser, resetLoginForm } = useAuthStore();

  return useMutation<UserProfileDto, Error, LoginRequest>({
    mutationFn: loginUser,
    onSuccess: async (data) => {
      if (data) {
        if (data.token) {
          tokenStorage.setToken(data.token);
        }
        
        const avatarUrl = await extractAvatarFromResponse(data as any)
        
        setUser({
          id: data.id.toString(),
          email: data.email,
          nickname: data.username,
          avatarUrl: avatarUrl,
        });
        
        resetLoginForm();
        
        navigate('/groups');
      }
    },
  });
}
