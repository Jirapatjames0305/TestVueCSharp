# Flow: ระบบจัดการข้อมูลบุคคล (IT 01)

## Tech Stack

| ส่วน | เทคโนโลยี | เวอร์ชัน |
|------|----------|---------|
| Frontend | Vue | 3.5 (Composition API + `<script setup>`) |
| Build tool | Vite | 7.x (Node 20.13.1 ไม่รองรับ Vite 8 — ต้องใช้ 7.x) |
| Language (FE) | TypeScript | 6.x |
| CSS | **Tailwind CSS** (ตัวเดียว) | 4.x (`@theme` directive — ไม่มี tailwind.config.js) |
| Component lib | **[Reka UI](https://reka-ui.com)** (ตัวเดียว) — headless, ครอบ a11y/focus trap/ESC | 2.x |
| HTTP client | Axios | latest |
| Backend | .NET | 9 |
| API style | ASP.NET Core **MVC (Controller-based Web API)** + EF Core 9 | latest |
| Database | PostgreSQL (Supabase) | 15+ |
| ORM | Entity Framework Core + Npgsql provider | 9.x |

---

## Design Direction — Formal Enterprise Admin

> **Aesthetic:** Admin dashboard ทางการ — สะอาด อ่านง่าย ดูน่าเชื่อถือ

### Color Palette

```css
--color-primary:       #1d4ed8   /* Blue 700 — primary action, navbar */
--color-primary-h:     #1e3a8a   /* Blue 900 — hover */
--color-primary-light: #eff6ff   /* Blue 50  — row hover, age badge bg */
--color-surface:       #ffffff   /* card / modal background */
--color-bg:            #f1f5f9   /* page background */
--color-border:        #e2e8f0   /* dividers, input borders */
--color-ink:           #0f172a   /* main text */
--color-sub:           #334155   /* secondary text */
--color-muted:         #64748b   /* hint, label, placeholder */
--color-danger:        #dc2626   /* error */
--color-danger-light:  #fef2f2   /* error background */
```

### Typography

| ใช้กับ | Font |
|--------|------|
| Body / UI | **Geist Sans** (via CDN jsDelivr) |
| ตัวเลข / ID / date / age | **JetBrains Mono** (Google Fonts) |

### Animations (keyframes ใน `@theme`)

| ชื่อ | ใช้กับ |
|------|--------|
| `fade-in` | table rows (staggered), dialog overlay |
| `slide-up` | modal บน desktop |
| `slide-in-bottom` | modal บน mobile (bottom sheet) |

> หมายเหตุ: animation class ต้องใส่ fill-mode `both` เสมอ — `animate-[fade-in_150ms_ease-out_both]`
> มิฉะนั้น `opacity` จะกลับเป็น 0 เมื่อ animation จบ (ถ้ามี inline `opacity: 0` สำหรับ stagger delay)

---

## หน้าจอ (UI)

ระบบมี 1 หน้าหลัก และ 2 Modal

### IT 01-1 — หน้ารายการ (List Page)
- Blue sticky navbar + icon
- Page heading card: ชื่อหัวข้อ, จำนวน record, ปุ่ม **เพิ่มข้อมูล**
- ตารางแสดงข้อมูล คอลัมน์: `ลำดับ`, `ชื่อ-นามสกุล`, `ที่อยู่`, `วันเกิด`, `อายุ`, `จัดการ`
- Loading skeleton ระหว่างโหลด
- Mobile: card layout แทน table
- ปุ่ม `ดูข้อมูล` ในแต่ละแถว → เปิด Modal IT 01-3

### IT 01-2 — Modal เพิ่มข้อมูล (Add)
- Bottom sheet บน mobile, centered dialog บน desktop
- ฟิลด์: `ชื่อ`*, `นามสกุล`*, `วันเกิด`*, `อายุ` (auto-calculated), `ที่อยู่`
- ปุ่ม `บันทึก` → POST → ปิด modal → reload ตาราง
- ปุ่ม `ยกเลิก` → ปิด modal โดยไม่บันทึก
- แสดง error message ถ้า API ตอบ error

### IT 01-3 — Modal ดูข้อมูล (View, Read-only)
- ฟิลด์เหมือน IT 01-2 แต่ read-only ทั้งหมด (แสดงผ่าน `<div>` ไม่ใช่ `<input>`)
- ปุ่ม `ปิด` เพื่อปิด modal

---

## Database Schema (PostgreSQL)

ตาราง `persons`

| Column | Type | Constraint |
|--------|------|-----------|
| `id` | `bigserial` | PK |
| `first_name` | `varchar(100)` | NOT NULL |
| `last_name` | `varchar(100)` | NOT NULL |
| `birth_date` | `date` | NOT NULL |
| `address` | `varchar(500)` | NULL |
| `created_at` | `timestamptz` | DEFAULT `now()` |

```sql
create table public.persons (
  id           bigserial primary key,
  first_name   varchar(100) not null,
  last_name    varchar(100) not null,
  birth_date   date         not null,
  address      varchar(500),
  created_at   timestamptz  not null default now()
);
```

> `Age` ไม่เก็บใน DB — คำนวณตอนส่งกลับ (`TodayInBangkok().Year - BirthDate.Year`)

---

## API Spec (.NET 9 MVC)

Base URL: `/api/persons`

| Method | Endpoint | Action | Response |
|--------|----------|--------|----------|
| `GET` | `/api/persons` | `GetAll` | `200` `PersonDto[]` เรียงตาม `id` ASC |
| `GET` | `/api/persons/{id}` | `GetById` | `200` `PersonDto` / `404` |
| `POST` | `/api/persons` | `Create` | `201` `PersonDto` + `Location` header |

JSON response เป็น **camelCase** ทั้งหมด (configured ใน `AddJsonOptions`)

### Program.cs

```csharp
using Microsoft.EntityFrameworkCore;
using PersonApi.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(o =>
        o.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase);

builder.Services.AddOpenApi();

builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddCors(o => o.AddDefaultPolicy(p => p
    .WithOrigins("http://localhost:5173")
    .AllowAnyHeader()
    .AllowAnyMethod()));

var app = builder.Build();

if (app.Environment.IsDevelopment()) { app.MapOpenApi(); }

app.UseCors();
app.MapControllers();
app.Run();
```

### DTOs

```csharp
// Request — DataAnnotations บน constructor parameter (ไม่ใช่ [property:...])
public record CreatePersonRequest(
    [Required, StringLength(100, MinimumLength = 1)] string FirstName,
    [Required, StringLength(100, MinimumLength = 1)] string LastName,
    [Required] DateOnly BirthDate,
    [StringLength(500)] string? Address
);

// Response
public record PersonDto(long Id, string FirstName, string LastName, DateOnly BirthDate, int Age, string? Address);
```

> หมายเหตุ: .NET 9 ไม่รองรับ `[property: Required]` บน record — ต้องเขียนโดยตรงบน constructor parameter

### อายุ + timezone

```csharp
private static DateOnly TodayInBangkok()
{
    var bkk = TimeZoneInfo.FindSystemTimeZoneById("Asia/Bangkok");
    return DateOnly.FromDateTime(TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, bkk));
}
```

---

## โครงสร้างโปรเจกต์

```
TestVueCSharp/
├── db/
│   └── 001_create_persons.sql
├── backend/
│   └── PersonApi/
│       ├── PersonApi.csproj
│       ├── Program.cs
│       ├── appsettings.json           # connection string (DefaultConnection)
│       ├── Properties/
│       │   └── launchSettings.json    # port 5050 (หลีกเลี่ยง macOS AirPlay ที่ใช้ port 5000)
│       ├── Controllers/
│       │   └── PersonsController.cs
│       ├── Data/
│       │   ├── AppDbContext.cs        # snake_case column mapping
│       │   └── Person.cs
│       └── Dtos/
│           ├── CreatePersonRequest.cs
│           └── PersonDto.cs
└── frontend/
    ├── package.json
    ├── vite.config.ts                 # proxy /api → localhost:5050
    ├── index.html                     # Google Fonts + Geist CDN
    └── src/
        ├── main.ts
        ├── App.vue
        ├── style.css                  # @theme tokens
        ├── api/
        │   └── personApi.ts
        ├── types/
        │   └── person.ts
        ├── lib/
        │   └── format.ts              # formatBirthDate, calcAge
        ├── components/
        │   ├── Field.vue
        │   ├── PersonTable.vue        # IT 01-1
        │   ├── PersonAddModal.vue     # IT 01-2
        │   └── PersonViewModal.vue    # IT 01-3
        └── views/
            └── PersonListView.vue
```

---

## Frontend Setup

### 1) สร้างโปรเจกต์

```bash
npm create vite@latest frontend -- --template vue-ts
cd frontend
npm install
npm install tailwindcss @tailwindcss/vite
npm install reka-ui
npm install axios
```

> Vite 8 ต้องการ Node 20.19+ — ถ้าใช้ Node 20.13.x ให้ downgrade: `npm install vite@^7`

### 2) `vite.config.ts`

```ts
import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'
import tailwindcss from '@tailwindcss/vite'

export default defineConfig({
  plugins: [vue(), tailwindcss()],
  server: {
    port: 5173,
    proxy: {
      '/api': {
        target: 'http://localhost:5050',
        changeOrigin: true,
      },
    },
  },
})
```

### 3) `src/style.css`

```css
@import "tailwindcss";

@theme {
  --color-primary:       #1d4ed8;
  --color-primary-h:     #1e3a8a;
  --color-primary-light: #eff6ff;
  --color-surface:       #ffffff;
  --color-bg:            #f1f5f9;
  --color-border:        #e2e8f0;
  --color-ink:           #0f172a;
  --color-sub:           #334155;
  --color-muted:         #64748b;
  --color-danger:        #dc2626;
  --color-danger-light:  #fef2f2;

  --font-sans: "Geist", system-ui, sans-serif;
  --font-mono: "JetBrains Mono", ui-monospace, monospace;

  --animate-fade-in:         fade-in 150ms ease-out forwards;
  --animate-slide-up:        slide-up 200ms cubic-bezier(0.16,1,0.3,1) forwards;
  --animate-slide-in-bottom: slide-in-bottom 260ms cubic-bezier(0.16,1,0.3,1) forwards;
}
```

### 4) Date helpers (`src/lib/format.ts`)

```ts
export const formatBirthDate = (iso: string) =>
  new Date(iso).toLocaleDateString('en-GB')   // 11/05/1990

export const calcAge = (iso: string): number | null => {
  if (!iso) return null
  return new Date().getFullYear() - new Date(iso).getFullYear()
}
```

### 5) Components จาก Reka UI

```ts
import {
  DialogRoot, DialogPortal, DialogOverlay,
  DialogContent, DialogTitle, DialogClose,
} from 'reka-ui'
```

---

## NuGet Packages

```xml
<PackageReference Include="Npgsql.EntityFrameworkCore.PostgreSQL" Version="9.0.4" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="9.0.0" />
<PackageReference Include="Microsoft.AspNetCore.OpenApi" Version="9.0.x" />
```

> Swashbuckle 10.x ใช้ไม่ได้กับ .NET 9 — ใช้ built-in `AddOpenApi()` + `MapOpenApi()` แทน

---

## Supabase Setup

### Connection string

ใช้ **Session pooler (port 5432)** — เสถียรกว่าสำหรับ EF Core

> Transaction pooler (port 6543) มีปัญหา timeout ระหว่างรอผล `INSERT ... RETURNING id` ของ EF Core ทำให้ได้ 500 แม้ข้อมูลลงสำเร็จ

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=aws-1-xxx.pooler.supabase.com;Port=5432;Database=postgres;Username=postgres.xxxx;Password=YOUR_PASSWORD;SSL Mode=Require;Trust Server Certificate=true"
  }
}
```

ใส่ใน `appsettings.json` (key ต้องเป็น `DefaultConnection` ให้ตรงกับ `GetConnectionString("DefaultConnection")` ใน Program.cs)

### Entity snake_case mapping

```csharp
modelBuilder.Entity<Person>(e =>
{
    e.ToTable("persons");
    e.Property(p => p.Id).HasColumnName("id");
    e.Property(p => p.FirstName).HasColumnName("first_name");
    e.Property(p => p.LastName).HasColumnName("last_name");
    e.Property(p => p.BirthDate).HasColumnName("birth_date");
    e.Property(p => p.Address).HasColumnName("address");
    e.Property(p => p.CreatedAt).HasColumnName("created_at");
});
```

---

## Dev Setup

| Service | Port | หมายเหตุ |
|---------|------|---------|
| Frontend (Vite) | 5173 | `npm run dev` |
| Backend (.NET) | 5050 | `dotnet run` — port 5000 ถูก macOS AirPlay ใช้อยู่ |

`launchSettings.json`:
```json
{
  "profiles": {
    "http": {
      "commandName": "Project",
      "applicationUrl": "http://localhost:5050",
      "environmentVariables": { "ASPNETCORE_ENVIRONMENT": "Development" }
    }
  }
}
```

---

## การ Run โปรเจกต์

> ต้องรัน **2 terminal พร้อมกัน** — backend และ frontend แยกกัน

### Terminal 1 — Backend (.NET)

```bash
cd backend/PersonApi
dotnet run
```

รอจนเห็น:
```
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5050
```

ทดสอบว่า API ทำงาน:
```bash
curl http://localhost:5050/api/persons
```
ต้องได้ JSON array กลับมา (ไม่ใช่ 403 หรือ error)

---

### Terminal 2 — Frontend (Vue + Vite)

```bash
cd frontend
npm run dev
```

รอจนเห็น:
```
  VITE v7.x  ready in xxx ms
  ➜  Local:   http://localhost:5173/
