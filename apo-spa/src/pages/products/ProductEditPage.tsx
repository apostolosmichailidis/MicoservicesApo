import { useEffect } from 'react'
import { useForm } from 'react-hook-form'
import { zodResolver } from '@hookform/resolvers/zod'
import { z } from 'zod'
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query'
import { useNavigate, useParams } from 'react-router-dom'
import { getProduct, updateProduct } from '../../api/products'

const schema = z.object({
  name: z.string().min(1, 'Name is required'),
  description: z.string().min(1, 'Description is required'),
  price: z.coerce.number().positive('Price must be positive'),
  categoryName: z.string().min(1, 'Category is required'),
  imageUrl: z.string().url('Must be a valid URL').or(z.literal('')),
})

type FormData = z.infer<typeof schema>

export default function ProductEditPage() {
  const { id } = useParams<{ id: string }>()
  const productId = Number(id)
  const queryClient = useQueryClient()
  const navigate = useNavigate()

  const { data: product, isLoading } = useQuery({
    queryKey: ['products', productId],
    queryFn: () => getProduct(productId),
    enabled: !!productId,
  })

  const {
    register,
    handleSubmit,
    reset,
    formState: { errors },
  } = useForm<FormData>({ resolver: zodResolver(schema) })

  useEffect(() => {
    if (product) reset(product)
  }, [product, reset])

  const mutation = useMutation({
    mutationFn: (data: FormData) => updateProduct({ ...data, productId }),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['products'] })
      navigate('/products')
    },
  })

  if (isLoading) return <p className="text-gray-500">Loading…</p>
  if (!product) return <p className="text-red-500">Product not found.</p>

  return (
    <div className="max-w-lg mx-auto">
      <h1 className="text-2xl font-bold mb-6">Edit Product</h1>

      {mutation.error && (
        <div className="bg-red-50 border border-red-200 text-red-700 px-4 py-2 rounded mb-4 text-sm">
          {(mutation.error as Error).message}
        </div>
      )}

      <form onSubmit={handleSubmit((data) => mutation.mutate(data))} className="space-y-4">
        {(
          [
            { name: 'name', label: 'Name' },
            { name: 'description', label: 'Description' },
            { name: 'price', label: 'Price', type: 'number' },
            { name: 'categoryName', label: 'Category' },
            { name: 'imageUrl', label: 'Image URL' },
          ] as const
        ).map(({ name, label, type = 'text' }) => (
          <div key={name}>
            <label className="block text-sm font-medium text-gray-700 mb-1">{label}</label>
            <input
              {...register(name)}
              type={type}
              step={name === 'price' ? '0.01' : undefined}
              className="w-full border border-gray-300 rounded px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-indigo-500"
            />
            {errors[name] && (
              <p className="text-red-500 text-xs mt-1">{errors[name]?.message}</p>
            )}
          </div>
        ))}

        <div className="flex gap-3 pt-2">
          <button
            type="submit"
            disabled={mutation.isPending}
            className="bg-indigo-600 text-white px-5 py-2 rounded hover:bg-indigo-700 disabled:opacity-60 text-sm"
          >
            {mutation.isPending ? 'Saving…' : 'Save Changes'}
          </button>
          <button
            type="button"
            onClick={() => navigate('/products')}
            className="border border-gray-300 px-5 py-2 rounded hover:bg-gray-50 text-sm"
          >
            Cancel
          </button>
        </div>
      </form>
    </div>
  )
}
