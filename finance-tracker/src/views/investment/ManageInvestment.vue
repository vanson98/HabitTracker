<template>
  <div>
    <div class="my-2">
      <el-button @click="createInvestment">Create Investment</el-button>
    </div>
    <div class="table-ctn">
      <el-table
        :data="listInvestment"
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
          label="Symbol"
          width="100"
        ></el-table-column>
        <el-table-column
          prop="totalAmountBuy"
          label="Amount Buy"
        ></el-table-column>
        <el-table-column prop="totalMoneyBuy" label="Money Bought">
          <template #default="scope">
            <span>
              {{ util.formatCurrency(scope.row.totalMoneyBuy * 1000) }}
            </span>
          </template>
        </el-table-column>
        <el-table-column
          prop="capitalCost"
          label="Capital Cost"
          width="120"
        ></el-table-column>
        <el-table-column prop="vol" label="Vol" width="100"> </el-table-column>
        <el-table-column
          prop="marketPrice"
          label="Market Price"
          width="120"
        ></el-table-column>
        <el-table-column
          prop="totalAmountSell"
          label="Amount Sell"
        ></el-table-column>
        <el-table-column prop="totalMoneySell" label="Money Sold">
          <template #default="scope">
            <span>
              {{ util.formatCurrency(scope.row.totalMoneySell * 1000) }}
            </span>
          </template>
        </el-table-column>
        <el-table-column prop="status" label="Status" width="80">
          <template #default="scope">
            <el-tag
              v-if="scope.row.status == InvestmentStatus.NotActive"
              class="ml-2"
              type="info"
              effect="dark"
              >NA</el-tag
            >
            <el-tag
              v-if="scope.row.status == InvestmentStatus.Active"
              class="ml-2"
              type="success"
              effect="dark"
              >AT</el-tag
            >
            <el-tag
              v-if="scope.row.status == InvestmentStatus.BuyOut"
              class="ml-2"
              type="warning"
              effect="dark"
              >BO</el-tag
            >
          </template>
        </el-table-column>
        <el-table-column label="Action" :align="'center'" width="180">
          <template #default="scope">
            <el-button size="small" @click="editInvestment(scope.row.id)"
              >Edit</el-button
            >
            <el-button
              size="small"
              type="danger"
              @click="deleteInvestment(scope.row)"
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
    <COEInvestmentDialog
      :isOpen="isOpenDialog"
      :editInvestmentId="editInvestmentId"
      @close="closeDialog"
    ></COEInvestmentDialog>
  </div>
</template>
<script lang="ts" setup>
import { onMounted, ref } from "vue";
import COEInvestmentDialog from "./COEInvestmentDialog.vue";
import { ElButton } from "element-plus";
import InvestmentDto from "@/models/investment/InvestmentDtos";
import financeService from "@/services/investment.service";
import { InvestmentStatus } from "@/models/investment/InvestmentDtos";
import util from "@/lib/util";
const pageSize = 10;
let currentPage = ref(1);
let totalCount = ref(0);

let isOpenDialog = ref(false);
let editInvestmentId = ref<number | null>(null);

let listInvestment = ref<InvestmentDto[]>();
// event
onMounted(() => {
  getAllInvestment();
});

const pageChange = (page: number) => {
  currentPage.value = page;
  getAllInvestment();
};

//method
const getAllInvestment = () => {
  financeService
    .getAllPaginInvestment((currentPage.value - 1) * pageSize, pageSize)
    .then((res) => {
      listInvestment.value = res.result.items;
      totalCount.value = res.result.totalCount;
    })
    .catch(() => {
      alert("Đã có lỗi xảy ra");
    });
};

const createInvestment = () => {
  isOpenDialog.value = true;
  editInvestmentId.value = null;
};

const editInvestment = (id: number) => {
  isOpenDialog.value = true;
  editInvestmentId.value = id;
};

const deleteInvestment = (row: any) => {
  console.log("delete");
};

const closeDialog = (isSuccess: boolean) => {
  isOpenDialog.value = false;
  if (isSuccess) {
    getAllInvestment();
  }
};
</script>
<style></style>