```

เปิดเบราว์เซอร์ไปที่ **http://localhost:5173**

---

### ลำดับการรันที่แนะนำ

1. รัน **backend ก่อน** — frontend อ่าน API ตอน load หน้า ถ้า backend ยังไม่พร้อมจะเห็น error state
2. รัน frontend
3. เปิดเบราว์เซอร์

---

### Troubleshooting

| อาการ | สาเหตุ | แก้ไข |
|-------|--------|-------|
| `403 Forbidden` จาก curl port 5050 | port ถูกใช้อยู่แล้ว | `lsof -ti :5050 \| xargs kill -9` แล้ว `dotnet run` ใหม่ |
| ข้อมูลไม่ขึ้นใน table | backend ไม่ได้รัน | ตรวจสอบ Terminal 1 |
| `CORS error` ใน console | backend รันที่ port อื่น | ตรวจสอบ `launchSettings.json` ว่า port คือ 5050 |
| `Cannot connect to DB` | connection string ผิด หรือ Supabase project paused | เข้า Supabase dashboard → resume project |
| `address already in use: 5000` | macOS AirPlay Receiver ใช้ port 5000 | ใช้ port 5050 เท่านั้น (ตั้งไว้แล้วใน launchSettings.json) |

---

## Flow รายละเอียด

### Flow 1: เพิ่มข้อมูล (Add)

```
User → กด "เพิ่มข้อมูล" (IT 01-1)
     → เปิด Modal IT 01-2
