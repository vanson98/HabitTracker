<template>
  <el-dialog
    :model-value="isOpen"
    :title="mode == 'ADD' ? 'Nhập tiền' : 'Rút tiền'"
  >
  </el-dialog>
</template>
<script lang="ts" setup>
import { defineProps, withDefaults } from "vue";
const props = withDefaults(defineProps<{ isOpen: boolean; mode: string }>(), {
  isOpen: false,
  mode: "ADD",
});
</script>
