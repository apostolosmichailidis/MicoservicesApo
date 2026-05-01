import { apiClient } from './client'
import type { LoginRequest, LoginResponse, RegisterRequest, ResponseDto } from '../types'

export async function login(data: LoginRequest): Promise<LoginResponse> {
  const res = await apiClient.post<ResponseDto<LoginResponse>>('/api/auth/login', data)
  if (!res.data.isSuccess) throw new Error(res.data.message)
  return res.data.result
}

export async function register(data: RegisterRequest): Promise<void> {
  const res = await apiClient.post<ResponseDto<unknown>>('/api/auth/register', data)
  if (!res.data.isSuccess) throw new Error(res.data.message)
}
