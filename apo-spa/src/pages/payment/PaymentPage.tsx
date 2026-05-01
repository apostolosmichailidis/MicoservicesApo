import { useState } from 'react'
import { useForm } from 'react-hook-form'
import { zodResolver } from '@hookform/resolvers/zod'
import { z } from 'zod'
import { useMutation, useQuery } from '@tanstack/react-query'
import { createPaymentIntent, getPaymentsByOrder, refundPayment } from '../../api/payment'
import { useAuth } from '../../context/AuthContext'
import type { PaymentDto } from '../../types'

const schema = z.object({
  orderId: z.string().min(1, 'Order ID is required'),
  amount: z.coerce.number().positive('Amount must be positive'),
  currency: z.string().min(3, 'Currency required').max(3),
})

type FormData = z.infer<typeof schema>

function statusBadge(status: string) {
  const colours: Record<string, string> = {
    Pending: 'bg-yellow-100 text-yellow-700',
    Succeeded: 'bg-green-100 text-green-700',
    Failed: 'bg-red-100 text-red-700',
    Refunded: 'bg-gray-100 text-gray-700',
  }
  return (
    <span className={`px-2 py-0.5 rounded text-xs font-medium ${colours[status] ?? 'bg-gray-100'}`}>
      {status}
    </span>
  )
}

export default function PaymentPage() {
  const { user } = useAuth()
  const [orderId, setOrderId] = useState('')
  const [searchOrderId, setSearchOrderId] = useState('')
  const [createdPayment, setCreatedPayment] = useState<PaymentDto | null>(null)

  const {
    register,
    handleSubmit,
    formState: { errors },
    reset,
  } = useForm<FormData>({
    resolver: zodResolver(schema),
    defaultValues: { currency: 'usd' },
  })

  const createMutation = useMutation({
    mutationFn: createPaymentIntent,
    onSuccess: (data) => {
      setCreatedPayment(data)
      reset()
    },
  })

  const { data: orderPayments = [], refetch: fetchOrder } = useQuery({
    queryKey: ['payments', 'order', searchOrderId],
    queryFn: () => getPaymentsByOrder(searchOrderId),
    enabled: false,
  })

  const refundMutation = useMutation({
    mutationFn: refundPayment,
    onSuccess: () => fetchOrder(),
  })

  return (
    <div className="space-y-10">
      {/* Create intent */}
      <section>
        <h1 className="text-2xl font-bold mb-4">Create Payment Intent</h1>

        {createMutation.error && (
          <div className="bg-red-50 border border-red-200 text-red-700 px-4 py-2 rounded mb-4 text-sm">
            {(createMutation.error as Error).message}
          </div>
        )}

        {createdPayment && (
          <div className="bg-green-50 border border-green-200 text-green-800 px-4 py-3 rounded mb-4 text-sm space-y-1">
            <p className="font-medium">Payment intent created</p>
            <p>Payment ID: <span className="font-mono">{createdPayment.paymentId}</span></p>
            <p>Status: {statusBadge(createdPayment.status)}</p>
            {createdPayment.stripeClientSecret && (
              <p className="truncate">
                Client secret: <span className="font-mono text-xs">{createdPayment.stripeClientSecret}</span>
              </p>
            )}
          </div>
        )}

        <form
          onSubmit={handleSubmit((data) =>
            createMutation.mutate({ ...data, userId: user!.id }),
          )}
          className="grid grid-cols-1 sm:grid-cols-3 gap-4 max-w-lg"
        >
          <div className="sm:col-span-2">
            <label className="block text-sm font-medium text-gray-700 mb-1">Order ID</label>
            <input
              {...register('orderId')}
              className="w-full border border-gray-300 rounded px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-indigo-500"
            />
            {errors.orderId && (
              <p className="text-red-500 text-xs mt-1">{errors.orderId.message}</p>
            )}
          </div>

          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">Currency</label>
            <input
              {...register('currency')}
              className="w-full border border-gray-300 rounded px-3 py-2 text-sm uppercase focus:outline-none focus:ring-2 focus:ring-indigo-500"
            />
          </div>

          <div className="sm:col-span-2">
            <label className="block text-sm font-medium text-gray-700 mb-1">Amount</label>
            <input
              {...register('amount')}
              type="number"
              step="0.01"
              className="w-full border border-gray-300 rounded px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-indigo-500"
            />
            {errors.amount && (
              <p className="text-red-500 text-xs mt-1">{errors.amount.message}</p>
            )}
          </div>

          <div className="flex items-end">
            <button
              type="submit"
              disabled={createMutation.isPending}
              className="w-full bg-indigo-600 text-white py-2 rounded hover:bg-indigo-700 disabled:opacity-60 text-sm"
            >
              {createMutation.isPending ? 'Creating…' : 'Create'}
            </button>
          </div>
        </form>
      </section>

      {/* Lookup by order */}
      <section>
        <h2 className="text-xl font-semibold mb-4">Look up Payments by Order</h2>
        <div className="flex gap-2 max-w-md mb-4">
          <input
            value={orderId}
            onChange={(e) => setOrderId(e.target.value)}
            placeholder="Order ID"
            className="flex-1 border border-gray-300 rounded px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-indigo-500"
          />
          <button
            onClick={() => {
              setSearchOrderId(orderId)
              setTimeout(() => fetchOrder(), 0)
            }}
            className="bg-gray-700 text-white px-4 py-2 rounded hover:bg-gray-600 text-sm"
          >
            Search
          </button>
        </div>

        {orderPayments.length > 0 && (
          <div className="overflow-x-auto">
            <table className="min-w-full bg-white border border-gray-200 rounded shadow-sm text-sm">
              <thead className="bg-gray-50 text-gray-600 uppercase text-xs">
                <tr>
                  <th className="px-4 py-3 text-left">ID</th>
                  <th className="px-4 py-3 text-right">Amount</th>
                  <th className="px-4 py-3 text-left">Currency</th>
                  <th className="px-4 py-3 text-left">Status</th>
                  <th className="px-4 py-3 text-left">Created</th>
                  <th className="px-4 py-3" />
                </tr>
              </thead>
              <tbody className="divide-y divide-gray-100">
                {orderPayments.map((p) => (
                  <tr key={p.paymentId} className="hover:bg-gray-50">
                    <td className="px-4 py-3 text-gray-400">{p.paymentId}</td>
                    <td className="px-4 py-3 text-right">{p.amount.toFixed(2)}</td>
                    <td className="px-4 py-3 uppercase">{p.currency}</td>
                    <td className="px-4 py-3">{statusBadge(p.status)}</td>
                    <td className="px-4 py-3 text-gray-500">
                      {new Date(p.createdAt).toLocaleDateString()}
                    </td>
                    <td className="px-4 py-3 text-right">
                      {p.status === 'Succeeded' && (
                        <button
                          onClick={() => refundMutation.mutate(p.paymentId)}
                          disabled={refundMutation.isPending}
                          className="text-orange-600 hover:underline text-xs disabled:opacity-50"
                        >
                          Refund
                        </button>
                      )}
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        )}
      </section>
    </div>
  )
}
