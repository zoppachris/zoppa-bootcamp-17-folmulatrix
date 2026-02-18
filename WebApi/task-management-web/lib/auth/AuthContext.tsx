'use client';

import React, { createContext, useContext, useEffect, useState, useCallback } from 'react';
import { useRouter, usePathname } from 'next/navigation';
import { apiGet, apiPost } from '@/lib/api/client';
import { User, LoginCredentials, RegisterData } from '@/types';

interface AuthContextType {
  user: User | null;
  isLoading: boolean;
  login: (credentials: LoginCredentials) => Promise<void>;
  register: (data: RegisterData) => Promise<void>;
  logout: () => Promise<void>;
  refreshUser: () => Promise<void>;
  checkAuth: () => Promise<boolean>;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

const PUBLIC_PATHS = ['/login', '/register', '/'];

export function AuthProvider({ children }: { children: React.ReactNode }) {
  const [user, setUser] = useState<User | null>(null);
  const [isLoading, setIsLoading] = useState(true);
  const router = useRouter();
  const pathname = usePathname();

  const checkAuth = useCallback(async (): Promise<boolean> => {
    try {
      await apiGet<User>('/users/me', {}, false);
      return true;
    } catch (error) {
      return false;
    }
  }, []);

  const refreshUser = useCallback(async (): Promise<void> => {
    try {
      const userData = await apiGet<User>('/users/me');
      setUser(userData);
    } catch (error) {
      setUser(null);
    } finally {
      setIsLoading(false);
    }
  }, []);

  useEffect(() => {
    let intervalId: NodeJS.Timeout;

    const initAuth = async () => {
      const isPublicPath = PUBLIC_PATHS.includes(pathname);

      try {
        const userData = await apiGet<User>('/users/me');
        setUser(userData);

        if (isPublicPath && userData) {
          router.push('/dashboard');
        }
      } catch (error) {
        setUser(null);

        if (!isPublicPath) {
          router.push('/login');
        }
      } finally {
        setIsLoading(false);
      }
    };

    initAuth();

    intervalId = setInterval(async () => {
      const isPublicPath = PUBLIC_PATHS.includes(pathname);

      if (!isPublicPath) {
        const isAuthenticated = await checkAuth();
        if (!isAuthenticated) {
          setUser(null);
          router.push('/login');
        }
      }
    }, 5 * 60 * 1000);

    return () => clearInterval(intervalId);
  }, [pathname, router, checkAuth]);

  const login = async (credentials: LoginCredentials) => {
    setIsLoading(true);
    try {
      await apiPost('/auth/login', credentials);
      await refreshUser();
      router.push('/dashboard');
      router.refresh();
    } catch (error) {
      console.error('Login error:', error);
      throw error;
    } finally {
      setIsLoading(false);
    }
  };

  const register = async (data: RegisterData) => {
    setIsLoading(true);
    try {
      await apiPost('/auth/register', data);
      await refreshUser();
      router.push('/dashboard');
      router.refresh();
    } catch (error) {
      console.error('Register error:', error);
      throw error;
    } finally {
      setIsLoading(false);
    }
  };

  const logout = async () => {
    setIsLoading(true);
    try {
      await apiPost('/auth/logout');
    } catch (error) {
      console.error('Logout error:', error);
    } finally {
      setUser(null);
      setIsLoading(false);
      router.push('/login');
      router.refresh();
    }
  };

  return (
    <AuthContext.Provider value={{
      user,
      isLoading,
      login,
      register,
      logout,
      refreshUser,
      checkAuth
    }}>
      {children}
    </AuthContext.Provider>
  );
}

export function useAuth() {
  const context = useContext(AuthContext);
  if (!context) throw new Error('useAuth must be used within AuthProvider');
  return context;
}