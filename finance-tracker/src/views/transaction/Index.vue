<template>
  <div>
    <!-- start control container -->
    <div class="flex flex-row space-x-4">
      <el-select
        placeholder="Chọn kênh đầu tư"
        @change="getAllTransaction"
        v-model="channelIdSelected"
        size="small"
      >
        <el-option
          v-for="channel in listChannel"
          :key="channel.id"
          :label="channel.code + ' - ' + channel.name"
          :value="channel.id"
        ></el-option>
      </el-select>
      <el-input
        placeholder="Nhập số liệu"
        v-model="updateAmountInfo"
        :type="'number'"
        style="width: 200px"
        size="small"
      ></el-input>
      <el-button type="primary" size="small">Add Money</el-button>
      <el-button type="primary" size="small">Withdraw Money</el-button>
      <el-button type="primary" size="small">Update Buy Fee</el-button>
      <el-button type="primary" size="small">Update Sell Fee</el-button>
    </div>
    <!-- end control container -->

    <!-- start info container -->
    <div>
      <table class="w-full">
        <tr>
          <td>Tổng tiền nhập vào</td>
          <td class="float-left">12</td>
          <td>Tiền mặt thực có</td>
          <td>10</td>
          <td>Tổng tiền rút ra</td>
          <td>13</td>
        </tr>
        <tr>
          <td>Tông giá giá trị CP (lúc mua)</td>
          <td class="float-left">12</td>
          <td>Tống giá trị thị trường (hiện tại)</td>
          <td>10</td>
          <td>Lãi/lỗ</td>
          <td>13</td>
        </tr>
        <tr>
          <td>Tài sản ròng (NAV)</td>
          <td>13</td>
          <td>Tổng phí mua / bán</td>
          <td>10</td>
          <td>Phí bán / mua</td>
          <td>13</td>
        </tr>
      </table>
    </div>
    <!-- end info container -->
    <ManageInvestment></ManageInvestment>
    <!-- start search form -->
    <div class="my-2 flex flex-row">
      <div class="flex flex-col mx-2 w-1/4">
        <label>Stock Code</label>
        <el-select
          v-model="searchingInfo.investmentId"
          placeholder="Nhập mã CP"
          @change="getAllTransaction"
        >
          <el-option
            v-for="item in listInvestment"
            :key="item.id"
            :value="item.id"
            :label="item.stockCode + ' - ' + item.companyName"
          ></el-option>
        </el-select>
      </div>
      <div class="flex flex-col mx-2">
        <label>Transaction Type</label>
        <el-select
          v-model="searchingInfo.transactionType"
          placeholder="Chọn loại giao dịch"
          @change="getAllTransaction"
        >
          <el-option :label="'ALL'" :value="-1"></el-option>
          <el-option
            v-for="keyEnum in transactionTypeEnumKey"
            :key="keyEnum"
            :label="keyEnum"
            :value="transactionType[keyEnum]"
          ></el-option>
        </el-select>
      </div>
      <div class="flex flex-col mx-2">
        <label class="demonstration">Time Range</label>
        <el-date-picker
          v-model="timeRangeSelect"
          type="daterange"
          unlink-panels
          range-separator="To"
          start-placeholder="Start date"
          end-placeholder="End date"
          :shortcuts="defaultTimeRange"
          @change="getAllTransaction"
        ></el-date-picker>
      </div>
      <div class="mt-auto flex-grow">
        <el-button @click="resetSearching" type="primary">Reset</el-button>
        <el-button @click="createTransaction" class="float-right" type="primary"
          >Add Transaction</el-button
        >
      </div>
    </div>
    <!-- end search form -->

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
    ></COETransactionDialog>
  </div>
</template>
<script lang="ts" setup>
import { onBeforeMount, ref } from "vue";
import COETransactionDialog from "./COETransactionDialog.vue";
import { ElButton, ElMessage, ElMessageBox } from "element-plus";
import TransactionDto, {
  SearchTransactionInputDto,
  TransactionType,
} from "@/models/transaction/TransactionModels";
import financeService from "@/services/investment.service";
import transactionService from "@/services/transaction.service";
import investmentChannelService from "@/services/investment-channel.service";
import { SearchTransactionOutputDto } from "@/models/transaction/TransactionModels";
import { InvestmentSelectDto } from "@/models/investment/InvestmentDtos";
import util from "@/lib/util";
import moment from "moment";
import { ChannelSellectDto } from "@/models/investment-channel/InvestmentChannelModels";
import AddOrWithdrawMoney from "./AddOrWithdrawMoneyDialog.vue";
import AddOrWithdrawMoneyDialog from "./AddOrWithdrawMoneyDialog.vue";
import ManageInvestment from "../investment/ManageInvestment.vue";

// page data
const pageSize = 10;
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
let listChannel = ref<ChannelSellectDto[]>();
let channelIdSelected = ref();
let currentPage = ref(1);
let totalCount = ref(0);
let listTransaction = ref<SearchTransactionOutputDto[]>();
let listInvestment = ref<InvestmentSelectDto[]>();
let searchingInfo = ref<SearchTransactionInputDto>({
  maxResultCount: pageSize,
  skipCount: 0,
  transactionType: -1,
});
let timeRangeSelect = ref("");
let transactionType = TransactionType;
let transactionTypeEnumKey = util.getEnumKeys(transactionType);
let updateAmountInfo = ref();
// dialog data
let isOpenCOETransactionDialog = ref(false);
let isOpenAddOrWithdrawMoneyDialog = ref(false);
let editTransactionId = ref<number | null>(null);

const init = () => {
  investmentChannelService.getAllChannel().then((res) => {
    listChannel.value = res.result;
    if (listChannel.value.length > 0) {
      channelIdSelected.value = listChannel.value[0].id;
      getAllInvestmentChannelInfo();
      getAllInvestment();
      getAllTransaction();
    }
  });
};

// Event
onBeforeMount(() => {
  init();
});

// reset searching transaction info
const resetSearching = () => {
  searchingInfo.value = {
    maxResultCount: pageSize,
    skipCount: 0,
    transactionType: -1,
  };
  timeRangeSelect.value = "";
  getAllTransaction();
};

// Get all investment channel info
const getAllInvestmentChannelInfo = () => {
  investmentChannelService.getAllChannel().then((res) => {
    listChannel.value = res.result;
  });
};

// Page change
const pageChange = (page: number) => {
  currentPage.value = page;
  getAllTransaction();
};

// Get All Transaction
const getAllTransaction = () => {
  if (timeRangeSelect.value != "" && timeRangeSelect.value.length == 2) {
    searchingInfo.value.fromTransactionDate = moment(
      timeRangeSelect.value[0],
    ).toISOString();
    searchingInfo.value.toTransactionDate = moment(
      timeRangeSelect.value[1],
    ).toISOString();
  }
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
    listInvestment.value = res.result;
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
  }
};

const onCloseAddOrWithDrawMoneyDialog = (isSuccess: boolean) => {
  isOpenAddOrWithdrawMoneyDialog.value = false;
  if (isSuccess) {
    getAllInvestmentChannelInfo();
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
