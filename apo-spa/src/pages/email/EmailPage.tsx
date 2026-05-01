import { useState } from 'react'
import { useForm } from 'react-hook-form'
import { zodResolver } from '@hookform/resolvers/zod'
import { z } from 'zod'
import { useMutation, useQuery } from '@tanstack/react-query'
import { sendEmail, getEmailsByRecipient, retryEmail } from '../../api/email'
import type { EmailDto } from '../../types'

const schema = z.object({
  recipientEmail: z.string().email('Invalid email'),
  recipientName: z.string().optional(),
  subject: z.string().min(1, 'Subject is required'),
  body: z.string().min(1, 'Body is required'),
})

type FormData = z.infer<typeof schema>

function statusBadge(status: string) {
  const colours: Record<string, string> = {
    Pending: 'bg-yellow-100 text-yellow-700',
    Sent: 'bg-green-100 text-green-700',
    Failed: 'bg-red-100 text-red-700',
  }
  return (
    <span className={`px-2 py-0.5 rounded text-xs font-medium ${colours[status] ?? 'bg-gray-100'}`}>
      {status}
    </span>
  )
}

export default function EmailPage() {
  const [lookupEmail, setLookupEmail] = useState('')
  const [searchEmail, setSearchEmail] = useState('')
  const [sentEmail, setSentEmail] = useState<EmailDto | null>(null)

  const {
    register,
    handleSubmit,
    formState: { errors },
    reset,
  } = useForm<FormData>({ resolver: zodResolver(schema) })

  const sendMutation = useMutation({
    mutationFn: sendEmail,
    onSuccess: (data) => {
      setSentEmail(data)
      reset()
    },
  })

  const { data: emails = [], refetch: fetchEmails } = useQuery({
    queryKey: ['emails', 'recipient', searchEmail],
    queryFn: () => getEmailsByRecipient(searchEmail),
    enabled: false,
  })

  const retryMutation = useMutation({
    mutationFn: retryEmail,
    onSuccess: () => fetchEmails(),
  })

  return (
    <div className="space-y-10">
      {/* Send email */}
      <section>
        <h1 className="text-2xl font-bold mb-4">Send Email</h1>

        {sendMutation.error && (
          <div className="bg-red-50 border border-red-200 text-red-700 px-4 py-2 rounded mb-4 text-sm">
            {(sendMutation.error as Error).message}
          </div>
        )}

        {sentEmail && (
          <div className="bg-green-50 border border-green-200 text-green-800 px-4 py-3 rounded mb-4 text-sm space-y-1">
            <p className="font-medium">Email queued</p>
            <p>ID: <span className="font-mono">{sentEmail.emailId}</span> · Status: {statusBadge(sentEmail.status)}</p>
          </div>
        )}

        <form
          onSubmit={handleSubmit((data) => sendMutation.mutate(data))}
          className="max-w-lg space-y-4"
        >
          <div className="grid grid-cols-1 sm:grid-cols-2 gap-4">
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">
                Recipient Email
              </label>
              <input
                {...register('recipientEmail')}
                type="email"
                className="w-full border border-gray-300 rounded px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-indigo-500"
              />
              {errors.recipientEmail && (
                <p className="text-red-500 text-xs mt-1">{errors.recipientEmail.message}</p>
              )}
            </div>
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">
                Recipient Name <span className="text-gray-400">(optional)</span>
              </label>
              <input
                {...register('recipientName')}
                className="w-full border border-gray-300 rounded px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-indigo-500"
              />
            </div>
          </div>

          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">Subject</label>
            <input
              {...register('subject')}
              className="w-full border border-gray-300 rounded px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-indigo-500"
            />
            {errors.subject && (
              <p className="text-red-500 text-xs mt-1">{errors.subject.message}</p>
            )}
          </div>

          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">Body</label>
            <textarea
              {...register('body')}
              rows={5}
              className="w-full border border-gray-300 rounded px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-indigo-500"
            />
            {errors.body && (
              <p className="text-red-500 text-xs mt-1">{errors.body.message}</p>
            )}
          </div>

          <button
            type="submit"
            disabled={sendMutation.isPending}
            className="bg-indigo-600 text-white px-6 py-2 rounded hover:bg-indigo-700 disabled:opacity-60 text-sm"
          >
            {sendMutation.isPending ? 'Sending…' : 'Send Email'}
          </button>
        </form>
      </section>

      {/* Lookup by recipient */}
      <section>
        <h2 className="text-xl font-semibold mb-4">Email History by Recipient</h2>
        <div className="flex gap-2 max-w-md mb-4">
          <input
            value={lookupEmail}
            onChange={(e) => setLookupEmail(e.target.value)}
            type="email"
            placeholder="recipient@email.com"
            className="flex-1 border border-gray-300 rounded px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-indigo-500"
          />
          <button
            onClick={() => {
              setSearchEmail(lookupEmail)
              setTimeout(() => fetchEmails(), 0)
            }}
            className="bg-gray-700 text-white px-4 py-2 rounded hover:bg-gray-600 text-sm"
          >
            Search
          </button>
        </div>

        {emails.length > 0 && (
          <div className="overflow-x-auto">
            <table className="min-w-full bg-white border border-gray-200 rounded shadow-sm text-sm">
              <thead className="bg-gray-50 text-gray-600 uppercase text-xs">
                <tr>
                  <th className="px-4 py-3 text-left">ID</th>
                  <th className="px-4 py-3 text-left">Subject</th>
                  <th className="px-4 py-3 text-left">Status</th>
                  <th className="px-4 py-3 text-left">Sent At</th>
                  <th className="px-4 py-3" />
                </tr>
              </thead>
              <tbody className="divide-y divide-gray-100">
                {emails.map((e) => (
                  <tr key={e.emailId} className="hover:bg-gray-50">
                    <td className="px-4 py-3 text-gray-400">{e.emailId}</td>
                    <td className="px-4 py-3 font-medium">{e.subject}</td>
                    <td className="px-4 py-3">{statusBadge(e.status)}</td>
                    <td className="px-4 py-3 text-gray-500">
                      {e.sentAt ? new Date(e.sentAt).toLocaleString() : '—'}
                    </td>
                    <td className="px-4 py-3 text-right">
                      {e.status === 'Failed' && (
                        <button
                          onClick={() => retryMutation.mutate(e.emailId)}
                          disabled={retryMutation.isPending}
                          className="text-indigo-600 hover:underline text-xs disabled:opacity-50"
                        >
                          Retry
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
