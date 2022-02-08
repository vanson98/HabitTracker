<template>
  <div>
    <!-- start control container -->
    <div class="flex flex-row space-x-4">
      <el-select
        placeholder="Chọn kênh đầu tư"
        @change="getAllInvestmentForSelect"
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
      <el-button type="primary" size="small" @click="addMoneyInput"
        >Add Money</el-button
      >
      <el-button type="primary" size="small" @click="withdrawMoney"
        >Withdraw Money</el-button
      >
      <el-button type="primary" size="small" @click="updateFee('BF')"
        >Update Buy Fee</el-button
      >
      <el-button type="primary" size="small" @click="updateFee('SF')"
        >Update Sell Fee</el-button
      >
    </div>
    <!-- end control container -->

    <!-- start info container -->
    <div v-if="channelInfoData != null">
      <table class="w-full">
        <tr>
          <td>Tổng tiền nhập vào</td>
          <td class="float-right">
            {{ util.formatCurrency(channelInfoData.moneyInput * 1000) }}
          </td>
          <td style="padding-left: 20px">Tiền mặt thực có</td>
          <td class="float-right">
            {{ util.formatCurrency(channelInfoData.moneyStock * 1000) }}
          </td>
          <td style="padding-left: 20px">Tổng tiền rút ra</td>
          <td class="float-right">
            {{ util.formatCurrency(channelInfoData.moneyOutput * 1000) }}
          </td>
        </tr>
        <tr>
          <td>Tông giá giá trị CP (lúc mua)</td>
          <td class="float-right">
            {{ util.formatCurrency(channelInfoData.valueOfStocks * 1000) }}
          </td>
          <td style="padding-left: 20px">Tống giá trị thị trường (hiện tại)</td>
          <td class="float-right">
            {{
              util.formatCurrency(channelInfoData.marketValueOfStocks * 1000)
            }}
          </td>
          <td style="padding-left: 20px">Lãi/lỗ</td>
          <td style="padding-left: 20px" class="float-right">
            {{
              util.formatCurrency(
                (channelInfoData.marketValueOfStocks -
                  channelInfoData.valueOfStocks) *
                  1000,
              )
            }}
            <span v-if="channelInfoData.valueOfStocks > 0">
              - ({{
                (
                  ((channelInfoData.marketValueOfStocks -
                    channelInfoData.valueOfStocks) *
                    100) /
                  channelInfoData.valueOfStocks
                ).toFixed(2)
              }}%)
            </span>
          </td>
        </tr>
        <tr>
          <td>Tài sản ròng (NAV)</td>
          <td class="float-right">
            {{ util.formatCurrency(channelInfoData.nav * 1000) }}
          </td>
          <td style="padding-left: 20px">Tổng phí mua / bán</td>
          <td class="float-right">
            {{ util.formatCurrency(channelInfoData.totalBuyFee * 1000) }} |
            {{ util.formatCurrency(channelInfoData.totalSellFee * 1000) }}
          </td>
          <td style="padding-left: 20px">Phí mua / bán</td>
          <td class="float-right">
            {{ channelInfoData.buyFee }}% | {{ channelInfoData.sellFee }}%
          </td>
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
        <el-button @click="resetSearching" type="primary">Reset</el-button>
      </div>
    </div>
    <!-- end search form -->

    <!--manage investment-->
    <ManageInvestment :isReload="reloadListInvestment"></ManageInvestment>

    <!--TransactionComponent-->
    <TransactionComponent
      :investmentIdSelected="searchingInfo.investmentId"
      :transactionType="searchingInfo.transactionType"
      :timeRangeSelected="timeRangeSelect"
      :channelId="channelInfoData.id"
      @reloadData="reloadData"
    ></TransactionComponent>
  </div>
