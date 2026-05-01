import { Link, Outlet, useNavigate } from 'react-router-dom'
import { useAuth } from '../context/AuthContext'

export default function Layout() {
  const { user, signOut } = useAuth()
  const navigate = useNavigate()

  function handleLogout() {
    signOut()
    navigate('/login')
  }

  return (
    <div className="min-h-screen bg-gray-50">
      <nav className="bg-indigo-700 text-white shadow">
        <div className="max-w-7xl mx-auto px-4 flex items-center justify-between h-14">
          <Link to="/" className="font-bold text-lg tracking-tight">
            ApoShop
          </Link>
          <div className="flex items-center gap-6 text-sm">
            {user ? (
              <>
                <Link to="/products" className="hover:text-indigo-200">Products</Link>
                <Link to="/coupons" className="hover:text-indigo-200">Coupons</Link>
                <Link to="/cart" className="hover:text-indigo-200">Cart</Link>
                <Link to="/payment" className="hover:text-indigo-200">Payment</Link>
                <Link to="/email" className="hover:text-indigo-200">Email</Link>
                <span className="text-indigo-300">Hi, {user.name || user.email}</span>
                <button
                  onClick={handleLogout}
                  className="bg-indigo-500 hover:bg-indigo-400 px-3 py-1 rounded"
                >
                  Logout
                </button>
              </>
            ) : (
              <>
                <Link to="/login" className="hover:text-indigo-200">Login</Link>
                <Link to="/register" className="hover:text-indigo-200">Register</Link>
              </>
            )}
          </div>
        </div>
      </nav>

      <main className="max-w-7xl mx-auto px-4 py-8">
        <Outlet />
      </main>
    </div>
  )
}
