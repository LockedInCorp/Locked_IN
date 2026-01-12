const API_BASE_URL = 'http://localhost:5122/api';

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

/**
 * Makes a POST request to the registration endpoint with FormData
 * @param data Registration data including optional avatar file
 * @returns Promise with API response containing user profile
 */
export async function registerUser(data: RegisterRequest): Promise<ApiResponse<UserProfileDto>> {
  const formData = new FormData();
  formData.append('username', data.username);
  formData.append('email', data.email);
  formData.append('password', data.password);
  
  if (data.avatar) {
    formData.append('avatar', data.avatar);
  }

  const response = await fetch(`${API_BASE_URL}/user/register`, {
    method: 'POST',
    body: formData,
    credentials: 'include', // Include cookies for authentication
  });

  if (!response.ok) {
    const errorData = await response.json().catch(() => ({ 
      success: false, 
      message: 'Registration failed. Please try again.' 
    }));
    throw new Error(errorData.message || 'Registration failed');
  }

  return response.json();
}

/**
 * Makes a POST request to the login endpoint with JSON
 * @param data Login credentials (username and password)
 * @returns Promise with API response containing user profile
 */
export async function loginUser(data: LoginRequest): Promise<ApiResponse<UserProfileDto>> {
  const response = await fetch(`${API_BASE_URL}/user/login`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify({
      username: data.username,
      password: data.password,
    }),
    credentials: 'include', // Include cookies for authentication
  });

  if (!response.ok) {
    const errorData = await response.json().catch(() => ({ 
      success: false, 
      message: 'Login failed. Please check your credentials.' 
    }));
    throw new Error(errorData.message || 'Login failed');
  }

  return response.json();
}

/**
 * Makes a POST request to the logout endpoint
 * @returns Promise with API response containing logout status
 */
export async function logoutUser(): Promise<ApiResponse<null>> {
  const response = await fetch(`${API_BASE_URL}/user/logout`, {
    method: 'POST',
    credentials: 'include', // Include cookies for authentication
  });

  if (!response.ok) {
    const errorData = await response.json().catch(() => ({ 
      success: false, 
      message: 'Logout failed. Please try again.' 
    }));
    throw new Error(errorData.message || 'Logout failed');
  }

  return response.json();
}
