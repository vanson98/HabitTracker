<template>
  <div>
    <div class="my-2 flex flex-row">
      <div>
        <el-button @click="createTransaction">Add Transaction</el-button>
      </div>
      <div>
        <label>Stock Code</label>
        <el-select
          v-model="searchingInfo.investmentId"
          placeholder="Nhập mã CP"
        >
          <el-option
            v-for="item in listInvestment"
            :key="item.id"
            :value="item.id"
            :label="item.stockCode"
          ></el-option>
        </el-select>
      </div>
      <div>
        <label>Loại giao dịch</label>
        <el-select
          v-model="searchingInfo.transactionType"
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
      <div class="block">
        <label class="demonstration">With quick options</label>
        <el-date-picker
          v-model="timeRangeSelect"
          type="daterange"
          unlink-panels
          range-separator="To"
          start-placeholder="Start date"
          end-placeholder="End date"
          :shortcuts="defaultTimeRange"
        >
        </el-date-picker>
      </div>
      <div></div>
    </div>
    <div class="table-ctn">
      <el-table
        :data="listTransaction"
        :border="true"
        style="width: 100%"
        height="550"
      >
        <el-table-column label="STT" width="50" :align="'center'">
          <template #default="scope">
            <span>{{ (currentPage - 1) * 10 + scope.$index + 1 }}</span>
          </template>
        </el-table-column>
        <el-table-column
          prop="stockCode"
          label="Mã CK"
          width="150"
        ></el-table-column>
        <el-table-column prop="transactionType" label="Type" width="150">
        </el-table-column>
        <el-table-column prop="price" label="Giá"></el-table-column>
        <el-table-column
          prop="numberOfShares"
          label="Số lượng"
        ></el-table-column>
        <el-table-column
          prop="transactionTime"
          label="Thời gian giao dịch"
        ></el-table-column>
        <el-table-column
          prop="createDate"
          label="Thời gian tạo"
        ></el-table-column>
        <el-table-column label="Action" :align="'center'" width="180">
          <template #default="scope">
            <el-button size="small" @click="editTransaction(scope.row.id)"
              >Edit</el-button
            >
            <el-button
              size="small"
              type="danger"
              @click="deleteTransaction(scope.row)"
              >Delete</el-button
            >
          </template>
        </el-table-column>
      </el-table>
      <div class="text-center my-6">
        <el-pagination
          layout="prev, pager, next"
          :total="totalCount"
          :pageSize="pageSize"
          :background="true"
          @current-change="pageChange($event)"
        ></el-pagination>
      </div>
    </div>
    <COETransactionDialog
      :isOpen="isOpenDialog"
      :editTransactionId="editTransactionId"
      @close="closeDialog"
    ></COETransactionDialog>
  </div>
</template>
<script lang="ts" setup>
import { onBeforeMount, onMounted, ref } from "vue";
import COETransactionDialog from "./COETransactionDialog.vue";
import { ElButton } from "element-plus";
import TransactionDto, {
  SearchTransactionInputDto,
  TransactionType,
} from "@/models/transaction/TransactionModels";
import financeService from "@/services/finance.service";
import transactionService from "@/services/transaction.service";
import { SearchTransactionOutputDto } from "@/models/transaction/TransactionModels";
import InvestmentSelectDto from "@/models/investment/InvestmentSelectDto";

const pageSize = 10;
let currentPage = ref(1);
let totalCount = ref(0);
let isOpenDialog = ref(false);
let editTransactionId = ref<number | null>(null);
let listTransaction = ref<SearchTransactionOutputDto[]>();
let listInvestment: InvestmentSelectDto[] = [];
let searchingInfo = ref<SearchTransactionInputDto>({
  maxResultCount: pageSize,
  skipCount: 0,
});
let timeRangeSelect = ref("");
const defaultTimeRange = [
  {
    text: "Last week",
    value: () => {
      const end = new Date();
      const start = new Date();
      start.setTime(start.getTime() - 3600 * 1000 * 24 * 7);
      return [start, end];
    },
  },
  {
    text: "Last month",
    value: () => {
      const end = new Date();
      const start = new Date();
      start.setTime(start.getTime() - 3600 * 1000 * 24 * 30);
      return [start, end];
    },
  },
  {
    text: "Last 3 months",
    value: () => {
      const end = new Date();
      const start = new Date();
      start.setTime(start.getTime() - 3600 * 1000 * 24 * 90);
      return [start, end];
    },
  },
];

// Event
onBeforeMount(() => {
  getAllInvestment();
  getAllTransaction();
});

// Page change
const pageChange = (page: number) => {
  currentPage.value = page;
  getAllTransaction();
};

// Get All Transaction
const getAllTransaction = () => {
  searchingInfo.value.skipCount = (currentPage.value - 1) * pageSize;
  transactionService
    .searchPaging(searchingInfo.value)
    .then((res) => {
      listTransaction.value = res.result.items;
      totalCount.value = res.result.totalCount;
    })
    .catch(() => {
      alert("Đã có lỗi xảy ra");
    });
};

// Get all investment
const getAllInvestment = () => {
  financeService.getAllForSelect().then((res) => {
    listInvestment = res.result;
  });
};

const createTransaction = () => {
  isOpenDialog.value = true;
  editTransactionId.value = null;
};

const editTransaction = (id: number) => {
  isOpenDialog.value = true;
  editTransactionId.value = id;
};

const deleteTransaction = (row: any) => {
  console.log("delete");
};

const closeDialog = (isSuccess: boolean) => {
  isOpenDialog.value = false;
  if (isSuccess) {
    getAllTransaction();
  }
};
</script>
<style></style>
