<template>
  <div>
    <!-- start control container -->
    <div class="flex flex-row space-x-4">
      <el-select
        placeholder="Chọn kênh đầu tư"
        @change="onChannelChange"
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
            {{ channel.moneyInput }}
          </td>
          <td style="padding-left: 20px">Tiền mặt thực có</td>
          <td class="float-right">
            {{ channel.moneyStock }}
          </td>
          <td style="padding-left: 20px">Tổng tiền rút ra</td>
          <td class="float-right">
            {{ channel.moneyOutput }}
          </td>
        </tr>
        <tr>
          <td>Tông giá giá trị CP (lúc mua)</td>
          <td class="float-right">
            {{ channel.valueOfStocks.toFixed(3) }}
          </td>
          <td style="padding-left: 20px">Tống giá trị thị trường (hiện tại)</td>
          <td class="float-right">
            {{ channel.marketValueOfStocks.toFixed(3) }}
          </td>
          <td style="padding-left: 20px">Lãi/lỗ</td>
          <td style="padding-left: 20px" class="float-right">
            {{
              (channel.marketValueOfStocks - channel.valueOfStocks).toFixed(3)
            }}
            <span v-if="channel.valueOfStocks > 0">
              - ({{
                (
                  ((channel.marketValueOfStocks - channel.valueOfStocks) *
                    100) /
                  channel.valueOfStocks
                ).toFixed(3)
              }}%)
            </span>
          </td>
        </tr>
        <tr>
          <td>Tài sản ròng (NAV)</td>
          <td class="float-right">
            {{ channel.nav.toFixed(3) }}
          </td>
          <td style="padding-left: 20px">Tổng phí mua / bán</td>
          <td class="float-right">
            {{ channel.totalBuyFee }} |
            {{ channel.totalSellFee }}
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
          @change="onSelectStockCodeChange()"
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
          @change="getAllTransaction(1)"
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
          @change="getAllInvestmentOverview(1)"
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
          @change="getAllTransaction(1)"
        ></el-date-picker>
      </div>
      <div class="mt-auto flex-grow">
        <el-button @click="resetSearching" type="primary">Reset</el-button>
      </div>
    </div>
    <!-- end search form -->

    <!--manage investment-->
    <ManageInvestment
      :totalCount="mi_totalCount"
      :listInvestment="mi_listInvestment"
      :pageSize="mi_pageSize"
      @onInvestmentPageChange="onInvestmentPageChange"
      @on-list-investment-change="init()"
    ></ManageInvestment>

    <!--TransactionComponent-->
    <TransactionComponent
      :list-transaction="mt_listTransaction"
      :page-size="mt_pageSize"
      :total-count="mt_totalCount"
      @on-list-transaction-updated="init()"
      @on-transaction-page-change="onTransactionPageChange"
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
import { onBeforeMount, onMounted, onUpdated, ref } from "vue";
import {
  SearchTransactionInputDto,
  SearchTransactionOutputDto,
  TransactionType,
} from "@/models/transaction/TransactionModels";
import {
  InvestmentOverviewDto,
  InvestmentSelectDto,
  InvestmentStatus,
} from "@/models/investment/InvestmentDtos";
import investmentService from "@/services/investment.service";
import util from "@/lib/util";
import { appStore } from "@/store/appStore";
import { storeToRefs } from "pinia";
import { ElMessage, ElMessageBox } from "element-plus";
import transactionService from "@/services/transaction.service";
import moment from "moment";
// =============== Data =====================
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
let s_investmentStatus = ref<number>(investmentStatusEnum.Active);
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

// investment data
let mi_listInvestment = ref<InvestmentOverviewDto[]>([]);
let mi_pageSize = 10;
let mi_totalCount = ref(0);

// transaction data
let mt_listTransaction = ref<SearchTransactionOutputDto[]>([]);
let mt_totalCount = ref(0);
const mt_pageSize = 10;

// ==================== Hook ====================
onMounted(() => {
  investmentChannelService.getAllChannel().then((res) => {
    listChannel.value = res.result;
    if (listChannel.value.length > 0) {
      channelIdSelected.value = listChannel.value[0].id;
      init();
    }
  });
});

// ================== Method ====================
// init
const init = () => {
  getAllInvestmentOverview(1);
  getAllTransaction(1);
  getChannelInfo();
};

// *********** channel method ***********

const onChannelChange = () => {
  init();
};

const getChannelInfo = () => {
  investmentChannelService
    .getChannelOverview(channelIdSelected.value)
    .then((res) => {
      console.log(res);
      store.setChannelInfo(res.result);
    });
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
        // channel.value.moneyInput = res.result.moneyInput;
        // channel.value.moneyStock = res.result.moneyStock;
        // channel.value.nav = channel.value.marketValueOfStocks
        getChannelInfo();
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
        getChannelInfo();
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

// ************ investment method ************

const onInvestmentPageChange = (page: number) => {
  getAllInvestmentOverview(page);
};

const getAllInvestmentOverview = (page: number) => {
  investmentService
    .getAllOverview(
      (page - 1) * mi_pageSize,
      mi_pageSize,
      channelIdSelected.value,
      s_investmentIds.value,
      s_investmentStatus.value,
    )
    .then((res) => {
      mi_listInvestment.value = res.result.items;
      mi_totalCount.value = res.result.totalCount;
    })
    .catch(() => {
      alert("Đã có lỗi xảy ra");
    });
};

// ********** transaction method **********

const onTransactionPageChange = (page: number) => {
  getAllTransaction(page);
};

const getAllTransaction = (page: number) => {
  var searchingInfo: SearchTransactionInputDto = {
    skipCount: (page - 1) * mt_pageSize,
    maxResultCount: mt_pageSize,
    transactionType: s_transactionType.value,
    investmentIds: s_investmentIds.value,
  };
  if (
    s_timeRangeSelect.value != null &&
    Array.isArray(s_timeRangeSelect.value) &&
    s_timeRangeSelect.value.length == 2
  ) {
    searchingInfo["fromTransactionDate"] = moment(
      s_timeRangeSelect.value[0],
    ).toISOString();
    searchingInfo["toTransactionDate"] = moment(
      s_timeRangeSelect.value[1],
    ).toISOString();
  }
  transactionService
    .searchPaging(searchingInfo)
    .then((res) => {
      mt_listTransaction.value = res.result.items;
      mt_totalCount.value = res.result.totalCount;
    })
    .catch(() => {
      alert("Đã có lỗi xảy ra");
    });
};

// ************ form search method ************

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

const onSelectStockCodeChange = () => {
  getAllInvestmentOverview(1);
  getAllTransaction(1);
};

const getAllInvestmentForSelect = (query: string) => {
  if (channelIdSelected.value != null) {
    investmentService
      .getAllForSelect(channelIdSelected.value, query)
      .then((res) => {
        listSelectInvestment.value = res.result;
      });
  }
};

const resetSearching = () => {
  s_investmentIds.value = [];
  s_investmentStatus.value = -1;
  s_transactionType.value = -1;
  s_timeRangeSelect.value = "";
  getAllInvestmentOverview(1);
  getAllTransaction(1);
};
</script>
