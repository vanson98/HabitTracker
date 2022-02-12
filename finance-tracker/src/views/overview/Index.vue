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
      <el-button
        type="primary"
        size="small"
        @click="() => updateChannel('add-money')"
        >Add Money</el-button
      >

      <el-button
        type="primary"
        size="small"
        @click="() => updateChannel('withdraw-money')"
        >Withdraw Money</el-button
      >

      <el-button
        type="primary"
        size="small"
        @click="() => updateChannel('update-buy-fee')"
        >Update Buy Fee</el-button
      >
      <el-button
        type="primary"
        size="small"
        @click="() => updateChannel('update-sell-fee')"
        >Update Sell Fee</el-button
      >
    </div>
    <!-- end control container -->

    <!-- start info container -->
    <div v-if="channel != null">
      <table class="w-full">
        <tr>
          <td>Tổng tiền nhập vào</td>
          <td class="float-right">
            {{ util.formatCurrency(channel.moneyInput * 1000) }}
          </td>
          <td style="padding-left: 20px">Tiền mặt thực có</td>
          <td class="float-right">
            {{ util.formatCurrency(channel.moneyStock * 1000) }}
          </td>
          <td style="padding-left: 20px">Tổng tiền rút ra</td>
          <td class="float-right">
            {{ util.formatCurrency(channel.moneyOutput * 1000) }}
          </td>
        </tr>
        <tr>
          <td>Tông giá giá trị CP (lúc mua)</td>
          <td class="float-right">
            {{ util.formatCurrency(channel.valueOfStocks * 1000) }}
          </td>
          <td style="padding-left: 20px">Tống giá trị thị trường (hiện tại)</td>
          <td class="float-right">
            {{ util.formatCurrency(channel.marketValueOfStocks * 1000) }}
          </td>
          <td style="padding-left: 20px">Lãi/lỗ</td>
          <td style="padding-left: 20px" class="float-right">
            {{
              util.formatCurrency(
                (channel.marketValueOfStocks - channel.valueOfStocks) * 1000,
              )
            }}
            <span v-if="channel.valueOfStocks > 0">
              - ({{
                (
                  ((channel.marketValueOfStocks - channel.valueOfStocks) *
                    100) /
                  channel.valueOfStocks
                ).toFixed(2)
              }}%)
            </span>
          </td>
        </tr>
        <tr>
          <td>Tài sản ròng (NAV)</td>
          <td class="float-right">
            {{ util.formatCurrency(channel.nav * 1000) }}
          </td>
          <td style="padding-left: 20px">Tổng phí mua / bán</td>
          <td class="float-right">
            {{ util.formatCurrency(channel.totalBuyFee * 1000) }} |
            {{ util.formatCurrency(channel.totalSellFee * 1000) }}
          </td>
          <td style="padding-left: 20px">Phí mua / bán</td>
          <td class="float-right">
            {{ channel.buyFee }}% | {{ channel.sellFee }}%
          </td>
        </tr>
      </table>
    </div>
    <!-- end info container -->

    <!-- start search form -->
    <div class="my-2 flex flex-row">
      <div class="flex flex-col mx-2 w-1/3">
        <label>Stock Code</label>
        <el-select
          v-model="s_investmentIds"
          placeholder="Nhập mã CP"
          multiple
          filterable
          remote
          reserve-keyword
          :remote-method="remoteMethod"
          :loading="getListInvestmentLoading"
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
          v-model="s_transactionType"
          placeholder="Chọn loại giao dịch"
        >
          <el-option :label="'ALL'" :value="-1"></el-option>
          <el-option
            v-for="keyEnum in transactionTypeEnumKey"
            :key="keyEnum"
            :label="keyEnum"
            :value="transactionTypeEnum[keyEnum]"
          ></el-option>
        </el-select>
      </div>
      <div class="flex flex-col mx-2">
        <label>Investment Status</label>
        <el-select
          v-model="s_investmentStatus"
          placeholder="Trạn thái investment"
        >
          <el-option :label="'ALL'" :value="-1"></el-option>
          <el-option
            v-for="keyEnum in investmentStatusEnumKey"
            :key="keyEnum"
            :label="keyEnum"
            :value="investmentStatusEnum[keyEnum]"
          ></el-option>
        </el-select>
      </div>
      <div class="flex flex-col mx-2">
        <label class="demonstration">Time Range</label>
        <el-date-picker
          v-model="s_timeRangeSelect"
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
    <ManageInvestment
      :isReload="reloadListInvestment"
      :investmentIds="s_investmentIds"
      :investmentStatus="s_investmentStatus"
    ></ManageInvestment>

    <!--TransactionComponent-->
    <TransactionComponent
      :investmentIds="s_investmentIds"
      :transactionType="s_transactionType"
      :timeRangeSelected="s_timeRangeSelect"
      :channelId="channel.id"
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
import { onBeforeMount, onUpdated, ref } from "vue";
import {
  SearchTransactionInputDto,
  TransactionType,
} from "@/models/transaction/TransactionModels";
import {
  InvestmentSelectDto,
  InvestmentStatus,
} from "@/models/investment/InvestmentDtos";
import investmentService from "@/services/investment.service";
import util from "@/lib/util";
import { appStore } from "@/store/appStore";
import { storeToRefs } from "pinia";
import { ElMessage, ElMessageBox } from "element-plus";
// enum
let investmentStatusEnum = InvestmentStatus;
let transactionTypeEnum = TransactionType;
let transactionTypeEnumKey = util.getEnumKeys(transactionTypeEnum);
let investmentStatusEnumKey = util.getEnumKeys(investmentStatusEnum);
// store
const store = appStore();
const { channel } = storeToRefs(store);
// channel data
let listChannel = ref<ChannelSellectDto[]>();
let channelIdSelected = ref<number>(0);
let updateAmountInfo = ref();

