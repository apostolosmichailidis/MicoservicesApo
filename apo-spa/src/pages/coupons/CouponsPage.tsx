import { useState } from 'react'
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query'
import { Link } from 'react-router-dom'
import { getCoupons, deleteCoupon } from '../../api/coupons'

export default function CouponsPage() {
  const queryClient = useQueryClient()
  const [deletingId, setDeletingId] = useState<number | null>(null)

  const { data: coupons = [], isLoading, error } = useQuery({
    queryKey: ['coupons'],
    queryFn: getCoupons,
  })

  const deleteMutation = useMutation({
    mutationFn: deleteCoupon,
    onSuccess: () => queryClient.invalidateQueries({ queryKey: ['coupons'] }),
    onSettled: () => setDeletingId(null),
  })

  function handleDelete(id: number) {
    if (!confirm('Delete this coupon?')) return
    setDeletingId(id)
    deleteMutation.mutate(id)
  }

  if (isLoading) return <p className="text-gray-500">Loading coupons…</p>
  if (error) return <p className="text-red-500">Failed to load coupons.</p>

  return (
    <div>
      <div className="flex justify-between items-center mb-6">
        <h1 className="text-2xl font-bold">Coupons</h1>
        <Link
          to="/coupons/create"
          className="bg-indigo-600 text-white px-4 py-2 rounded hover:bg-indigo-700 text-sm"
        >
          + New Coupon
        </Link>
      </div>

      {coupons.length === 0 ? (
        <p className="text-gray-500">No coupons yet.</p>
      ) : (
        <div className="overflow-x-auto">
          <table className="min-w-full bg-white border border-gray-200 rounded shadow-sm text-sm">
            <thead className="bg-gray-50 text-gray-600 uppercase text-xs">
              <tr>
                <th className="px-4 py-3 text-left">ID</th>
                <th className="px-4 py-3 text-left">Code</th>
                <th className="px-4 py-3 text-right">Discount</th>
                <th className="px-4 py-3 text-center">Actions</th>
              </tr>
            </thead>
            <tbody className="divide-y divide-gray-100">
              {coupons.map((c) => (
                <tr key={c.couponId} className="hover:bg-gray-50">
                  <td className="px-4 py-3 text-gray-400">{c.couponId}</td>
                  <td className="px-4 py-3 font-medium font-mono">{c.couponCode}</td>
                  <td className="px-4 py-3 text-right text-green-600">
                    ${c.discountAmount.toFixed(2)}
                  </td>
                  <td className="px-4 py-3 text-center">
                    <button
                      onClick={() => handleDelete(c.couponId)}
                      disabled={deletingId === c.couponId}
                      className="text-red-500 hover:underline disabled:opacity-50"
                    >
                      Delete
                    </button>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}
    </div>
  )
}
