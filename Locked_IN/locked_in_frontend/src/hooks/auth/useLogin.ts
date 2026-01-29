import { useMutation } from '@tanstack/react-query';
import { loginUser, type LoginRequest, type ApiResponse, type UserProfileDto } from '@/lib/api';
import { useAuthStore } from '@/stores/authStore';
import { useNavigate } from 'react-router-dom';
import { extractAvatarFromResponse } from '@/utils/profile/avatarUtils';
import { tokenStorage } from '@/utils/auth/tokenStorage';

export function useLogin() {
  const navigate = useNavigate();
  const { setUser, resetLoginForm } = useAuthStore();

  return useMutation<ApiResponse<UserProfileDto>, Error, LoginRequest>({
    mutationFn: loginUser,
    onSuccess: async (data) => {
      if (data.success && data.data) {
        if (data.data.token) {
          tokenStorage.setToken(data.data.token);
        }
        
        const avatarUrl = await extractAvatarFromResponse(data.data as any)
        
        setUser({
          id: data.data.id.toString(),
          email: data.data.email,
          nickname: data.data.username,
          avatarUrl: avatarUrl,
        });
        
        resetLoginForm();
        
        navigate('/groups');
      }
    },
  });
}
