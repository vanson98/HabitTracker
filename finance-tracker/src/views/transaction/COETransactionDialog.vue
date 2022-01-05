<template>
  <el-dialog
    :model-value="isOpen"
    :title="editTransactionId ? 'Cập nhật giao dịch' : 'Tạo mới giao dịch'"
    width="30%"
    @close="closeModel(false)"
  >
    <div>
      <div>
        <label>Nhập mã cổ phiếu</label>
        <el-select
          v-model="transaction.investmentId"
          class="m-2"
          placeholder="Chọn mã cổ phiếu"
        >
          <el-option
            v-for="item in listInvestment"
            :key="item.id"
            :label="item.stockCode + '-' + item.companyName"
            :value="item.id"
          ></el-option>
        </el-select>
      </div>
      <div>
        <label>Loại giao dịch</label>
        <el-select
          v-model="transaction.transactionType"
          placeholder="Chọn loại giao dịch"
        >
          <el-option
            v-for="item in TransactionType"
            :key="item"
            :label="item"
            :value="item"
          ></el-option>
        </el-select>
      </div>
      <div>
        <label>Giá</label>
        <el-input v-model="transaction.price" type="number"></el-input>
      </div>
      <div>
        <label>Số lượng</label>
        <el-input v-model="transaction.numberOfShares" type="number"></el-input>
      </div>
      <div>
        <label>Thời gian giao dịch</label>
        <el-date-picker
          v-model="transaction.transactionTime"
          type="date"
          placeholder="Pick a day"
        >
        </el-date-picker>
      </div>
    </div>
    <template #footer>
      <span class="dialog-footer">
        <el-button @click="closeModel(false)">Cancel</el-button>
        <el-button type="primary" @click="save">Confirm</el-button>
      </span>
    </template>
  </el-dialog>
</template>
<script lang="ts" setup>
import InvestmentSelectDto from "@/models/investment/InvestmentSelectDto";
import Transaction, {
  TransactionType,
} from "@/models/transaction/TransactionModels";
import transactionService from "@/services/transaction.service";
import { ElNotification } from "element-plus";
import {
  ref,
  defineProps,
  defineEmits,
  withDefaults,
  onUpdated,
  onMounted,
  onActivated,
  onDeactivated,
  watch,
  onBeforeMount,
} from "vue";
import financeService from "@/services/finance.service";

//data
let transaction = ref<Transaction>({});
let listInvestment: InvestmentSelectDto[] = [];

// Prop
const props = withDefaults(
  defineProps<{ isOpen: boolean; editTransactionId: number | null }>(),
  {
    isOpen: false,
    editTransactionId: null,
  },
);

// Event
const emits = defineEmits(["close"]);

// Hook
onBeforeMount(() => {
  financeService.getAllForSelect().then((res) => {
    listInvestment = res.result;
  });
});

onUpdated(() => {
  if (props.editTransactionId && props.isOpen) {
    transactionService.getById(props.editTransactionId).then((res) => {
      transaction.value = res.result;
    });
  } else if (!props.editTransactionId && props.isOpen) {
    transaction.value = {};
  }
});

// Method
async function save() {
  if (props.editTransactionId) {
    transactionService.update(transaction.value).then((res) => {
      if (res.success) {
        ElNotification({
          title: "Success",
          message: "Cập nhật thành công",
          type: "success",
        });
        closeModel(true);
      } else {
        ElNotification({
          title: "Error",
          message: result.error.message,
          type: "error",
        });
        closeModel(false);
      }
    });
  } else {
    var result = await transactionService.create(transaction.value);
    if (!result.success) {
      ElNotification({
        title: "Error",
        message: result.error.message,
        type: "error",
      });

      closeModel(false);
    } else {
      ElNotification({
        title: "Success",
        message: "Thêm mới thành công",
        type: "success",
      });
      closeModel(true);
    }
  }
}

// close model
function closeModel(isSuccess: boolean) {
  emits("close", isSuccess);
}
</script>
