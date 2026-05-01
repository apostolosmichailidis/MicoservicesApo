import { apiClient } from './client'
import type { CartDto, ResponseDto } from '../types'

export async function getCart(userId: string): Promise<CartDto> {
  const res = await apiClient.get<ResponseDto<CartDto>>(`/api/ShoppingCartApi/GetCart/${userId}`)
  if (!res.data.isSuccess) throw new Error(res.data.message)
  return res.data.result
}

export async function upsertCart(data: CartDto): Promise<CartDto> {
  const res = await apiClient.post<ResponseDto<CartDto>>('/api/ShoppingCartApi/CartUpsert', data)
  if (!res.data.isSuccess) throw new Error(res.data.message)
  return res.data.result
}

export async function removeFromCart(cartDetailsId: number): Promise<void> {
  const res = await apiClient.post<ResponseDto<unknown>>('/api/ShoppingCartApi/RemoveCart', {
    cartDetailsId,
  })
  if (!res.data.isSuccess) throw new Error(res.data.message)
}
