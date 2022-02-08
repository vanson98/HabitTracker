<template>
  <el-dialog
    :model-value="isOpen"
    :title="editTransactionId ? 'Cập nhật giao dịch' : 'Tạo mới giao dịch'"
    width="30%"
    @close="closeModel(false)"
  >
    <div class="flex flex-col">
      <div>
        <label class="mb-3">Nhập mã cổ phiếu</label>
        <el-select
          v-model="transaction.investmentId"
          class="w-full my-2"
          placeholder="Chọn mã cổ phiếu"
        >
          <el-option
            v-for="item in listInvestment"
            :key="item.id"
            :label="item.stockCode + ' - ' + item.companyName"
            :value="item.id"
          ></el-option>
        </el-select>
      </div>
      <div>
        <label>Loại giao dịch</label>
        <el-select
          v-model="transaction.transactionType"
          placeholder="Chọn loại giao dịch"
          class="w-full my-2"
        >
          <el-option
            v-for="keyEnum in transactionTypeEnumKey"
            :key="keyEnum"
            :label="keyEnum"
            :value="transactionType[keyEnum]"
          ></el-option>
        </el-select>
      </div>
      <div>
        <label>Giá (x1000)</label>
        <el-input
          v-model="transaction.price"
          type="number"
          class="w-full my-2"
          placeholder="Nhập giá"
        ></el-input>
      </div>
      <div>
        <label>Số lượng</label>
        <el-input
          v-model="transaction.numberOfShares"
          type="number"
          class="w-full my-2"
          placeholder="Nhập số lượng"
        ></el-input>
      </div>
      <div class="flex">
        <div class="w-2/3">
          <div>Thời gian giao dịch</div>
          <el-date-picker
            v-model="transaction.transactionTime"
            type="date"
            class="w-full my-2"
            placeholder="Chọn ngày"
          ></el-date-picker>
        </div>
        <div class="w-1/3">
          <p>Thành tiền</p>
          {{
            transaction.numberOfShares && transaction.price
              ? util.formatCurrency(
                  transaction.numberOfShares * transaction.price * 1000,
                )
              : ""
          }}
        </div>
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
import { InvestmentSelectDto } from "@/models/investment/InvestmentDtos";
import util from "@/lib/util";
import TransactionDto, {
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
  onBeforeMount,
} from "vue";
import investmentService from "@/services/investment.service";

// Data
let listInvestment = ref<InvestmentSelectDto[]>();
let transactionType = TransactionType;
let transactionTypeEnumKey = util.getEnumKeys(transactionType);
let transaction = ref<TransactionDto>({
  transactionTime: new Date(),
  transactionType: transactionType.BUY,
  totalFee: 0,
});

// Prop
const props = withDefaults(
  defineProps<{
    isOpen: boolean;
    editTransactionId: number | null;
    channelId: number;
  }>(),
  {
    isOpen: false,
    editTransactionId: null,
    channelId: 0,
  },
);

// Event
const emits = defineEmits(["close"]);

// Hook
onUpdated(() => {
  if (props.channelId > 0) {
    investmentService.getAllForSelect(props.channelId).then((res) => {
      listInvestment.value = res.result;
    });
  }
});

onUpdated(() => {
  if (props.editTransactionId && props.isOpen) {
    transactionService.getById(props.editTransactionId).then((res) => {
      transaction.value = res.result;
    });
  } else if (!props.editTransactionId && props.isOpen) {
    transaction.value = {
      transactionTime: new Date(),
      transactionType: transactionType.BUY,
      totalFee: 0,
    };
  }
});

// Method
async function save() {
  if (props.editTransactionId) {
    // Cập nhật
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
          message: res.error.message,
          type: "error",
        });
        closeModel(false);
      }
    });
  } else {
    // Thêm mới
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
