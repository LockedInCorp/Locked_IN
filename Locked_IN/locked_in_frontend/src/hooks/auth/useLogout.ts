import { useMutation } from '@tanstack/react-query';
import { logoutUser } from '@/api/api';
import { useAuthStore } from '@/stores/authStore';
import { useNavigate } from 'react-router-dom';
import { authStorage } from '@/utils/auth/authStorage';

export function useLogout() {
  const navigate = useNavigate();
  const { logout } = useAuthStore();

  return useMutation<void, Error, void>({
    mutationFn: () => logoutUser(),
    onSuccess: () => {
      authStorage.clear();
      logout();
      navigate('/');
    },
    onError: (error) => {
      console.error('Logout error:', error);

      authStorage.clear();
      logout();
      navigate('/');
    },
  });
}
