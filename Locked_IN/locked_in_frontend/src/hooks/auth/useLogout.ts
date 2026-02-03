import { useMutation } from '@tanstack/react-query';
import { logoutUser } from '@/api/api';
import { useAuthStore } from '@/stores/authStore';
import { useNavigate } from 'react-router-dom';
import { tokenStorage } from '@/utils/auth/tokenStorage';

export function useLogout() {
  const navigate = useNavigate();
  const { logout } = useAuthStore();

  return useMutation<void, Error, void>({
    mutationFn: () => logoutUser(),
    onSuccess: () => {
      tokenStorage.clear();
      logout();
      navigate('/');
    },
    onError: (error) => {
      console.error('Logout error:', error);

      tokenStorage.clear();
      logout();
      navigate('/');
    },
  });
}
