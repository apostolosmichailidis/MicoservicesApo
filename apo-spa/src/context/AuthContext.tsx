import { createContext, useContext, useState, useCallback, type ReactNode } from 'react'
import { jwtDecode } from 'jwt-decode'
import type { AuthUser } from '../types'

interface JwtPayload {
  sub?: string
  email?: string
  name?: string
  'http://schemas.microsoft.com/ws/2008/06/identity/claims/role'?: string
  unique_name?: string
}

interface AuthContextValue {
  user: AuthUser | null
  token: string | null
  signIn: (token: string) => void
  signOut: () => void
}

const AuthContext = createContext<AuthContextValue | null>(null)

function parseUser(token: string): AuthUser {
  const payload = jwtDecode<JwtPayload>(token)
  return {
    id: payload.sub ?? '',
    name: payload.name ?? payload.unique_name ?? '',
    email: payload.email ?? '',
    role: payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'],
  }
}

export function AuthProvider({ children }: { children: ReactNode }) {
  const [token, setToken] = useState<string | null>(() => localStorage.getItem('token'))
  const [user, setUser] = useState<AuthUser | null>(() => {
    const stored = localStorage.getItem('token')
    return stored ? parseUser(stored) : null
  })

  const signIn = useCallback((newToken: string) => {
    localStorage.setItem('token', newToken)
    setToken(newToken)
    setUser(parseUser(newToken))
  }, [])

  const signOut = useCallback(() => {
    localStorage.removeItem('token')
    setToken(null)
    setUser(null)
  }, [])

  return (
    <AuthContext.Provider value={{ user, token, signIn, signOut }}>
      {children}
    </AuthContext.Provider>
  )
}

export function useAuth() {
  const ctx = useContext(AuthContext)
  if (!ctx) throw new Error('useAuth must be used inside AuthProvider')
  return ctx
}
