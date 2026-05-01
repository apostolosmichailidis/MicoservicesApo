import { apiClient } from './client'
import type { EmailDto, ResponseDto, SendEmailRequest } from '../types'

export async function sendEmail(data: SendEmailRequest): Promise<EmailDto> {
  const res = await apiClient.post<ResponseDto<EmailDto>>('/api/email/send', data)
  if (!res.data.isSuccess) throw new Error(res.data.message)
  return res.data.result
}

export async function getEmail(id: number): Promise<EmailDto> {
  const res = await apiClient.get<ResponseDto<EmailDto>>(`/api/email/${id}`)
  if (!res.data.isSuccess) throw new Error(res.data.message)
  return res.data.result
}

export async function getEmailsByRecipient(email: string): Promise<EmailDto[]> {
  const res = await apiClient.get<ResponseDto<EmailDto[]>>(
    `/api/email/recipient/${encodeURIComponent(email)}`,
  )
  if (!res.data.isSuccess) throw new Error(res.data.message)
  return res.data.result
}

export async function retryEmail(id: number): Promise<EmailDto> {
  const res = await apiClient.post<ResponseDto<EmailDto>>(`/api/email/retry/${id}`)
  if (!res.data.isSuccess) throw new Error(res.data.message)
  return res.data.result
}
