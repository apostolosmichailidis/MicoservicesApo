import { apiClient } from './client'
import type { CreatePaymentIntentRequest, PaymentDto, ResponseDto } from '../types'

export async function createPaymentIntent(
  data: CreatePaymentIntentRequest,
): Promise<PaymentDto> {
  const res = await apiClient.post<ResponseDto<PaymentDto>>('/api/payment/create-intent', data)
  if (!res.data.isSuccess) throw new Error(res.data.message)
  return res.data.result
}

export async function getPayment(id: number): Promise<PaymentDto> {
  const res = await apiClient.get<ResponseDto<PaymentDto>>(`/api/payment/${id}`)
  if (!res.data.isSuccess) throw new Error(res.data.message)
  return res.data.result
}

export async function getPaymentsByOrder(orderId: string): Promise<PaymentDto[]> {
  const res = await apiClient.get<ResponseDto<PaymentDto[]>>(`/api/payment/order/${orderId}`)
  if (!res.data.isSuccess) throw new Error(res.data.message)
  return res.data.result
}

export async function refundPayment(id: number): Promise<PaymentDto> {
  const res = await apiClient.post<ResponseDto<PaymentDto>>(`/api/payment/refund/${id}`)
  if (!res.data.isSuccess) throw new Error(res.data.message)
  return res.data.result
}
