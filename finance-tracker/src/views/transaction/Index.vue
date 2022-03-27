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
              {{ (scope.row.numberOfShares * scope.row.price).toFixed(3) }}
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
        <el-table-column label="Tổng phí">
          <template #default="scope">
            <span>{{ scope.row.totalFee }}</span>
          </template>
        </el-table-column>
        <el-table-column label="Action" :align="'center'" width="180">
          <template #default="scope">
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
    <CreateTransactionDialog
      :isOpen="isOpenCreateTransactionDialog"
      @close="onCloseCreateTransactionDialog"
    ></CreateTransactionDialog>
  </div>
</template>
<script lang="ts" setup>
import { ref, defineProps, defineEmits, withDefaults } from "vue";
import CreateTransactionDialog from "./CreateTransactionDialog.vue";
import { ElButton, ElMessage, ElMessageBox } from "element-plus";
import { TransactionType } from "@/models/transaction/TransactionModels";

import transactionService from "@/services/transaction.service";
import { SearchTransactionOutputDto } from "@/models/transaction/TransactionModels";
import util from "@/lib/util";
import moment from "moment";

// ============== Props ================
const props = withDefaults(
  defineProps<{
    totalCount: number;
    pageSize: number;
    listTransaction: SearchTransactionOutputDto[];
  }>(),
  {},
);

// ============== Data ================
// enum
let transactionType = TransactionType;

// emit
const emits = defineEmits([
  "onListTransactionUpdated",
  "onTransactionPageChange",
]);

// page data
let currentPage = ref(1);

// dialog data
let isOpenCreateTransactionDialog = ref(false);

// =========== Method ===========
const pageChange = (page: number) => {
  currentPage.value = page;
  emits("onTransactionPageChange", page);
};

const createTransaction = () => {
  isOpenCreateTransactionDialog.value = true;
};

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
          emits("onListTransactionUpdated");
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

const onCloseCreateTransactionDialog = (isSuccess: boolean) => {
  isOpenCreateTransactionDialog.value = false;
  if (isSuccess) {
    emits("onListTransactionUpdated");
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
