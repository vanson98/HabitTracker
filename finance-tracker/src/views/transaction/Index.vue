<template>
  <div>
    <div>
      <el-button @click="createTransaction" class="float-right" type="primary"
        >Add Transaction</el-button
      >
    </div>
    <!-- start table content -->
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
        <el-table-column label="Type" width="150">
          <template #default="scope">
            <span
              :class="{
                't-buy': scope.row.transactionType == transactionType.BUY,
                't-sell': scope.row.transactionType == transactionType.SELL,
              }"
              >{{ transactionType[scope.row.transactionType] }}</span
            >
          </template>
        </el-table-column>
        <el-table-column prop="price" label="Giá (x1000đ)"></el-table-column>
        <el-table-column
          prop="numberOfShares"
          label="Số lượng"
        ></el-table-column>
        <el-table-column label="Tổng tiền">
          <template #default="scope">
            <span>
              {{
                util.formatCurrency(
                  scope.row.numberOfShares * scope.row.price * 1000,
                )
              }}
            </span>
          </template>
        </el-table-column>
        <el-table-column label="Thời gian giao dịch">
          <template #default="scope">
            <span>{{
              moment(scope.row.transactionTime).format("DD/MM/yyyy")
            }}</span>
          </template>
        </el-table-column>
        <el-table-column label="Thời gian tạo">
          <template #default="scope">
            <span>{{
              moment(scope.row.creationTime).format("DD/MM/yyyy")
            }}</span>
          </template>
        </el-table-column>
        <el-table-column label="Action" :align="'center'" width="180">
          <template #default="scope">
            <el-button size="small" @click="editTransaction(scope.row.id)"
              >Edit</el-button
            >
            <el-button
              size="small"
              type="danger"
              @click="deleteTransaction(scope.row.id)"
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
    <!-- end table content -->

    <!-- dialogs -->
    <COETransactionDialog
      :isOpen="isOpenCOETransactionDialog"
      :editTransactionId="editTransactionId"
      @close="onCloseCOETransactionDialog"
      :channelId="props.channelId"
    ></COETransactionDialog>
  </div>
</template>
<script lang="ts" setup>
import {
  onBeforeMount,
  ref,
  defineProps,
  onMounted,
  onUpdated,
  defineEmits,
} from "vue";
import COETransactionDialog from "./COETransactionDialog.vue";
import { ElButton, ElMessage, ElMessageBox } from "element-plus";
import TransactionDto, {
  SearchTransactionInputDto,
  TransactionType,
} from "@/models/transaction/TransactionModels";

import transactionService from "@/services/transaction.service";
import { SearchTransactionOutputDto } from "@/models/transaction/TransactionModels";
import util from "@/lib/util";
import moment from "moment";

// props
let props = defineProps<{
  investmentIdSelected: number | undefined;
  transactionType: number;
  timeRangeSelected: string | Array<any>;
  channelId: number;
}>();
// emit
const emits = defineEmits(["reloadData"]);
// page data
const pageSize = 10;
let transactionType = TransactionType;
let currentPage = ref(1);
let totalCount = ref(0);
let listTransaction = ref<SearchTransactionOutputDto[]>();
// dialog data
let isOpenCOETransactionDialog = ref(false);
let isOpenAddOrWithdrawMoneyDialog = ref(false);
let editTransactionId = ref<number | null>(null);

const init = () => {
  getAllTransaction();
};

// Event
onBeforeMount(() => {
  init();
});

// Page change
const pageChange = (page: number) => {
  currentPage.value = page;
  getAllTransaction();
};

// Get All Transaction
const getAllTransaction = () => {
  var searchingInfo: SearchTransactionInputDto = {
    skipCount: (currentPage.value - 1) * pageSize,
    maxResultCount: pageSize,
    transactionType: props.transactionType,
    investmentId: props.investmentIdSelected,
  };
  if (
    props.timeRangeSelected != null &&
    Array.isArray(props.timeRangeSelected) &&
    props.timeRangeSelected.length == 2
  ) {
    searchingInfo["fromTransactionDate"] = moment(
      props.timeRangeSelected[0],
    ).toISOString();
    searchingInfo["toTransactionDate"] = moment(
      props.timeRangeSelected[1],
    ).toISOString();
  }
  transactionService
    .searchPaging(searchingInfo)
    .then((res) => {
      listTransaction.value = res.result.items;
      totalCount.value = res.result.totalCount;
    })
    .catch(() => {
      alert("Đã có lỗi xảy ra");
    });
};

// Tạo mới transaction
const createTransaction = () => {
  isOpenCOETransactionDialog.value = true;
  editTransactionId.value = null;
};

// Sửa transaction
const editTransaction = (id: number) => {
  isOpenCOETransactionDialog.value = true;
  editTransactionId.value = id;
};

// Xóa transaction
const deleteTransaction = (id: number) => {
  ElMessageBox.confirm(
    "Thao tác này sẽ xóa vĩnh viễn giao dịch. Bạn có muốn tiếp tục?",
    "Warning",
    {
      confirmButtonText: "OK",
      cancelButtonText: "Cancel",
      type: "warning",
    },
  )
    .then(() => {
      transactionService.delete(id).then((res) => {
        if (res.success) {
          getAllTransaction();
          ElMessage({
            type: "success",
            message: "Delete completed",
          });
        } else {
          ElMessage({
            type: "info",
            message: "Delete canceled",
          });
        }
      });
    })
    .catch(() => {
      ElMessage({
        type: "info",
        message: "Delete canceled",
      });
    });
};

const onCloseCOETransactionDialog = (isSuccess: boolean) => {
  isOpenCOETransactionDialog.value = false;
  if (isSuccess) {
    getAllTransaction();
    emits("reloadData");
  }
};
</script>

<style>
.t-buy {
  color: #42c442;
  background-color: #88d48833;
  padding: 8px;
  font-weight: 600;
}
.t-sell {
  color: red;
  background-color: #e2a6a659;
  padding: 8px;
  font-weight: 600;
}
</style>
