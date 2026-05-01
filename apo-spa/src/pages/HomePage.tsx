import { Link } from 'react-router-dom'
import { useAuth } from '../context/AuthContext'

export default function HomePage() {
  const { user } = useAuth()

  return (
    <div className="text-center py-16">
      <h1 className="text-4xl font-bold text-gray-900 mb-4">Welcome to ApoShop</h1>
      <p className="text-gray-500 mb-8">
        A modern e-commerce platform powered by .NET 8 microservices
      </p>
      {user ? (
        <div className="flex justify-center gap-4 flex-wrap">
          <Link
            to="/products"
            className="bg-indigo-600 text-white px-6 py-2 rounded hover:bg-indigo-700"
          >
            Browse Products
          </Link>
          <Link
            to="/cart"
            className="bg-white text-indigo-600 border border-indigo-600 px-6 py-2 rounded hover:bg-indigo-50"
          >
            View Cart
          </Link>
        </div>
      ) : (
        <div className="flex justify-center gap-4">
          <Link
            to="/login"
            className="bg-indigo-600 text-white px-6 py-2 rounded hover:bg-indigo-700"
          >
            Sign In
          </Link>
          <Link
            to="/register"
            className="bg-white text-indigo-600 border border-indigo-600 px-6 py-2 rounded hover:bg-indigo-50"
          >
            Register
          </Link>
        </div>
      )}
    </div>
  )
}
