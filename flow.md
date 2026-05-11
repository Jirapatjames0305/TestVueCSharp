# Flow: ระบบจัดการข้อมูลบุคคล (IT 01)

## Tech Stack

| ส่วน | เทคโนโลยี | เวอร์ชัน |
|------|----------|---------|
| Frontend | Vue | 3.5 (Composition API + `<script setup>`) |
| Build tool | Vite | 6.x |
| Language (FE) | TypeScript | 5.x |
| CSS | **Tailwind CSS** (ตัวเดียว) | 4.x |
| Component lib | **[Reka UI](https://reka-ui.com)** (ตัวเดียว) — headless, ครอบ a11y/focus trap/ESC ให้, style เองด้วย Tailwind 100% | 1.x |
| HTTP client | Axios | latest |
| State (ถ้าจำเป็น) | Pinia | 2.x |
| Backend | .NET | 9 (LTS) |
| API style | ASP.NET Core **MVC (Controller-based Web API)** + EF Core 9 | latest |
| Database | PostgreSQL (Supabase) | 15+ |
| ORM | Entity Framework Core + Npgsql provider | 9.x |

---

## Design Direction — "Editorial Brutalism"

> **Aesthetic ที่เลือก:** Editorial minimalism + brutalist accents — ไม่ใช่ admin dashboard generic ที่ใช้ Inter + soft shadow ทั่วไป
>
> **Mood:** กระดาษหนา, ตู้บัตรห้องสมุด, ฉลากของจริง — เน้น typography และ whitespace มากกว่า decoration

### Typography (โหลดผ่าน `<link>` หรือ self-host)

| ใช้กับ | Font | Weight | หมายเหตุ |
|--------|------|--------|---------|
| Display / headings | **Instrument Serif** | 400 (regular เท่านั้น) | italic ได้ — ใช้กับชื่อหน้า "IT 01" |
| Body / form labels | **Geist Sans** | 400 / 500 | อ่านง่ายขึ้นกว่า Inter |
| ตัวเลข / ID / date / age | **JetBrains Mono** | 500 | สำคัญ — ทำให้ data รู้สึก "แม่นยำ" |

ห้ามใช้: Inter (เกร่อ), Poppins, Roboto, Open Sans, system-ui default

### Color Palette (single accent, high contrast)

```
--ink:        #0A0A0A   /* near-black text & borders */
--paper:      #F5F1EA   /* warm off-white background — NOT pure white */
--paper-2:    #ECE7DD   /* row hover / subtle blocks */
--rule:       #1A1A1A   /* divider lines (1px hard) */
--accent:     #E0421C   /* deep vermillion — primary action */
--accent-ink: #FAFAFA   /* text on accent */
--muted:      #6B6357   /* secondary labels, hint text */
--ok:         #1F6B3A   /* save confirmation */
--danger:     #8B1A1A   /* cancel / destructive */
```

ห้าม: gradient ทุกชนิด, glassmorphism, neumorphism, soft drop shadow, Bootstrap blue (#0d6efd)

### Spatial & Visual Language

- **Hard edges** — `border-radius: 0` เป็นค่า default, ยกเว้น avatar/badge เล็ก ๆ
- **1px hard borders** สีเข้ม (`--rule`) แทน shadow
- **Generous whitespace** — padding ของ card/modal อย่างน้อย 48px
- **Asymmetric layout** — ปุ่ม ADD ไม่อยู่ตรงกลาง, ใช้ flex justify-end กับ extra negative margin
- **Numbered labels** — ใช้ตัวเลขโรมัน หรือ "01 — ", "02 — " กับ section headings
- **Tabular numbers** (`font-variant-numeric: tabular-nums`) ในตาราง

### Motion (เล็กแต่ตั้งใจ)

- transition ทั้งหมด `180ms cubic-bezier(0.2, 0.8, 0.2, 1)` (snappy ease-out)
- Modal open: fade + translateY(8px → 0), ไม่ใช้ scale
- Button hover: invert สี (bg ↔ text) ทันทีในเวลา 100ms
- Row hover ในตาราง: เปลี่ยน background เป็น `--paper-2`, **ไม่มี** transform
- ห้ามใช้: bounce, spring, parallax, animated gradient, loading spinner — ใช้ skeleton หรือ progress bar เส้นบางแทน

### Component แนวทาง

**ตาราง (IT 01-1):**
- ไม่มี cell border แนวตั้ง — เฉพาะ horizontal rule 1px
- Header row: `text-transform: uppercase`, `letter-spacing: 0.08em`, font-size 11px, weight 500
- Body rows: font-size 15px, line-height 1.6, padding-y 18px
- Column "Id" align right, JetBrains Mono
- Column "อายุ" align right, JetBrains Mono + suffix `ปี` สี muted
- ปุ่ม View: text-only link สีดำ underline offset 4px (ไม่ใช่ button ที่มีพื้น)
- ปุ่ม ADD บนขวา: solid bg `--ink`, text `--paper`, padding-x 28px, padding-y 14px, ตัวพิมพ์ใหญ่ + letter-spacing — กดแล้ว invert

**Modal (IT 01-2 / 01-3):**
- Backdrop: `--ink` ที่ opacity 0.85, **ไม่ blur**
- Modal panel: bg `--paper`, border 1px `--rule`, width 640px, padding 56px
- Header: เลขลำดับ "01 — เพิ่มข้อมูล" Instrument Serif italic 36px
- Form labels อยู่ **ด้านบน** input (ไม่ใช่ inline left ตามรูปต้นแบบ) เพื่อความคม
- Input: ไม่มี border-radius, border-bottom 1px `--ink` เท่านั้น (no box), focus → border-bottom 2px `--accent`
- Date picker: ใช้ `<DatePicker>` ของ Reka UI + style เองด้วย Tailwind (ห้าม Bootstrap datepicker default)
- ปุ่ม `บันทึก`: bg `--accent`, text `--accent-ink`
- ปุ่ม `ยกเลิก`: text-only `--muted`, ไม่มี border
- ปุ่ม `ปิด` (IT 01-3): bg `--ink`, text `--paper`

### Atmospheric Details (เล็กแต่ทำให้จำได้)

- มุมบนซ้ายของหน้า: meta strip — "TestVueCSharp / Persons / 01" ใน JetBrains Mono 11px สี `--muted`
- ใต้ heading หลัก: เส้น `--rule` ยาวเต็มความกว้าง + วันที่ปัจจุบันด้านขวาในรูปแบบ `2026.05.11` (mono)
- Empty state ของตาราง: "— ยังไม่มีข้อมูล —" italic serif สี `--muted` กึ่งกลาง (ไม่ใช่ illustration)
- Footer ของ modal: เส้น rule บาง + caption "ESC เพื่อปิด" ใน mono 11px มุมล่างซ้าย

### Anti-pattern checklist (ห้ามทำ)

- ❌ Tailwind default colors (`bg-blue-500`, `bg-gray-100`) ใช้ตรง ๆ
- ❌ Material/Bootstrap default components ที่ไม่ override
- ❌ Drop shadow `0 4px 6px rgba(0,0,0,0.1)` style
- ❌ Border radius 8px, 12px ทั่วทั้ง app
- ❌ Spinner กลม animated, skeleton แบบ shimmer gradient
- ❌ Emoji ใน UI (เว้นแต่ user กรอกเอง)
- ❌ Toast popup สี success/error pastel

---

## หน้าจอ (UI)

ระบบมี 1 หน้าหลัก และ 2 Modal

### IT 01-1 — หน้ารายการ (List Page)
- ตารางแสดงข้อมูล มีคอลัมน์:
  - `Id`
  - `ชื่อ-สกุล` (รวมชื่อ + นามสกุล)
  - `ที่อยู่`
  - `วันเกิด` (รูปแบบ `dd/mm/yyyy`)
  - `อายุ`
  - `Action` (ปุ่ม `View`)
- ปุ่ม `ADD` ที่มุมขวาบน → เปิด Modal IT 01-2
- ปุ่ม `View` ในแต่ละแถว → เปิด Modal IT 01-3 พร้อมข้อมูลของแถวนั้น

### IT 01-2 — Modal เพิ่มข้อมูล (Add)
- ฟิลด์:
  - `ชื่อ` (TextField)
  - `นามสกุล` (TextField)
  - `วันเกิด` (DatePicker)
  - `อายุ` (แสดงผลอย่างเดียว — คำนวณจาก `ปีปัจจุบัน − ปีเกิด`)
  - `ที่อยู่` (Textarea)
- ปุ่ม:
  - `บันทึก` → POST ข้อมูลไป API → ปิด modal → refresh ตาราง (แถวใหม่อยู่ท้ายตาราง)
  - `ยกเลิก` → ปิด modal โดยไม่บันทึก

### IT 01-3 — Modal ดูข้อมูล (View, Read-only)
- ฟิลด์เหมือน IT 01-2 แต่ `disabled / readonly` ทั้งหมด
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

SQL สร้างตาราง (รันใน Supabase SQL Editor):

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

> หมายเหตุ: `Age` **ไม่เก็บใน DB** — คำนวณตอนส่งกลับ (`currentYear - birthYear`) เพื่อไม่ให้ข้อมูลไม่ตรงกับวันปัจจุบัน

---

## API Spec (.NET 9 MVC — Controller-based Web API)

Base URL: `/api/persons` (กำหนดด้วย `[Route("api/[controller]")]` บน `PersonsController`)

| Method | Endpoint | Action | Description | Body / Query | Response |
|--------|----------|--------|-------------|--------------|----------|
| `GET`  | `/api/persons` | `GetAll` | ดึงรายการทั้งหมด (เรียง `Id` ASC เพื่อให้แถวใหม่อยู่ท้าย) | – | `200` `PersonDto[]` |
| `GET`  | `/api/persons/{id}` | `GetById` | ดึงข้อมูลตาม Id | – | `200` `PersonDto` / `404` |
| `POST` | `/api/persons` | `Create` | เพิ่มข้อมูลใหม่ | `CreatePersonRequest` | `201` `PersonDto` (พร้อม `Location` header) |

### Controller skeleton

```csharp
[ApiController]
[Route("api/[controller]")]
public class PersonsController : ControllerBase
{
    private readonly AppDbContext _db;
    public PersonsController(AppDbContext db) => _db = db;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PersonDto>>> GetAll()
    {
        var items = await _db.Persons
            .OrderBy(p => p.Id)
            .Select(p => ToDto(p))
            .ToListAsync();
        return Ok(items);
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<PersonDto>> GetById(long id)
    {
        var person = await _db.Persons.FindAsync(id);
        return person is null ? NotFound() : Ok(ToDto(person));
    }

    [HttpPost]
    public async Task<ActionResult<PersonDto>> Create([FromBody] CreatePersonRequest req)
    {
        // ModelState validation จะรันอัตโนมัติเพราะมี [ApiController]
        var entity = new Person
        {
            FirstName = req.FirstName.Trim(),
            LastName  = req.LastName.Trim(),
            BirthDate = req.BirthDate,
            Address   = req.Address?.Trim()
        };
        _db.Persons.Add(entity);
        await _db.SaveChangesAsync();

        var dto = ToDto(entity);
        return CreatedAtAction(nameof(GetById), new { id = entity.Id }, dto);
    }

    private static PersonDto ToDto(Person p)
    {
        // ใช้เวลาในไทย (UTC+7) เพื่อเลี่ยงความคลาดเคลื่อนตอนเที่ยงคืนของ UTC
        var bkk = TimeZoneInfo.FindSystemTimeZoneById("Asia/Bangkok");
        var todayTh = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, bkk);
        var age = todayTh.Year - p.BirthDate.Year;
        return new PersonDto(p.Id, p.FirstName, p.LastName, p.BirthDate, age, p.Address);
    }
}
```

### Program.cs (จุดสำคัญ)

```csharp
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddCors(o => o.AddDefaultPolicy(p => p
    .WithOrigins("http://localhost:5173")
    .AllowAnyHeader()
    .AllowAnyMethod()));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();
app.MapControllers();   // ← MVC routing
app.Run();
```

### DTO + Validation

`[ApiController]` จะตอบ `400 ValidationProblem` อัตโนมัติเมื่อ ModelState ไม่ผ่าน (อิงจาก DataAnnotations ด้านล่าง)

```csharp
// Request — มี DataAnnotations
public record CreatePersonRequest(
    [Required, StringLength(100, MinimumLength = 1)] string FirstName,
    [Required, StringLength(100, MinimumLength = 1)] string LastName,
    [Required] DateOnly BirthDate,
    [StringLength(500)] string? Address
);

// Response
public record PersonDto(
    long Id,
    string FirstName,
    string LastName,
    DateOnly BirthDate,
    int Age,
    string? Address
);
```

กฎ validation:
- `FirstName`, `LastName`: required, length 1–100
- `BirthDate`: required (ถ้าต้องการ check ว่า `<=` วันนี้ ทำ custom validation หรือเช็คใน controller)
- `Address`: optional, length ≤ 500

---

## Flow รายละเอียด

### Flow 1: เพิ่มข้อมูล (Add)

```
User → กด ADD (IT 01-1)
     → เปิด Modal IT 01-2
User → กรอก ชื่อ, นามสกุล, วันเกิด, ที่อยู่
     → FE คำนวณ "อายุ" อัตโนมัติเมื่อเลือกวันเกิด (currentYear - birthYear)
User → กด "บันทึก"
FE   → validate ฟอร์ม (required fields)
     → POST /api/persons ด้วย CreatePersonRequest
BE   → validate → insert DB → return PersonDto
FE   → ปิด modal
     → reload GET /api/persons → render ตารางใหม่ (แถวใหม่ท้ายสุด)

หรือ User กด "ยกเลิก" → ปิด modal โดยไม่เรียก API
```

### Flow 2: ดูข้อมูล (View)

```
User → กด "View" ที่แถวใดแถวหนึ่ง (IT 01-1)
FE   → ใช้ข้อมูลที่มีอยู่ใน state (ไม่ต้องเรียก API ซ้ำ)
       หรือเรียก GET /api/persons/{id} ก็ได้
     → เปิด Modal IT 01-3 พร้อม bind ข้อมูล (readonly)
User → กด "ปิด" → ปิด modal
```

### การคำนวณอายุ
- สูตรตามโจทย์: `อายุ = ปีปัจจุบัน − ปีเกิด`
- ทำที่ **ทั้งฝั่ง FE** (แสดงใน modal ตอนเลือกวันเกิด) **และฝั่ง BE** (ใส่ใน `PersonDto.Age` ก่อนส่งกลับ) — แหล่งความจริงคือ BE

---

## โครงสร้างโปรเจกต์ (แนะนำ)

```
TestVueCSharp/
├── backend/                       # .NET 9 solution
│   ├── PersonApi.sln
│   └── PersonApi/
│       ├── Program.cs             # AddControllers + DI + EF Core (Npgsql) + CORS
│       ├── Controllers/
│       │   └── PersonsController.cs   # MVC controller, [ApiController]
│       ├── Data/
│       │   ├── AppDbContext.cs    # ใช้ UseNpgsql
│       │   └── Person.cs          # entity + snake_case column mapping
│       ├── Dtos/
│       │   ├── CreatePersonRequest.cs   # มี DataAnnotations
│       │   └── PersonDto.cs
│       └── appsettings.json       # connection string ไป Supabase
│
└── frontend/                      # Vue 3.5 + Vite + TS
    ├── package.json
    ├── vite.config.ts
    ├── index.html
    └── src/
        ├── main.ts
        ├── App.vue
        ├── api/
        │   └── personApi.ts       # wrap axios/fetch
        ├── types/
        │   └── person.ts
        ├── components/
        │   ├── PersonTable.vue    # IT 01-1
        │   ├── PersonAddModal.vue # IT 01-2
        │   └── PersonViewModal.vue# IT 01-3
        └── views/
            └── PersonListView.vue
```

---

## Frontend Setup (Tailwind 4 + Reka UI + Fonts)

### 1) สร้างโปรเจกต์

```bash
npm create vite@latest frontend -- --template vue-ts
cd frontend
npm install
npm install tailwindcss @tailwindcss/vite
npm install reka-ui
npm install axios
```

### 2) `vite.config.ts`

```ts
import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'
import tailwindcss from '@tailwindcss/vite'

export default defineConfig({
  plugins: [vue(), tailwindcss()],
})
```

### 3) โหลด Font (Google Fonts)

ใน `index.html` เพิ่มใน `<head>`:

```html
<link rel="preconnect" href="https://fonts.googleapis.com">
<link rel="preconnect" href="https://fonts.gstatic.com" crossorigin>
<link rel="stylesheet"
  href="https://fonts.googleapis.com/css2?family=Instrument+Serif:ital@0;1&family=JetBrains+Mono:wght@500&display=swap">
<link rel="stylesheet"
  href="https://cdn.jsdelivr.net/npm/geist@1/dist/fonts/geist-sans/style.css">
```

> หมายเหตุ: Geist Sans ไม่อยู่บน Google Fonts → โหลดผ่าน CDN (npm package `geist`) หรือ self-host จาก [vercel/geist-font](https://github.com/vercel/geist-font)

### 4) `src/style.css` — Tailwind 4 `@theme` mapping design tokens เป็น utilities

```css
@import "tailwindcss";

@theme {
  /* Colors → ใช้เป็น bg-ink, text-paper, border-rule, bg-accent ฯลฯ */
  --color-ink:        #0A0A0A;
  --color-paper:      #F5F1EA;
  --color-paper-2:    #ECE7DD;
  --color-rule:       #1A1A1A;
  --color-accent:     #E0421C;
  --color-accent-ink: #FAFAFA;
  --color-muted:      #6B6357;
  --color-ok:         #1F6B3A;
  --color-danger:     #8B1A1A;

  /* Fonts → ใช้เป็น font-display, font-sans, font-mono */
  --font-display: "Instrument Serif", serif;
  --font-sans:    "Geist", system-ui, sans-serif;
  --font-mono:    "JetBrains Mono", ui-monospace, monospace;

  /* Motion */
  --ease-snap: cubic-bezier(0.2, 0.8, 0.2, 1);
}

/* Global */
html, body {
  background: var(--color-paper);
  color: var(--color-ink);
  font-family: var(--font-sans);
  font-feature-settings: "ss01";
}

/* Tabular numbers สำหรับตาราง / ID / age */
.tabular { font-variant-numeric: tabular-nums; }
```

### 5) ตัวอย่างการใช้ใน component

```vue
<button class="bg-ink text-paper px-7 py-3.5 uppercase tracking-wider
               hover:bg-accent transition-colors duration-100">
  ADD
</button>

<h1 class="font-display italic text-4xl">01 — เพิ่มข้อมูล</h1>

<td class="font-mono tabular text-right">{{ person.id }}</td>
```

### 6) Date display formats

ใช้ 2 รูปแบบในที่ต่างกัน — ห้ามผสม:
- **ในตาราง / form** (วันเกิด): `dd/mm/yyyy` → เช่น `11/05/2026`
- **ใน meta strip / atmospheric** (วันที่ปัจจุบันใต้ heading): `yyyy.mm.dd` → เช่น `2026.05.11`

แนะนำใช้ helper สั้น ๆ (ไม่ต้อง install date-fns) — `Intl.DateTimeFormat` ทำได้ทั้งคู่:

```ts
export const formatBirthDate = (iso: string) =>
  new Date(iso).toLocaleDateString('en-GB') // 11/05/2026

export const formatMetaDate = (d = new Date()) =>
  `${d.getFullYear()}.${String(d.getMonth()+1).padStart(2,'0')}.${String(d.getDate()).padStart(2,'0')}`
```

### 7) Components ที่จะ import จาก Reka UI

```ts
// Modal (IT 01-2, IT 01-3)
import {
  DialogRoot, DialogTrigger, DialogPortal, DialogOverlay,
  DialogContent, DialogTitle, DialogClose,
} from 'reka-ui'
```

DatePicker ของ Reka UI ใช้งานซับซ้อนกว่า — สำหรับโปรเจกต์นี้ใช้ **`<input type="date">` native** จะดีกว่า: ดิบ, เข้า brutalist mood, ฟรี a11y, format ออกเป็น `yyyy-mm-dd` ที่ส่ง API ได้ทันที

---

## Database Approach — DB-first

flow นี้ใช้แนวทาง **DB-first**: สร้าง schema ผ่าน Supabase SQL Editor (ตามเซกชันด้านล่าง) แล้ว C# entity แค่ map ทับ — **ไม่ใช้ EF Core migration**

เหตุผล:
- Supabase มี dashboard/SQL Editor ที่ใช้สะดวกอยู่แล้ว
- ไม่ต้อง config migration history table
- ทีมเดียว/โปรเจกต์เล็ก ไม่ต้อง track schema versioning

ถ้าจะเปลี่ยนเป็น code-first ภายหลัง: ใช้ `dotnet ef migrations add InitialCreate` + `dotnet ef database update` (ต้อง grant CREATE privilege บน `public` schema ใน Supabase)

---

## Supabase Setup (PostgreSQL ฟรี online)

### 1) สร้างโปรเจกต์
1. ไปที่ [supabase.com](https://supabase.com) → Sign up (ใช้ GitHub ได้)
2. **New project** → ตั้งชื่อ, ตั้ง **Database password** (เก็บไว้ให้ดี ใช้ในขั้นถัดไป), เลือก region ที่ใกล้ (Singapore แนะนำ)
3. รอ provision ~1-2 นาที

### 2) สร้างตาราง
- เมนูซ้าย → **SQL Editor** → New query → paste SQL จากเซกชัน Database Schema ด้านบน → **Run**
- ตรวจสอบที่ **Table Editor** ว่ามีตาราง `persons`

### 3) ดึง Connection string
- เมนูซ้าย → **Project Settings** → **Database** → ส่วน **Connection string** → เลือก tab **`.NET`**
- หรือใช้ **Session pooler** / **Transaction pooler** (แนะนำ pooler สำหรับ deploy บน serverless)

ตัวอย่าง connection string:
```
Host=db.<project-ref>.supabase.co;Port=5432;Database=postgres;Username=postgres;Password=<your-password>;SSL Mode=Require;Trust Server Certificate=true;
```

ใส่ใน `appsettings.Development.json`:
```json
{
  "ConnectionStrings": {
    "Default": "Host=db.xxxxx.supabase.co;Port=5432;Database=postgres;Username=postgres;Password=YOUR_PASSWORD;SSL Mode=Require;Trust Server Certificate=true;"
  }
}
```

> 💡 อย่า commit password เข้า git → ใช้ **User Secrets** หรือ environment variable แทน
> ```bash
> dotnet user-secrets init
> dotnet user-secrets set "ConnectionStrings:Default" "Host=...;Password=...;"
> ```

### 4) NuGet packages ที่ต้องติดตั้ง (ฝั่ง .NET)

```bash
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Swashbuckle.AspNetCore.Annotations
```

> ℹ️ **`DateOnly` ใน Npgsql** — ตั้งแต่ Npgsql 8 จะ map `DateOnly` ↔ PostgreSQL `date` อัตโนมัติ ไม่ต้อง config เพิ่ม
>
> ℹ️ **Swashbuckle + `DateOnly`** — Swashbuckle รุ่นใหม่รองรับ `DateOnly` แล้ว ถ้าเจอ schema เพี้ยน เพิ่ม `c.MapType<DateOnly>(() => new OpenApiSchema { Type = "string", Format = "date" });` ใน `AddSwaggerGen`

### 5) ลงทะเบียน DbContext

ตามตัวอย่างใน `Program.cs` ของเซกชัน API Spec ด้านบนแล้ว — บรรทัด `AddDbContext + UseNpgsql`

### 6) Entity (snake_case mapping)

```csharp
public class Person
{
    public long Id { get; set; }
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public DateOnly BirthDate { get; set; }
    public string? Address { get; set; }
    public DateTime CreatedAt { get; set; }
}
```

ใน `AppDbContext.OnModelCreating` ใช้ snake_case ให้ตรงกับ Supabase:
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

### 7) Row Level Security (RLS)
- Supabase เปิด RLS เป็น default — ถ้า API เชื่อมด้วย user `postgres` (service role) จะ bypass RLS ได้
- ในโปรเจกต์เรียน/ภายในนี้: ใช้ `postgres` user ตามค่า default ก็พอ
- ถ้าจะ public ในอนาคต: ปิด RLS ชั่วคราว หรือเขียน policy

---

### Free tier ของ Supabase
- ✅ 2 projects ฟรี
- ✅ 500 MB DB storage
- ✅ 5 GB bandwidth / เดือน
- ✅ 50,000 monthly active users (ถ้าใช้ Auth)
- ⚠️ Project **pause หลัง 1 สัปดาห์ที่ไม่มี activity** — เข้า dashboard เพื่อ resume

### เครื่องมือจัดการ DB
- **Supabase Dashboard** (ในเว็บ) — ใช้งานสะดวกสุด ⭐
- **DBeaver Community** (ฟรี, ข้ามแพลตฟอร์ม) — ถ้าอยากใช้ desktop tool
- **psql** (command line)

---

## CORS / Dev setup

- BE รันที่ `https://localhost:5001`
- FE รันที่ `http://localhost:5173` (Vite default)
- เปิด CORS ใน BE สำหรับ origin ของ Vite ใน Development (ดู `Program.cs` ด้านบน)

### Fix port ของ .NET ให้คงที่

`dotnet new webapi` จะ generate `Properties/launchSettings.json` ด้วย port สุ่ม → แก้ให้ตรงกับ CORS config:

```json
{
  "profiles": {
    "https": {
      "commandName": "Project",
      "launchBrowser": false,
      "applicationUrl": "https://localhost:5001;http://localhost:5000",
      "environmentVariables": { "ASPNETCORE_ENVIRONMENT": "Development" }
    }
  }
}
```

---

## Acceptance Criteria (สรุปเป็น checklist)

- [ ] เปิดหน้า IT 01-1 แล้วเห็นตารางพร้อมข้อมูลจาก DB
- [ ] กด `ADD` → เปิด Modal IT 01-2
- [ ] กรอกวันเกิด → ช่อง "อายุ" อัปเดตอัตโนมัติ
- [ ] กด `บันทึก` พร้อมข้อมูลครบ → ปิด modal, แถวใหม่ปรากฏท้ายตาราง
- [ ] กด `บันทึก` พร้อมข้อมูลไม่ครบ → แสดง validation error, ไม่ปิด modal
- [ ] กด `ยกเลิก` → ปิด modal, ไม่มีข้อมูลใหม่ใน DB
- [ ] กด `View` ที่แถวใด → Modal IT 01-3 แสดงข้อมูลของแถวนั้น (readonly)
- [ ] ใน IT 01-3 แก้ไขช่องใดไม่ได้
- [ ] กด `ปิด` ใน IT 01-3 → ปิด modal
- [ ] เลือกวันเกิดในอนาคต → BE ตอบ 400 และ FE แสดง error ใน modal
- [ ] API ล่ม (BE ปิด) → FE แสดง error state ในตาราง ไม่ crash
- [ ] รัน `dotnet run` + `npm run dev` → เปิด `http://localhost:5173` ได้โดยไม่มี CORS error ใน console
