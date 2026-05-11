export interface Person {
  id: number
  firstName: string
  lastName: string
  birthDate: string // ISO yyyy-mm-dd
  age: number
  address: string | null
}

export interface CreatePersonRequest {
  firstName: string
  lastName: string
  birthDate: string // yyyy-mm-dd
  address: string | null
}