User → กรอก ชื่อ, นามสกุล, วันเกิด, ที่อยู่
     → FE คำนวณ "อายุ" อัตโนมัติเมื่อเลือกวันเกิด
User → กด "บันทึก"
FE   → validate (required fields)
     → POST /api/persons { firstName, lastName, birthDate, address }
BE   → validate BirthDate (ต้องไม่อยู่ในอนาคต)
     → insert DB → return PersonDto (camelCase JSON)
FE   → emit('created') → reload GET /api/persons
     → emit('update:open', false) → ปิด modal

หรือ User กด "ยกเลิก" → ปิด modal โดยไม่เรียก API
```

### Flow 2: ดูข้อมูล (View)

```
User → กด "ดูข้อมูล" ที่แถวใดแถวหนึ่ง
FE   → ใช้ข้อมูลที่มีอยู่ใน state (ไม่ต้อง GET ซ้ำ)
     → เปิด Modal IT 01-3 พร้อม bind ข้อมูล (readonly)
User → กด "ปิด" → ปิด modal
```

### การคำนวณอายุ
- สูตร: `ปีปัจจุบัน − ปีเกิด`
- FE: แสดงใน modal ขณะกรอกวันเกิด (`calcAge` ใน `format.ts`)
- BE: ส่งกลับใน `PersonDto.age` (คำนวณเทียบกับ timezone Asia/Bangkok)

---
