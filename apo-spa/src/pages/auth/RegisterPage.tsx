import { useForm } from 'react-hook-form'
import { zodResolver } from '@hookform/resolvers/zod'
import { z } from 'zod'
import { useNavigate, Link } from 'react-router-dom'
import { useMutation } from '@tanstack/react-query'
import { register as registerUser } from '../../api/auth'

const schema = z.object({
  name: z.string().min(1, 'Name is required'),
  email: z.string().email('Invalid email'),
  phoneNumber: z.string().min(1, 'Phone number is required'),
  password: z.string().min(6, 'Password must be at least 6 characters'),
})

type FormData = z.infer<typeof schema>

export default function RegisterPage() {
  const navigate = useNavigate()

  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<FormData>({ resolver: zodResolver(schema) })

  const mutation = useMutation({
    mutationFn: registerUser,
    onSuccess: () => navigate('/login'),
  })

  return (
    <div className="max-w-sm mx-auto mt-16">
      <h2 className="text-2xl font-bold mb-6 text-center">Create Account</h2>

      {mutation.error && (
        <div className="bg-red-50 border border-red-200 text-red-700 px-4 py-2 rounded mb-4 text-sm">
          {(mutation.error as Error).message}
        </div>
      )}

      {mutation.isSuccess && (
        <div className="bg-green-50 border border-green-200 text-green-700 px-4 py-2 rounded mb-4 text-sm">
          Account created! Redirecting to login…
        </div>
      )}

      <form onSubmit={handleSubmit((data) => mutation.mutate(data))} className="space-y-4">
        {(
          [
            { name: 'name', label: 'Full Name' },
            { name: 'email', label: 'Email', type: 'email' },
            { name: 'phoneNumber', label: 'Phone Number', type: 'tel' },
            { name: 'password', label: 'Password', type: 'password' },
          ] as const
        ).map(({ name, label, type = 'text' }) => (
          <div key={name}>
            <label className="block text-sm font-medium text-gray-700 mb-1">{label}</label>
            <input
              {...register(name)}
              type={type}
              className="w-full border border-gray-300 rounded px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-indigo-500"
            />
            {errors[name] && (
              <p className="text-red-500 text-xs mt-1">{errors[name]?.message}</p>
            )}
          </div>
        ))}

        <button
          type="submit"
          disabled={mutation.isPending}
          className="w-full bg-indigo-600 text-white py-2 rounded hover:bg-indigo-700 disabled:opacity-60"
        >
          {mutation.isPending ? 'Registering…' : 'Register'}
        </button>
      </form>

      <p className="text-center text-sm text-gray-500 mt-4">
        Already have an account?{' '}
        <Link to="/login" className="text-indigo-600 hover:underline">
          Sign in
        </Link>
      </p>
    </div>
  )
}
