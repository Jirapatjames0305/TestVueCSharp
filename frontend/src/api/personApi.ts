import axios from 'axios'
import type { Person, CreatePersonRequest } from '../types/person'

const http = axios.create({
  baseURL: '/api',
  headers: { 'Content-Type': 'application/json' },
})

export const personApi = {
  list: () => http.get<Person[]>('/persons').then((r) => r.data),
  get: (id: number) => http.get<Person>(`/persons/${id}`).then((r) => r.data),
  create: (req: CreatePersonRequest) =>
    http.post<Person>('/persons', req).then((r) => r.data),
}
