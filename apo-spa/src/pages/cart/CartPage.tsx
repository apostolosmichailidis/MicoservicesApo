import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query'
import { getCart, removeFromCart } from '../../api/cart'
import { useAuth } from '../../context/AuthContext'

export default function CartPage() {
  const { user } = useAuth()
  const queryClient = useQueryClient()

  const { data: cart, isLoading, error } = useQuery({
    queryKey: ['cart', user?.id],
    queryFn: () => getCart(user!.id),
    enabled: !!user?.id,
  })

  const removeMutation = useMutation({
    mutationFn: removeFromCart,
    onSuccess: () => queryClient.invalidateQueries({ queryKey: ['cart'] }),
  })

  if (isLoading) return <p className="text-gray-500">Loading cart…</p>
  if (error)
    return <p className="text-red-500">Failed to load cart.</p>
  if (!cart || cart.cartDetails.length === 0)
    return (
      <div className="text-center py-16">
        <h1 className="text-2xl font-bold mb-2">Your Cart</h1>
        <p className="text-gray-500">Your cart is empty.</p>
      </div>
    )

  const { cartHeader, cartDetails } = cart

  return (
    <div>
      <h1 className="text-2xl font-bold mb-6">Your Cart</h1>

      <div className="bg-white border border-gray-200 rounded shadow-sm overflow-hidden mb-6">
        <table className="min-w-full text-sm">
          <thead className="bg-gray-50 text-gray-600 uppercase text-xs">
            <tr>
              <th className="px-4 py-3 text-left">Product</th>
              <th className="px-4 py-3 text-right">Unit Price</th>
              <th className="px-4 py-3 text-right">Qty</th>
              <th className="px-4 py-3 text-right">Subtotal</th>
              <th className="px-4 py-3" />
            </tr>
          </thead>
          <tbody className="divide-y divide-gray-100">
            {cartDetails.map((item) => (
              <tr key={item.cartDetailsId} className="hover:bg-gray-50">
                <td className="px-4 py-3 font-medium">{item.product?.name ?? `#${item.productId}`}</td>
                <td className="px-4 py-3 text-right">${item.product?.price?.toFixed(2) ?? '—'}</td>
                <td className="px-4 py-3 text-right">{item.count}</td>
                <td className="px-4 py-3 text-right">
                  ${((item.product?.price ?? 0) * item.count).toFixed(2)}
                </td>
                <td className="px-4 py-3 text-right">
                  <button
                    onClick={() => removeMutation.mutate(item.cartDetailsId)}
                    disabled={removeMutation.isPending}
                    className="text-red-500 hover:underline text-xs disabled:opacity-50"
                  >
                    Remove
                  </button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>

      <div className="flex justify-end">
        <div className="bg-white border border-gray-200 rounded shadow-sm p-4 w-64 text-sm space-y-2">
          {cartHeader.couponCode && (
            <div className="flex justify-between text-green-600">
              <span>Coupon ({cartHeader.couponCode})</span>
              <span>-${cartHeader.discount.toFixed(2)}</span>
            </div>
          )}
          <div className="flex justify-between font-bold text-base border-t pt-2">
            <span>Total</span>
            <span>${cartHeader.cartTotal.toFixed(2)}</span>
          </div>
        </div>
      </div>
    </div>
  )
}
