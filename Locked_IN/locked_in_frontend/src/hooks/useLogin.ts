import { useMutation } from '@tanstack/react-query';
import { loginUser, type LoginRequest, type ApiResponse, type UserProfileDto } from '@/lib/api';
import { useAuthStore } from '@/stores/authStore';
import { useNavigate } from 'react-router-dom';

/**
 * Custom hook for user login
 * Handles login mutation, error handling, and success callbacks
 */
export function useLogin() {
  const navigate = useNavigate();
  const { setUser, resetLoginForm } = useAuthStore();

  return useMutation<ApiResponse<UserProfileDto>, Error, LoginRequest>({
    mutationFn: loginUser,
    onSuccess: (data) => {
      if (data.success && data.data) {
        setUser({
          id: data.data.id.toString(),
          email: data.data.email,
          nickname: data.data.username,
          avatarUrl: typeof data.data.avatar === 'string' ? data.data.avatar : undefined,
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
