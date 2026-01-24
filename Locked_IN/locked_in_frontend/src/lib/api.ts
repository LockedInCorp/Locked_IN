export { API_BASE_URL } from './apiClient';
import { apiClient } from './apiClient';

export interface ApiResponse<T> {
  success: boolean;
  message: string;
  data?: T;
}

export interface UserProfileDto {
  id: number;
  email: string;
  username: string;
  avatar?: File | string;
  avatarURL?: string;
  token?: string;
  availability?: Record<string, string[]>;
}

export interface RegisterRequest {
  username: string;
  email: string;
  password: string;
  avatar?: File | null;
}

export interface LoginRequest {
  username: string;
  password: string;
}

export async function registerUser(data: RegisterRequest): Promise<ApiResponse<UserProfileDto>> {
  const formData = new FormData();
  formData.append('username', data.username);
  formData.append('email', data.email);
  formData.append('password', data.password);
  
  if (data.avatar) {
    formData.append('avatar', data.avatar);
  }

  try {
    const response = await apiClient.post<ApiResponse<UserProfileDto>>('/user/register', formData, {
      headers: {
        'Content-Type': 'multipart/form-data',
      },
    });
    return response.data;
  } catch (error: any) {
    const errorData = error.response?.data || { 
      success: false, 
      message: 'Registration failed. Please try again.' 
    };
    throw new Error(errorData.message || 'Registration failed');
  }
}

export async function loginUser(data: LoginRequest): Promise<ApiResponse<UserProfileDto>> {
  try {
    const response = await apiClient.post<ApiResponse<UserProfileDto>>('/user/login', {
      username: data.username,
      password: data.password,
    });
    return response.data;
  } catch (error: any) {
    const errorData = error.response?.data || { 
      success: false, 
      message: 'Login failed. Please check your credentials.' 
    };
    throw new Error(errorData.message || 'Login failed');
  }
}

export async function logoutUser(): Promise<ApiResponse<null>> {
  try {
    const response = await apiClient.post<ApiResponse<null>>('/user/logout');
    return response.data;
  } catch (error: any) {
    const errorData = error.response?.data || { 
      success: false, 
      message: 'Logout failed. Please try again.' 
    };
    throw new Error(errorData.message || 'Logout failed');
  }
}