</template>
<script lang="ts" setup>
import TransactionComponent from "../transaction/Index.vue";
import ManageInvestment from "../investment/ManageInvestment.vue";
import investmentChannelService from "@/services/investment-channel.service";
import {
  ChannelSellectDto,
  InvestmentChannelOverviewDto,
} from "@/models/investment-channel/InvestmentChannelModels";
import { onBeforeMount, ref } from "vue";
import {
  SearchTransactionInputDto,
  TransactionType,
} from "@/models/transaction/TransactionModels";
import { InvestmentSelectDto } from "@/models/investment/InvestmentDtos";
import investmentService from "@/services/investment.service";
import util from "@/lib/util";

// channel data
let listChannel = ref<ChannelSellectDto[]>();
let channelIdSelected = ref<number>(0);
let updateAmountInfo = ref(0);
let channelInfoData = ref<InvestmentChannelOverviewDto>({
  id: 0,
  buyFee: 0,
  changnelName: "",
  channelCode: "",
  marketValueOfStocks: 0,
  moneyInput: 0,
  moneyOutput: 0,
  moneyStock: 0,
  nav: 0,
  sellFee: 0,
  totalBuyFee: 0,
  totalSellFee: 0,
  valueOfStocks: 0,
});

// investment data
let listSelectInvestment = ref<InvestmentSelectDto[]>();
let transactionType = TransactionType;
let transactionTypeEnumKey = util.getEnumKeys(transactionType);
let searchingInfo = ref<SearchTransactionInputDto>({
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
let timeRangeSelect = ref<string | Array<any>>("");
let reloadListInvestment = ref<boolean>(false);

const init = () => {
  investmentChannelService.getAllChannel().then((res) => {
    listChannel.value = res.result;
    if (listChannel.value.length > 0) {
      channelIdSelected.value = listChannel.value[0].id;
      getChannelInfo();
      getAllInvestmentForSelect();
    }
  });
};

// Event
onBeforeMount(() => {
  init();
});

// Get Channel Info
const getChannelInfo = () => {
  investmentChannelService
    .getChannelOverview(channelIdSelected.value)
    .then((res) => {
      channelInfoData.value = res.result;
    });
};

// Get all investment for select
const getAllInvestmentForSelect = () => {
  if (channelIdSelected.value != null) {
    investmentService.getAllForSelect(channelIdSelected.value).then((res) => {
      listSelectInvestment.value = res.result;
    });
  }
};

// reset searching transaction info
const resetSearching = () => {
  searchingInfo.value = {
    maxResultCount: 10,
    skipCount: 0,
    transactionType: -1,
  };
  timeRangeSelect.value = "";
};

const addMoneyInput = () => {
  if (channelInfoData.value != null && channelInfoData.value.channelCode) {
    investmentChannelService
      .addMoneyInput(channelInfoData.value?.channelCode, updateAmountInfo.value)
      .then((res) => {
        channelInfoData.value.moneyInput = res.result.moneyInput;
        channelInfoData.value.moneyStock = res.result.moneyStock;
      });
    updateAmountInfo.value = 0;
  }
};

const withdrawMoney = () => {
  if (channelInfoData.value != null && channelInfoData.value.channelCode) {
    investmentChannelService
      .withdrawMoney(channelInfoData.value?.channelCode, updateAmountInfo.value)
      .then((res) => {
        channelInfoData.value.moneyOutput = res.result.moneyOutput;
        channelInfoData.value.moneyStock = res.result.moneyStock;
      });
    updateAmountInfo.value = 0;
  }
};

const updateFee = (type: string) => {
  if (channelInfoData.value != null) {
    investmentChannelService
      .updateFee(channelInfoData.value.id, type, updateAmountInfo.value)
      .then((res) => {
        if (type == "BF") {
          channelInfoData.value.buyFee = res.result.buyFee;
        } else {
          channelInfoData.value.sellFee = res.result.sellFee;
        }
      });
    updateAmountInfo.value = 0;
  }
};

const reloadData = () => {
  getChannelInfo();
  reloadListInvestment.value = true;
};
</script>