// search data
let s_investmentIds = ref<number[]>([]);
let s_timeRangeSelect = ref<string | Array<any>>("");
let s_transactionType = ref<number>(-1);
let s_investmentStatus = ref<number>(-1);

let listSelectInvestment = ref<InvestmentSelectDto[]>();
let getListInvestmentLoading = ref<boolean>(false);

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

let reloadListInvestment = ref<boolean>(false);

// Method
const init = () => {
  investmentChannelService.getAllChannel().then((res) => {
    listChannel.value = res.result;
    if (listChannel.value.length > 0) {
      channelIdSelected.value = listChannel.value[0].id;
      getChannelInfo();
      //getAllInvestmentForSelect();
    }
  });
};

// Hook
onBeforeMount(() => {
  init();
});

// Get Channel Info
const getChannelInfo = () => {
  investmentChannelService
    .getChannelOverview(channelIdSelected.value)
    .then((res) => {
      store.setChannelInfo(res.result);
    });
};

const remoteMethod = (query: string) => {
  if (query) {
    getListInvestmentLoading.value = true;
    setTimeout(() => {
      getListInvestmentLoading.value = false;
      getAllInvestmentForSelect(query);
    }, 200);
  } else {
    listSelectInvestment.value = [];
  }
};

// Get all investment for select
const getAllInvestmentForSelect = (query: string) => {
  if (channelIdSelected.value != null) {
    investmentService
      .getAllForSelect(channelIdSelected.value, query)
      .then((res) => {
        listSelectInvestment.value = res.result;
      });
  }
};

// reset searching transaction info
const resetSearching = () => {
  s_investmentIds.value = [];
  s_investmentStatus.value = -1;
  s_transactionType.value = -1;
  s_timeRangeSelect.value = "";
};

const updateChannel = (actionType: string) => {
  var messageAlert = "";
  if (actionType == "add-money") {
    messageAlert = "Bạn có chắc muốn thêm tiền?";
  } else if (actionType == "withdraw-money") {
    messageAlert = "Bạn có chắc muốn rút tiền?";
  } else if (actionType == "update-buy-fee") {
    messageAlert = "Bạn có chắc muốn cập nhật phí mua?";
  } else if (actionType == "update-sell-fee") {
    messageAlert = "Bạn có chắc muốn cập nhật phí bán?";
  }

  ElMessageBox.confirm(messageAlert, "Warning", {
    confirmButtonText: "OK",
    cancelButtonText: "Cancel",
    type: "warning",
  }).then(() => {
    if (actionType == "add-money") {
      addMoneyInput();
    } else if (actionType == "withdraw-money") {
      withdrawMoney();
    } else if (actionType == "update-buy-fee") {
      updateFee("BF");
    } else if (actionType == "update-sell-fee") {
      updateFee("SF");
    }
  });
};

const addMoneyInput = () => {
  if (channel.value != null && channel.value.channelCode) {
    investmentChannelService
      .addMoneyInput(channel.value?.channelCode, updateAmountInfo.value)
      .then((res) => {
        channel.value.moneyInput = res.result.moneyInput;
        channel.value.moneyStock = res.result.moneyStock;
        updateAmountInfo.value = null;
      })
      .catch(() => {
        ElMessage({
          type: "info",
          message: "Nộp tiền thất bại.",
        });
      });
  }
};

const withdrawMoney = () => {
  if (channel.value != null && channel.value.channelCode) {
    investmentChannelService
      .withdrawMoney(channel.value?.channelCode, updateAmountInfo.value)
      .then((res) => {
        channel.value.moneyOutput = res.result.moneyOutput;
        channel.value.moneyStock = res.result.moneyStock;
        updateAmountInfo.value = null;
      })
      .catch(() => {
        ElMessage({
          type: "info",
          message: "Rút tiền thất bại.",
        });
      });
  }
};

const updateFee = (type: string) => {
  if (channel.value != null) {
    investmentChannelService
      .updateFee(channel.value.id, type, updateAmountInfo.value)
      .then((res) => {
        if (type == "BF") {
          channel.value.buyFee = res.result.buyFee;
        } else {
          channel.value.sellFee = res.result.sellFee;
        }
        updateAmountInfo.value = null;
      })
      .catch(() => {
        ElMessage({
          type: "info",
          message: "Cập nhật phí thất bại",
        });
      });
  }
};

const reloadData = () => {
  getChannelInfo();
  reloadListInvestment.value = true;
};
</script>
