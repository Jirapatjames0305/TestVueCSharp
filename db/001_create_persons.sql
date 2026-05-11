-- IT 01 — Persons table
-- รันใน Supabase SQL Editor (Project → SQL Editor → New query → paste → Run)

create table public.persons (
  id           bigserial primary key,
  first_name   varchar(100) not null,
  last_name    varchar(100) not null,
  birth_date   date         not null,
  address      varchar(500),
  created_at   timestamptz  not null default now()
);

-- (ตัวเลือก) ข้อมูล seed ไว้ทดสอบหน้าตาราง
insert into public.persons (first_name, last_name, birth_date, address) values
  ('สมชาย', 'ใจดี',    '1990-04-15', '123 ถ.พหลโยธิน เขตจตุจักร กทม.'),
  ('สมหญิง', 'มีสุข',  '1995-11-02', '45/2 ถ.สุขุมวิท เขตวัฒนา กทม.'),
  ('อาทิตย์', 'แสงทอง', '2001-07-21', null);

-- (ตัวเลือก) ปิด Row Level Security สำหรับโปรเจกต์ภายใน/เรียน
-- หาก API เชื่อมด้วย user 'postgres' (จาก connection string ตรง) จะ bypass RLS ได้อยู่แล้ว
-- ถ้าต้องการให้ anon/authenticated เข้าถึงผ่าน Supabase client ด้วย:
-- alter table public.persons disable row level security;
