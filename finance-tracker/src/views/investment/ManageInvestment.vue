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
          label="Mã CK"
          width="150"
        ></el-table-column>
        <el-table-column
          prop="companyName"
          label="Tên công ty"
        ></el-table-column>
        <el-table-column
          prop="totalBuy"
          label="Tổng số lượng mua"
        ></el-table-column>
        <el-table-column
          prop="totalSell"
          label="Tổng số lượng bán"
        ></el-table-column>
        <el-table-column
          prop="currentPrice"
          label="Giá hiện tại"
        ></el-table-column>
        <el-table-column prop="vol" label="Số lượng hiện tại"></el-table-column>
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
import Investment from "@/models/investment/Investment";
import financeService from "@/services/finance.service";

const pageSize = 10;
let currentPage = ref(1);
let totalCount = ref(0);

let isOpenDialog = ref(false);
let editInvestmentId = ref<number | null>(null);

let listInvestment = ref<Investment[]>();
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
