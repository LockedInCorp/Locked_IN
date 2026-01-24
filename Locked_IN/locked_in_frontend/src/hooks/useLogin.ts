import { useMutation } from '@tanstack/react-query';
import { loginUser, type LoginRequest, type ApiResponse, type UserProfileDto, API_BASE_URL } from '@/lib/api';
import { useAuthStore } from '@/stores/authStore';
import { useNavigate } from 'react-router-dom';
import { extractAvatarFromResponse } from '@/utils/avatarUtils';

/**
 * Custom hook for user login
 * Handles login mutation, error handling, and success callbacks
 */
export function useLogin() {
  const navigate = useNavigate();
  const { setUser, resetLoginForm } = useAuthStore();

  return useMutation<ApiResponse<UserProfileDto>, Error, LoginRequest>({
    mutationFn: loginUser,
    onSuccess: async (data) => {
      if (data.success && data.data) {
        const avatarUrl = await extractAvatarFromResponse(data.data as any, API_BASE_URL)
        
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
    onError: (error) => {
      console.error('Login error:', error);
    },
  });
}
