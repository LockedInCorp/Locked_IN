import { useMutation } from '@tanstack/react-query';
import { logoutUser } from '@/api/api';
import { useAuthStore } from '@/stores/authStore';
import { useNavigate } from 'react-router-dom';
import { persist } from '@/utils/auth/persistance';
import { clearProfileStores } from '@/utils/clearProfileStores';

export function useLogout() {
  const navigate = useNavigate();
  const { logout } = useAuthStore();

  return useMutation<void, Error, void>({
    mutationFn: () => logoutUser(),
    onSuccess: () => {
      persist.clearUserData();
      clearProfileStores();
      logout();
      navigate('/');
    },
    onError: (error) => {
      console.error('Logout error:', error);

      persist.clearUserData();
      clearProfileStores();
      logout();
      navigate('/');
    },
  });
}
