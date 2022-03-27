<template>
  <div>
    <div>
      <el-button @click="createInvestment">Create Investment</el-button>
    </div>
    <div class="table-ctn">
      <el-table
        :data="props.listInvestment"
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
          width="110"
        ></el-table-column>
        <el-table-column prop="totalMoneyBuy" label="Money Bought (x1000)">
          <template #default="scope">
            <span>
              {{ scope.row.totalMoneyBuy }}
            </span>
          </template>
        </el-table-column>
        <el-table-column
          prop="capitalCost"
          label="Capital Cost"
          width="120"
          :class-name="'mid-col-highlight'"
        >
          <template #default="scope">{{
            scope.row.capitalCost.toFixed(3)
          }}</template>
        </el-table-column>
        <el-table-column
          prop="vol"
          label="Vol"
          width="100"
          :class-name="'mid-col-highlight'"
        ></el-table-column>
        <el-table-column
          label="Market Price"
          width="180"
          :class-name="'mid-col-highlight'"
        >
          <template #default="scope">
            <div>
              <div class="mk-p-ctn">
                <span>{{ scope.row.marketPrice }}</span>
                <span>
                  {{ (scope.row.marketPrice * scope.row.vol).toFixed(3) }}
                </span>
              </div>
              <div style="text-align: right">
                <span
                  :class="{
                    interest: scope.row.gainLossPercent > 0,
                    loss: scope.row.gainLossPercent < 0,
                  }"
                >
                  {{ scope.row.gainLossValue.toFixed(2) }} ({{
                    scope.row.gainLossPercent.toFixed(2)
                  }}%)
                </span>
              </div>
            </div>
          </template>
        </el-table-column>
        <el-table-column prop="totalMoneySell" label="Money Sold">
          <template #default="scope">
            <span>
              {{ scope.row.totalMoneySell }}
            </span>
            <div>
              <span
                v-if="scope.row.status == InvestmentStatus.BuyOut"
                :class="{
                  interest: scope.row.sellInterest > 0,
                  loss: scope.row.sellInterest < 0,
                }"
              >
                {{ scope.row.sellInterest }} ({{
                  scope.row.sellInterestPercent.toFixed(2)
                }}%)
              </span>
            </div>
          </template>
        </el-table-column>
        <el-table-column
          prop="totalAmountSell"
          label="Amount Sell"
          width="110"
        ></el-table-column>
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
              @click="deleteInvestment(scope.row.id)"
              >Delete</el-button
            >
          </template>
        </el-table-column>
      </el-table>
      <div class="text-center my-6">
        <el-pagination
          layout="prev, pager, next"
          :total="props.totalCount"
          :pageSize="props.pageSize"
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
import { ref, defineProps, withDefaults, defineEmits } from "vue";
import COEInvestmentDialog from "./COEInvestmentDialog.vue";
import { ElButton, ElMessage, ElMessageBox } from "element-plus";
import InvestmentDto, {
  InvestmentOverviewDto,
} from "@/models/investment/InvestmentDtos";
import financeService from "@/services/investment.service";
import { InvestmentStatus } from "@/models/investment/InvestmentDtos";
import util from "@/lib/util";
import investmentService from "@/services/investment.service";

//props
const props = withDefaults(
  defineProps<{
    totalCount: number;
    pageSize: number;
    listInvestment: InvestmentOverviewDto[];
  }>(),
  {},
);
// emit
const emits = defineEmits(["onInvestmentPageChange", "onListInvestmentChange"]);
// data
let currentPage = ref(1);
let isOpenDialog = ref(false);
let editInvestmentId = ref<number | null>(null);
//method
const pageChange = (page: number) => {
  currentPage.value = page;
  emits("onInvestmentPageChange", page);
};

const createInvestment = () => {
  isOpenDialog.value = true;
  editInvestmentId.value = null;
};

const editInvestment = (id: number) => {
  isOpenDialog.value = true;
  editInvestmentId.value = id;
};

const deleteInvestment = (id: number) => {
  debugger;
  ElMessageBox.confirm(
    "Thao tác này sẽ xóa vĩnh viễn danh mục. Bạn có muốn tiếp tục?",
    "Warning",
    {
      confirmButtonText: "OK",
      cancelButtonText: "Cancel",
      type: "warning",
    },
  )
    .then(() => {
      investmentService.delete(id).then((res) => {
        if (res.success) {
          debugger;
          emits("onListInvestmentChange");
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

const closeDialog = (isSuccess: boolean) => {
  isOpenDialog.value = false;
  if (isSuccess) {
    emits("onListInvestmentChange");
  }
};
</script>
<style>
.mid-col-highlight {
  background-color: #e0e0e0bf !important;
}
.loss {
  color: red;
  font-weight: 600;
}
.interest {
  color: #01c801;
  font-weight: 600;
}
.mk-p-ctn {
  display: flex;
  justify-content: space-between;
}
</style>
