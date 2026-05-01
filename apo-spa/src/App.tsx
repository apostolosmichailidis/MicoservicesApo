import { BrowserRouter, Routes, Route } from 'react-router-dom'
import { AuthProvider } from './context/AuthContext'
import Layout from './components/Layout'
import PrivateRoute from './components/PrivateRoute'
import HomePage from './pages/HomePage'
import LoginPage from './pages/auth/LoginPage'
import RegisterPage from './pages/auth/RegisterPage'
import ProductsPage from './pages/products/ProductsPage'
import ProductCreatePage from './pages/products/ProductCreatePage'
import ProductEditPage from './pages/products/ProductEditPage'
import CouponsPage from './pages/coupons/CouponsPage'
import CouponCreatePage from './pages/coupons/CouponCreatePage'
import CartPage from './pages/cart/CartPage'
import PaymentPage from './pages/payment/PaymentPage'
import EmailPage from './pages/email/EmailPage'

export default function App() {
  return (
    <AuthProvider>
      <BrowserRouter>
        <Routes>
          <Route element={<Layout />}>
            <Route path="/" element={<HomePage />} />
            <Route path="/login" element={<LoginPage />} />
            <Route path="/register" element={<RegisterPage />} />

            {/* Protected routes */}
            <Route element={<PrivateRoute />}>
              <Route path="/products" element={<ProductsPage />} />
              <Route path="/products/create" element={<ProductCreatePage />} />
              <Route path="/products/edit/:id" element={<ProductEditPage />} />
              <Route path="/coupons" element={<CouponsPage />} />
              <Route path="/coupons/create" element={<CouponCreatePage />} />
              <Route path="/cart" element={<CartPage />} />
              <Route path="/payment" element={<PaymentPage />} />
              <Route path="/email" element={<EmailPage />} />
            </Route>
          </Route>
        </Routes>
      </BrowserRouter>
    </AuthProvider>
  )
}
