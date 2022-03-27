<template>
  <el-dialog
    :model-value="isOpen"
    :title="'Tạo mới giao dịch'"
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
          filterable
          remote
          reserve-keyword
          :remote-method="remoteMethod"
          :loading="getListInvestmentLoading"
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
        <div>
          <div>Thời gian giao dịch</div>
          <el-date-picker
            v-model="transaction.transactionTime"
            type="date"
            class="w-full my-2"
            placeholder="Chọn ngày"
          ></el-date-picker>
        </div>
      </div>
      <div>
        <p>Thành tiền: {{ totalAmount }}</p>
        <div>
          <span>Tổng phí tự nhập (x1000đ):</span>
          <el-input v-model="transaction.totalFee" type="number"> </el-input>
          <span> Tổng phí (máy tính): {{ totalFee }} </span>
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
  computed,
} from "vue";
import investmentService from "@/services/investment.service";
import { appStore } from "@/store/appStore";
import { storeToRefs } from "pinia";

// Store
var store = appStore();
const { channel } = storeToRefs(store);

// Data
let listInvestment = ref<InvestmentSelectDto[]>();
let transactionType = TransactionType;
let transactionTypeEnumKey = util.getEnumKeys(transactionType);
let transaction = ref<TransactionDto>({
  transactionTime: new Date(),
  transactionType: transactionType.BUY,
  totalFee: -1,
  numberOfShares: 0,
  price: 0,
});
let getListInvestmentLoading = ref<boolean>(false);

// Prop
const props = withDefaults(
  defineProps<{
    isOpen: boolean;
  }>(),
  {
    isOpen: false,
  },
);

//Computed property
const totalAmount = computed((): string => {
  var result =
    transaction.value.numberOfShares && transaction.value.price
      ? util.formatCurrency(
          transaction.value.numberOfShares * transaction.value.price * 1000,
        )
      : "";
  return result;
});

const totalFee = computed((): string => {
  if (transaction.value.numberOfShares && transaction.value.price) {
    var feePortion =
      transaction.value.transactionType == transactionType.BUY
        ? channel.value.buyFee
        : channel.value.sellFee;
    return util.formatCurrency(
      (feePortion *
        (transaction.value.numberOfShares * transaction.value.price * 1000)) /
        100,
    );
  } else {
    return "";
  }
});

// Event
const emits = defineEmits(["close"]);

// Hook
onUpdated(() => {
  // if (channel.value.id > 0 && props.isOpen && props.editTransactionId) {

  // }
  if (props.isOpen) {
    transaction.value = {
      transactionTime: new Date(),
      transactionType: transactionType.BUY,
      totalFee: -1,
    };
  }
});

// Method
async function save() {
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

const remoteMethod = (query: string) => {
  if (query) {
    getListInvestmentLoading.value = true;
    setTimeout(() => {
      getListInvestmentLoading.value = false;
      getAllInvestmentForSelect(query);
    }, 200);
  } else {
    listInvestment.value = [];
  }
};

// Get all investment for select
const getAllInvestmentForSelect = (query: string | undefined) => {
  if (channel.value.id != null) {
    investmentService.getAllForSelect(channel.value.id, query).then((res) => {
      listInvestment.value = res.result;
    });
  }
};

// close model
function closeModel(isSuccess: boolean) {
  emits("close", isSuccess);
}
</script>
