// dd/mm/yyyy — ใช้ในตาราง/ฟอร์ม
export function formatBirthDate(iso: string): string {
  const d = new Date(iso)
  if (Number.isNaN(d.getTime())) return ''
  return d.toLocaleDateString('en-GB')
}

// yyyy.mm.dd — ใช้ใน meta strip ใต้ heading
export function formatMetaDate(d: Date = new Date()): string {
  const y = d.getFullYear()
  const m = String(d.getMonth() + 1).padStart(2, '0')
  const day = String(d.getDate()).padStart(2, '0')
  return `${y}.${m}.${day}`
}

// ปีปัจจุบัน - ปีเกิด (ตามโจทย์)
export function calcAge(isoBirthDate: string): number | null {
  if (!isoBirthDate) return null
  const birth = new Date(isoBirthDate)
  if (Number.isNaN(birth.getTime())) return null
  return new Date().getFullYear() - birth.getFullYear()
}
