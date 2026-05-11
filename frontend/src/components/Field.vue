<script setup lang="ts">
defineProps<{ label: string; required?: boolean }>()
</script>

<template>
  <div class="flex flex-col gap-1.5">
    <label class="text-xs font-semibold text-sub uppercase tracking-wide select-none">
      {{ label }}<span v-if="required" class="text-danger ml-0.5">*</span>
    </label>
    <slot />
  </div>
</template>
