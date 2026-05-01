import { apiClient } from './client'
import type { ProductDto, ProductFormData, ResponseDto } from '../types'

export async function getProducts(): Promise<ProductDto[]> {
  const res = await apiClient.get<ResponseDto<ProductDto[]>>('/api/product')
  if (!res.data.isSuccess) throw new Error(res.data.message)
  return res.data.result
}

export async function getProduct(id: number): Promise<ProductDto> {
  const res = await apiClient.get<ResponseDto<ProductDto>>(`/api/product/${id}`)
  if (!res.data.isSuccess) throw new Error(res.data.message)
  return res.data.result
}

export async function createProduct(data: ProductFormData): Promise<ProductDto> {
  const res = await apiClient.post<ResponseDto<ProductDto>>('/api/product', data)
  if (!res.data.isSuccess) throw new Error(res.data.message)
  return res.data.result
}

export async function updateProduct(data: ProductDto): Promise<ProductDto> {
  const res = await apiClient.put<ResponseDto<ProductDto>>('/api/product', data)
  if (!res.data.isSuccess) throw new Error(res.data.message)
  return res.data.result
}

export async function deleteProduct(id: number): Promise<void> {
  const res = await apiClient.delete<ResponseDto<unknown>>(`/api/product/${id}`)
  if (!res.data.isSuccess) throw new Error(res.data.message)
}
