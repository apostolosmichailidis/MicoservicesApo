export interface ResponseDto<T = unknown> {
  isSuccess: boolean
  result: T
  message: string
}

// Auth
export interface LoginRequest {
  userName: string
  password: string
}

export interface RegisterRequest {
  name: string
  email: string
  phoneNumber: string
  password: string
  role?: string
}

export interface UserDto {
  id: string
  name: string
  email: string
  phoneNumber: string
}

export interface LoginResponse {
  user: UserDto
  token: string
}

export interface AuthUser {
  id: string
  name: string
  email: string
  role?: string
}

// Products
export interface ProductDto {
  productId: number
  name: string
  description: string
  price: number
  categoryName: string
  imageUrl: string
}

export type ProductFormData = Omit<ProductDto, 'productId'>

// Coupons
export interface CouponDto {
  couponId: number
  couponCode: string
  discountAmount: number
  minAmount: number
}

export type CouponFormData = Omit<CouponDto, 'couponId'>

// Cart
export interface CartDetailsDto {
  cartDetailsId: number
  cartHeaderId: number
  productId: number
  product: ProductDto
  count: number
}

export interface CartHeaderDto {
  cartHeaderId: number
  userId: string
  couponCode: string
  discount: number
  cartTotal: number
  name: string
  phone: string
  email: string
}

export interface CartDto {
  cartHeader: CartHeaderDto
  cartDetails: CartDetailsDto[]
}

// Payment
export interface CreatePaymentIntentRequest {
  orderId: string
  userId: string
  amount: number
  currency: string
}

export interface PaymentDto {
  paymentId: number
  orderId: string
  userId: string
  amount: number
  currency: string
  status: string
  stripePaymentIntentId: string | null
  stripeClientSecret: string | null
  createdAt: string
  updatedAt: string | null
}

// Email
export interface SendEmailRequest {
  recipientEmail: string
  recipientName?: string
  subject: string
  body: string
}

export interface EmailDto {
  emailId: number
  recipientEmail: string
  recipientName: string | null
  subject: string
  body: string
  status: string
  sentAt: string | null
  createdAt: string
  errorMessage: string | null
}
