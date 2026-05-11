<script setup lang="ts">
import { ref, computed, watch } from 'vue'
import { DialogRoot, DialogPortal, DialogOverlay, DialogContent, DialogTitle, DialogClose } from 'reka-ui'
import { personApi } from '../api/personApi'
import { calcAge } from '../lib/format'
import Field from './Field.vue'

const props = defineProps<{ open: boolean }>()
const emit = defineEmits<{
  (e: 'update:open', value: boolean): void
  (e: 'created'): void
}>()

const firstName  = ref('')
const lastName   = ref('')
const birthDate  = ref('')
const address    = ref('')
const submitting  = ref(false)
const serverError = ref<string | null>(null)

const age = computed(() => calcAge(birthDate.value))
const canSubmit = computed(() =>
  !!firstName.value.trim() && !!lastName.value.trim() && !!birthDate.value && !submitting.value
)

function reset() {
  firstName.value = lastName.value = birthDate.value = address.value = ''
  serverError.value = null
  submitting.value = false
}

watch(() => props.open, v => { if (!v) reset() })

async function save() {
  if (!canSubmit.value) return
  submitting.value = true
  serverError.value = null
  try {
    await personApi.create({
      firstName: firstName.value.trim(),
      lastName:  lastName.value.trim(),
      birthDate: birthDate.value,
      address:   address.value.trim() || null,
    })
    emit('created')
    emit('update:open', false)
  } catch (err: any) {
    serverError.value = err?.response?.data?.title ?? 'บันทึกไม่สำเร็จ กรุณาลองอีกครั้ง'
  } finally {
    submitting.value = false
  }
}

const inputClass = `
  w-full border border-border rounded-md px-3 py-2.5 text-sm text-ink
  focus:outline-none focus:ring-2 focus:ring-primary/30 focus:border-primary
  placeholder:text-muted transition-shadow
`
</script>

<template>
  <DialogRoot :open="open" @update:open="emit('update:open', $event)">
    <DialogPortal>
      <!-- Backdrop -->
      <DialogOverlay class="fixed inset-0 bg-ink/40 backdrop-blur-sm z-40 animate-[fade-in_150ms_ease-out]" />

      <!-- Panel -->
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
          <DialogTitle class="font-bold text-base text-ink">เพิ่มข้อมูลบุคคล</DialogTitle>
          <DialogClose class="w-8 h-8 flex items-center justify-center rounded-full hover:bg-bg text-muted hover:text-sub transition-colors">
            <svg class="w-4 h-4" fill="none" viewBox="0 0 24 24" stroke="currentColor" stroke-width="2.5">
              <path stroke-linecap="round" stroke-linejoin="round" d="M6 18L18 6M6 6l12 12"/>
            </svg>
          </DialogClose>
        </div>

        <!-- Form body -->
        <form class="px-5 py-5 space-y-4" @submit.prevent="save">
          <div class="grid grid-cols-1 sm:grid-cols-2 gap-4">
            <Field label="ชื่อ" :required="true">
              <input v-model="firstName" type="text" placeholder="ชื่อ" :class="inputClass" required />
            </Field>
            <Field label="นามสกุล" :required="true">
              <input v-model="lastName" type="text" placeholder="นามสกุล" :class="inputClass" required />
            </Field>
          </div>

          <div class="grid grid-cols-2 gap-4">
            <Field label="วันเกิด" :required="true">
              <input v-model="birthDate" type="date" :class="inputClass" required />
            </Field>
            <Field label="อายุ (คำนวณอัตโนมัติ)">
              <div class="border border-border rounded-md px-3 py-2.5 bg-bg text-sm font-mono tabular text-sub h-[42px] flex items-center">
                <span v-if="age !== null" class="font-semibold text-ink">{{ age }} <span class="text-muted font-normal">ปี</span></span>
                <span v-else class="text-muted">—</span>
              </div>
            </Field>
          </div>

          <Field label="ที่อยู่">
            <textarea
              v-model="address"
              rows="3"
              placeholder="ที่อยู่ (ไม่บังคับ)"
              :class="inputClass + ' resize-none'"
            />
          </Field>

          <!-- Error -->
          <div v-if="serverError" class="flex items-start gap-2.5 bg-danger-light border border-danger/20 rounded-md px-4 py-3 text-danger text-sm">
            <svg class="w-4 h-4 shrink-0 mt-0.5" fill="none" viewBox="0 0 24 24" stroke="currentColor" stroke-width="2">
              <path stroke-linecap="round" stroke-linejoin="round" d="M12 9v3.75m9-.75a9 9 0 11-18 0 9 9 0 0118 0zm-9 3.75h.008v.008H12v-.008z"/>
            </svg>
            {{ serverError }}
          </div>
        </form>

        <!-- Modal footer -->
        <div class="flex items-center justify-end gap-3 px-5 py-4 border-t border-border bg-bg/50">
          <DialogClose class="px-4 py-2.5 rounded-md text-sm font-medium text-sub hover:bg-border/60 transition-colors">
            ยกเลิก
          </DialogClose>
          <button
            type="button"
            :disabled="!canSubmit"
            class="flex items-center gap-2 bg-primary hover:bg-primary-h text-white px-5 py-2.5 rounded-md text-sm font-semibold shadow-sm transition-colors disabled:opacity-40 disabled:cursor-not-allowed"
            @click="save"
          >
            <svg v-if="submitting" class="w-4 h-4 animate-spin" fill="none" viewBox="0 0 24 24">
              <circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4"/>
              <path class="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8v8H4z"/>
            </svg>
            <svg v-else class="w-4 h-4" fill="none" viewBox="0 0 24 24" stroke="currentColor" stroke-width="2.5">
              <path stroke-linecap="round" stroke-linejoin="round" d="M9 12.75L11.25 15 15 9.75M21 12a9 9 0 11-18 0 9 9 0 0118 0z"/>
            </svg>
            บันทึก
          </button>
        </div>
      </DialogContent>
    </DialogPortal>
  </DialogRoot>
</template>
