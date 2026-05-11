<script setup lang="ts">
import { ref, onMounted } from 'vue'
import type { Person } from '../types/person'
import { personApi } from '../api/personApi'
import PersonTable from '../components/PersonTable.vue'
import PersonAddModal from '../components/PersonAddModal.vue'
import PersonViewModal from '../components/PersonViewModal.vue'

const persons = ref<Person[]>([])
const loading = ref(false)
const error   = ref<string | null>(null)
const addOpen = ref(false)
const viewOpen = ref(false)
const selected = ref<Person | null>(null)

async function load() {
  loading.value = true
  error.value = null
  try {
    persons.value = await personApi.list()
  } catch {
    error.value = 'ไม่สามารถโหลดข้อมูลได้ กรุณาตรวจสอบการเชื่อมต่อ'
  } finally {
    loading.value = false
  }
}

function onView(p: Person) {
  selected.value = p
  viewOpen.value = true
}

onMounted(load)
</script>

<template>
  <!-- Top navbar -->
  <header class="bg-primary shadow-md sticky top-0 z-30">
    <div class="max-w-6xl mx-auto px-4 sm:px-6 h-14 flex items-center gap-3">
      <div class="w-7 h-7 bg-white/20 rounded flex items-center justify-center shrink-0">
        <svg class="w-4 h-4 text-white" fill="none" viewBox="0 0 24 24" stroke="currentColor" stroke-width="2">
          <path stroke-linecap="round" stroke-linejoin="round" d="M17 20h5v-2a3 3 0 00-5.356-1.857M17 20H7m10 0v-2c0-.656-.126-1.283-.356-1.857M7 20H2v-2a3 3 0 015.356-1.857M7 20v-2c0-.656.126-1.283.356-1.857m0 0a5.002 5.002 0 019.288 0M15 7a3 3 0 11-6 0 3 3 0 016 0z"/>
        </svg>
      </div>
      <span class="text-white font-semibold text-base tracking-wide">ระบบจัดการข้อมูลบุคคล</span>
    </div>
  </header>

  <!-- Loading bar -->
  <div class="h-0.5 bg-bg sticky top-14 z-20">
    <div
      class="h-full bg-primary transition-all duration-500"
      :class="loading ? 'w-2/3 opacity-100' : 'w-full opacity-0'"
    />
  </div>

  <!-- Main content -->
  <main class="max-w-6xl mx-auto px-4 sm:px-6 py-6 sm:py-8">

    <!-- Page heading card -->
    <div class="bg-surface rounded-lg shadow-sm border border-border px-5 py-4 mb-5 flex items-center justify-between gap-4 flex-wrap">
      <div>
        <h1 class="text-xl font-bold text-ink">รายการข้อมูลบุคคล</h1>
        <p class="text-sub text-xs mt-0.5">
          ทั้งหมด
          <span class="font-semibold text-primary font-mono tabular">{{ persons.length }}</span>
          รายการ
        </p>
      </div>
      <button
        class="bg-primary hover:bg-primary-h text-white px-5 py-2.5 rounded-md text-sm font-semibold shadow-sm flex items-center gap-2 transition-colors shrink-0"
        @click="addOpen = true"
      >
        <svg class="w-4 h-4" fill="none" viewBox="0 0 24 24" stroke="currentColor" stroke-width="2.5">
          <path stroke-linecap="round" stroke-linejoin="round" d="M12 4v16m8-8H4"/>
        </svg>
        เพิ่มข้อมูล
      </button>
    </div>

    <!-- Table card -->
    <div class="bg-surface rounded-lg shadow-sm border border-border overflow-hidden">
      <PersonTable
        :persons="persons"
        :loading="loading"
        :error="error"
        @view="onView"
      />
    </div>

  </main>

  <PersonAddModal v-model:open="addOpen" @created="load" />
  <PersonViewModal v-model:open="viewOpen" :person="selected" />
</template>
