<script setup lang="ts">
import { DialogRoot, DialogPortal, DialogOverlay, DialogContent, DialogTitle, DialogClose } from 'reka-ui'
import type { Person } from '../types/person'
import Field from './Field.vue'
import { formatBirthDate } from '../lib/format'

defineProps<{ open: boolean; person: Person | null }>()
const emit = defineEmits<{ (e: 'update:open', value: boolean): void }>()
</script>

<template>
  <DialogRoot :open="open" @update:open="emit('update:open', $event)">
    <DialogPortal>
      <DialogOverlay class="fixed inset-0 bg-ink/40 backdrop-blur-sm z-40 animate-[fade-in_150ms_ease-out]" />

      <DialogContent
        class="
          fixed z-50 bg-surface
          w-full bottom-0 left-0 right-0 rounded-t-2xl max-h-[92dvh] overflow-y-auto shadow-xl
          animate-[slide-in-bottom_260ms_cubic-bezier(0.16,1,0.3,1)]
          sm:bottom-auto sm:left-1/2 sm:top-1/2 sm:-translate-x-1/2 sm:-translate-y-1/2
          sm:w-[560px] sm:max-w-[92vw] sm:rounded-xl sm:max-h-[90vh]
          sm:animate-[slide-up_200ms_cubic-bezier(0.16,1,0.3,1)]
        "
      >
        <!-- Drag handle -->
        <div class="sm:hidden flex justify-center pt-3">
          <div class="w-9 h-1 rounded-full bg-border" />
        </div>

        <!-- Modal header -->
        <div class="flex items-center justify-between px-5 py-4 border-b border-border">
          <DialogTitle class="font-bold text-base text-ink">ข้อมูลบุคคล</DialogTitle>
          <DialogClose class="w-8 h-8 flex items-center justify-center rounded-full hover:bg-bg text-muted hover:text-sub transition-colors">
            <svg class="w-4 h-4" fill="none" viewBox="0 0 24 24" stroke="currentColor" stroke-width="2.5">
              <path stroke-linecap="round" stroke-linejoin="round" d="M6 18L18 6M6 6l12 12"/>
            </svg>
          </DialogClose>
        </div>

        <!-- Content -->
        <div v-if="person" class="px-5 py-5 space-y-4">
          <!-- ID badge -->
          <div class="flex items-center gap-2">
            <span class="text-xs font-semibold text-muted uppercase tracking-wide">รหัส</span>
            <span class="bg-bg border border-border text-sub font-mono tabular text-xs px-2.5 py-1 rounded-full">#{{ person.id }}</span>
          </div>

          <div class="grid grid-cols-1 sm:grid-cols-2 gap-4">
            <Field label="ชื่อ">
              <div class="border border-border rounded-md px-3 py-2.5 bg-bg text-sm text-ink">{{ person.firstName }}</div>
            </Field>
            <Field label="นามสกุล">
              <div class="border border-border rounded-md px-3 py-2.5 bg-bg text-sm text-ink">{{ person.lastName }}</div>
            </Field>
          </div>

          <div class="grid grid-cols-2 gap-4">
            <Field label="วันเกิด">
              <div class="border border-border rounded-md px-3 py-2.5 bg-bg text-sm font-mono tabular text-sub">
                {{ formatBirthDate(person.birthDate) }}
              </div>
            </Field>
            <Field label="อายุ">
              <div class="border border-border rounded-md px-3 py-2.5 bg-bg text-sm font-mono tabular h-[42px] flex items-center">
                <span class="font-semibold text-ink">{{ person.age }}</span>
                <span class="text-muted ml-1">ปี</span>
              </div>
            </Field>
          </div>

          <Field label="ที่อยู่">
            <div class="border border-border rounded-md px-3 py-2.5 bg-bg text-sm text-sub whitespace-pre-wrap min-h-[72px]">
              {{ person.address || '—' }}
            </div>
          </Field>
        </div>

        <!-- Modal footer -->
        <div class="flex justify-end px-5 py-4 border-t border-border bg-bg/50">
          <DialogClose
            class="flex items-center gap-2 bg-primary hover:bg-primary-h text-white px-5 py-2.5 rounded-md text-sm font-semibold shadow-sm transition-colors"
          >
            <svg class="w-4 h-4" fill="none" viewBox="0 0 24 24" stroke="currentColor" stroke-width="2.5">
              <path stroke-linecap="round" stroke-linejoin="round" d="M6 18L18 6M6 6l12 12"/>
            </svg>
            ปิด
          </DialogClose>
        </div>
      </DialogContent>
    </DialogPortal>
  </DialogRoot>
</template>
