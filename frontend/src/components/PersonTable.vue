<script setup lang="ts">
import type { Person } from '../types/person'
import { formatBirthDate } from '../lib/format'

defineProps<{ persons: Person[]; loading: boolean; error: string | null }>()
const emit = defineEmits<{ (e: 'view', person: Person): void }>()
</script>

<template>
  <!-- Loading skeleton -->
  <div v-if="loading" class="divide-y divide-border">
    <div class="px-5 py-3.5 grid grid-cols-4 gap-4 bg-bg/60">
      <div v-for="n in 4" :key="n" class="h-3 rounded bg-border animate-pulse" />
    </div>
    <div v-for="n in 5" :key="n" class="px-5 py-4 flex items-center gap-4">
      <div class="h-3 w-8 rounded bg-border/60 animate-pulse" />
      <div class="h-3 flex-1 rounded bg-border/60 animate-pulse" />
      <div class="h-3 w-24 rounded bg-border/60 animate-pulse hidden sm:block" />
      <div class="h-3 w-16 rounded bg-border/60 animate-pulse" />
    </div>
  </div>

  <!-- Error -->
  <div v-else-if="error" class="px-5 py-8 flex items-start gap-3 text-danger bg-danger-light">
    <svg class="w-5 h-5 shrink-0 mt-0.5" fill="none" viewBox="0 0 24 24" stroke="currentColor" stroke-width="2">
      <path stroke-linecap="round" stroke-linejoin="round" d="M12 9v3.75m-9.303 3.376c-.866 1.5.217 3.374 1.948 3.374h14.71c1.73 0 2.813-1.874 1.948-3.374L13.949 3.378c-.866-1.5-3.032-1.5-3.898 0L2.697 16.126zM12 15.75h.007v.008H12v-.008z"/>
    </svg>
    <span class="text-sm font-medium">{{ error }}</span>
  </div>

  <template v-else>
    <!-- ─── Desktop table ─── -->
    <div class="hidden sm:block overflow-x-auto">
      <table class="w-full text-sm border-collapse">
        <!-- Column headers -->
        <thead>
          <tr class="bg-bg border-b border-border">
            <th class="px-5 py-3 text-left text-xs font-semibold text-sub uppercase tracking-wider w-16">ลำดับ</th>
            <th class="px-5 py-3 text-left text-xs font-semibold text-sub uppercase tracking-wider">ชื่อ-นามสกุล</th>
            <th class="px-5 py-3 text-left text-xs font-semibold text-sub uppercase tracking-wider hidden lg:table-cell">ที่อยู่</th>
            <th class="px-5 py-3 text-left text-xs font-semibold text-sub uppercase tracking-wider w-32">วันเกิด</th>
            <th class="px-5 py-3 text-center text-xs font-semibold text-sub uppercase tracking-wider w-20">อายุ</th>
            <th class="px-5 py-3 text-center text-xs font-semibold text-sub uppercase tracking-wider w-24">จัดการ</th>
          </tr>
        </thead>
        <tbody class="divide-y divide-border">
          <!-- Empty state -->
          <tr v-if="persons.length === 0">
            <td colspan="6" class="px-5 py-16 text-center text-sub">
              <div class="flex flex-col items-center gap-2">
                <svg class="w-10 h-10 text-border" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="1.5" d="M20 13V6a2 2 0 00-2-2H6a2 2 0 00-2 2v7m16 0v5a2 2 0 01-2 2H6a2 2 0 01-2-2v-5m16 0h-2.586a1 1 0 00-.707.293l-2.414 2.414a1 1 0 01-.707.293h-3.172a1 1 0 01-.707-.293l-2.414-2.414A1 1 0 006.586 13H4"/>
                </svg>
                <span class="text-sm">ยังไม่มีข้อมูล</span>
              </div>
            </td>
          </tr>
          <!-- Data rows -->
          <tr
            v-for="(p, i) in persons"
            :key="p.id"
            class="hover:bg-primary-light transition-colors cursor-pointer group animate-[fade-in_150ms_ease-out_both]"
            :style="{ animationDelay: `${i * 25}ms` }"
            @click="emit('view', p)"
          >
            <td class="px-5 py-3.5 font-mono tabular text-muted text-xs">{{ p.id }}</td>
            <td class="px-5 py-3.5 font-medium text-ink">{{ p.firstName }} {{ p.lastName }}</td>
            <td class="px-5 py-3.5 text-sub hidden lg:table-cell">
              <span class="line-clamp-1">{{ p.address || '—' }}</span>
            </td>
            <td class="px-5 py-3.5 font-mono tabular text-sub text-xs">{{ formatBirthDate(p.birthDate) }}</td>
            <td class="px-5 py-3.5 text-center">
              <span class="inline-flex items-center gap-1 bg-primary-light text-primary font-semibold font-mono tabular px-2.5 py-0.5 rounded-full text-xs">
                {{ p.age }} ปี
              </span>
            </td>
            <td class="px-5 py-3.5 text-center" @click.stop>
              <button
                class="inline-flex items-center gap-1.5 border border-border hover:border-primary hover:text-primary text-sub px-3 py-1.5 rounded text-xs font-medium transition-colors"
                @click="emit('view', p)"
              >
                <svg class="w-3.5 h-3.5" fill="none" viewBox="0 0 24 24" stroke="currentColor" stroke-width="2">
                  <path stroke-linecap="round" stroke-linejoin="round" d="M2.036 12.322a1.012 1.012 0 010-.639C3.423 7.51 7.36 4.5 12 4.5c4.638 0 8.573 3.007 9.963 7.178.07.207.07.431 0 .639C20.577 16.49 16.64 19.5 12 19.5c-4.638 0-8.573-3.007-9.963-7.178z"/>
                  <path stroke-linecap="round" stroke-linejoin="round" d="M15 12a3 3 0 11-6 0 3 3 0 016 0z"/>
                </svg>
                ดูข้อมูล
              </button>
            </td>
          </tr>
        </tbody>
      </table>
    </div>

    <!-- ─── Mobile cards ─── -->
    <div class="sm:hidden divide-y divide-border">
      <div
        v-if="persons.length === 0"
        class="px-5 py-14 text-center text-sub text-sm"
      >
        ยังไม่มีข้อมูล
      </div>
      <div
        v-for="(p, i) in persons"
        :key="p.id"
        class="px-4 py-4 flex items-start justify-between gap-3 hover:bg-primary-light transition-colors cursor-pointer animate-[fade-in_150ms_ease-out_both]"
        :style="{ animationDelay: `${i * 30}ms` }"
        @click="emit('view', p)"
      >
        <div class="flex-1 min-w-0">
          <div class="font-semibold text-ink text-sm">{{ p.firstName }} {{ p.lastName }}</div>
          <div class="flex items-center gap-3 mt-1">
            <span class="font-mono tabular text-sub text-xs">{{ formatBirthDate(p.birthDate) }}</span>
            <span class="inline-flex items-center bg-primary-light text-primary font-semibold font-mono tabular px-2 py-0.5 rounded-full text-xs">
              {{ p.age }} ปี
            </span>
          </div>
          <div v-if="p.address" class="text-xs text-muted mt-1 line-clamp-1">{{ p.address }}</div>
        </div>
        <button
          class="shrink-0 text-primary border border-primary/30 hover:bg-primary hover:text-white px-3 py-1.5 rounded text-xs font-medium transition-colors"
          @click.stop="emit('view', p)"
        >
          ดูข้อมูล
        </button>
      </div>
    </div>
  </template>
</template>
