import { useMutation } from '@tanstack/react-query';
import { registerUser, type RegisterRequest, type ApiResponse, type UserProfileDto } from '@/lib/api';
import { useAuthStore } from '@/stores/authStore';
import { useNavigate } from 'react-router-dom';
import { tokenStorage } from '@/utils/tokenStorage';

/**
 * Custom hook for user registration
 * Handles registration mutation, error handling, and success callbacks
 */
export function useRegister() {
  const navigate = useNavigate();
  const { setUser, resetRegisterForm } = useAuthStore();

  return useMutation<ApiResponse<UserProfileDto>, Error, RegisterRequest>({
    mutationFn: registerUser,
    onSuccess: (data) => {
      if (data.success && data.data) {
        if (data.data.token) {
          tokenStorage.setToken(data.data.token);
        }

        setUser({
          id: data.data.id.toString(),
          email: data.data.email,
          nickname: data.data.username,
          avatarUrl: typeof data.data.avatar === 'string' ? data.data.avatar : undefined,
        });
        

        resetRegisterForm();
        
        navigate('/groups');
      }
    },
    onError: (error) => {

      console.error('Registration error:', error);
    },
  });
}
