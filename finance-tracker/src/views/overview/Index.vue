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

    <!-- start search form -->
    <div class="my-2 flex flex-row">
      <div class="flex flex-col mx-2 w-1/4">
        <label>Stock Code</label>
        <el-select
          v-model="searchingInfo.investmentId"
          placeholder="Nhập mã CP"
        >
          <el-option
            v-for="item in listSelectInvestment"
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
        ></el-date-picker>
      </div>
      <div class="mt-auto flex-grow">
        <el-button @click="createInvestment">Create Investment</el-button>
        <el-button @click="resetSearching" type="primary">Reset</el-button>
        <el-button @click="createTransaction" class="float-right" type="primary"
          >Add Transaction</el-button
        >
      </div>
    </div>
    <!-- end search form -->

    <!--manage investment-->
    <ManageInvestment></ManageInvestment>

    <!--TransactionComponent-->
    <TransactionComponent
      :investmentIdSelected="searchingInfo.investmentId"
      :transactionType="searchingInfo.transactionType"
      :timeRangeSelected="timeRangeSelect"
    ></TransactionComponent>
  </div>
</template>
<script lang="ts" setup>
import TransactionComponent from "../transaction/Index.vue";
import investmentChannelService from "@/services/investment-channel.service";
import { ChannelSellectDto } from "@/models/investment-channel/InvestmentChannelModels";
import { onBeforeMount, ref } from "vue";
import {
  SearchTransactionInputDto,
  TransactionType,
} from "@/models/transaction/TransactionModels";
import { InvestmentSelectDto } from "@/models/investment/InvestmentDtos";
import investmentService from "@/services/investment.service";
import util from "@/lib/util";

// Channel data
let listChannel = ref<ChannelSellectDto[]>();
let channelIdSelected = ref();
// Search investment data
let listSelectInvestment = ref<InvestmentSelectDto[]>();
let transactionType = TransactionType;
let transactionTypeEnumKey = util.getEnumKeys(transactionType);
let searchingInfo = ref<SearchTransactionInputDto>({
  investmentId: null,
  maxResultCount: 10,
  skipCount: 0,
  transactionType: -1,
});
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
let timeRangeSelect = ref("");

const init = () => {
  investmentChannelService.getAllChannel().then((res) => {
    listChannel.value = res.result;
    if (listChannel.value.length > 0) {
      channelIdSelected.value = listChannel.value[0].id;
      getAllInvestmentChannelInfo();
      getAllInvestmentForSelect();
    }
  });
};

// Event
onBeforeMount(() => {
  init();
});

// Methods
const getAllInvestmentChannelInfo = () => {
  investmentChannelService.getAllChannel().then((res) => {
    listChannel.value = res.result;
  });
};

// Get all investment for select
const getAllInvestmentForSelect = () => {
  investmentService.getAllForSelect().then((res) => {
    listSelectInvestment.value = res.result;
  });
};

// reset searching transaction info
const resetSearching = () => {
  searchingInfo.value = {
    investmentId: null,
    maxResultCount: 10,
    skipCount: 0,
    transactionType: -1,
  };
  timeRangeSelect.value = "";
};
</script>
