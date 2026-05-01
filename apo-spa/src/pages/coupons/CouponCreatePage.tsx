import { useForm } from 'react-hook-form'
import { zodResolver } from '@hookform/resolvers/zod'
import { z } from 'zod'
import { useMutation, useQueryClient } from '@tanstack/react-query'
import { useNavigate } from 'react-router-dom'
import { createCoupon } from '../../api/coupons'

const schema = z.object({
  couponCode: z.string().min(1, 'Coupon code is required'),
  discountAmount: z.coerce.number().positive('Discount must be positive'),
  minAmount: z.coerce.number().min(0, 'Min amount must be ≥ 0'),
})

type FormData = z.infer<typeof schema>

export default function CouponCreatePage() {
  const queryClient = useQueryClient()
  const navigate = useNavigate()

  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<FormData>({ resolver: zodResolver(schema) })

  const mutation = useMutation({
    mutationFn: createCoupon,
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['coupons'] })
      navigate('/coupons')
    },
  })

  return (
    <div className="max-w-md mx-auto">
      <h1 className="text-2xl font-bold mb-6">New Coupon</h1>

      {mutation.error && (
        <div className="bg-red-50 border border-red-200 text-red-700 px-4 py-2 rounded mb-4 text-sm">
          {(mutation.error as Error).message}
        </div>
      )}

      <form onSubmit={handleSubmit((data) => mutation.mutate(data))} className="space-y-4">
        <div>
          <label className="block text-sm font-medium text-gray-700 mb-1">Coupon Code</label>
          <input
            {...register('couponCode')}
            className="w-full border border-gray-300 rounded px-3 py-2 text-sm font-mono uppercase focus:outline-none focus:ring-2 focus:ring-indigo-500"
            placeholder="e.g. SAVE10"
          />
          {errors.couponCode && (
            <p className="text-red-500 text-xs mt-1">{errors.couponCode.message}</p>
          )}
        </div>

        <div>
          <label className="block text-sm font-medium text-gray-700 mb-1">Discount Amount ($)</label>
          <input
            {...register('discountAmount')}
            type="number"
            step="0.01"
            className="w-full border border-gray-300 rounded px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-indigo-500"
          />
          {errors.discountAmount && (
            <p className="text-red-500 text-xs mt-1">{errors.discountAmount.message}</p>
          )}
        </div>

        <div>
          <label className="block text-sm font-medium text-gray-700 mb-1">
            Minimum Cart Amount ($)
          </label>
          <input
            {...register('minAmount')}
            type="number"
            step="0.01"
            className="w-full border border-gray-300 rounded px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-indigo-500"
          />
          {errors.minAmount && (
            <p className="text-red-500 text-xs mt-1">{errors.minAmount.message}</p>
          )}
        </div>

        <div className="flex gap-3 pt-2">
          <button
            type="submit"
            disabled={mutation.isPending}
            className="bg-indigo-600 text-white px-5 py-2 rounded hover:bg-indigo-700 disabled:opacity-60 text-sm"
          >
            {mutation.isPending ? 'Creating…' : 'Create Coupon'}
          </button>
          <button
            type="button"
            onClick={() => navigate('/coupons')}
            className="border border-gray-300 px-5 py-2 rounded hover:bg-gray-50 text-sm"
          >
            Cancel
          </button>
        </div>
      </form>
    </div>
  )
}
