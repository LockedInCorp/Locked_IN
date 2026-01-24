import { useMutation } from '@tanstack/react-query';
import { logoutUser, type ApiResponse } from '@/lib/api';
import { useAuthStore } from '@/stores/authStore';
import { useNavigate } from 'react-router-dom';
import { tokenStorage } from '@/utils/tokenStorage';

/**
 * Custom hook for user logout
 * Handles logout mutation, error handling, and success callbacks
 */
export function useLogout() {
  const navigate = useNavigate();
  const { logout } = useAuthStore();

  return useMutation<ApiResponse<null>, Error, void>({
    mutationFn: () => logoutUser(),
    onSuccess: () => {
      console.log('Logout successful');
      tokenStorage.removeToken();
      logout();
      navigate('/');
    },
    onError: (error) => {
      console.error('Logout error:', error);
      // Even if logout fails on server, clear local state
      tokenStorage.removeToken();
      logout();
      navigate('/');
    },
  });
}
