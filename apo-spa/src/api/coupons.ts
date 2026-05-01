import { apiClient } from './client'
import type { CouponDto, CouponFormData, ResponseDto } from '../types'

export async function getCoupons(): Promise<CouponDto[]> {
  const res = await apiClient.get<ResponseDto<CouponDto[]>>('/api/coupon')
  if (!res.data.isSuccess) throw new Error(res.data.message)
  return res.data.result
}

export async function getCoupon(id: number): Promise<CouponDto> {
  const res = await apiClient.get<ResponseDto<CouponDto>>(`/api/coupon/${id}`)
  if (!res.data.isSuccess) throw new Error(res.data.message)
  return res.data.result
}

export async function createCoupon(data: CouponFormData): Promise<CouponDto> {
  const res = await apiClient.post<ResponseDto<CouponDto>>('/api/coupon', data)
  if (!res.data.isSuccess) throw new Error(res.data.message)
  return res.data.result
}

export async function deleteCoupon(id: number): Promise<void> {
  const res = await apiClient.delete<ResponseDto<unknown>>(`/api/coupon/${id}`)
  if (!res.data.isSuccess) throw new Error(res.data.message)
}
