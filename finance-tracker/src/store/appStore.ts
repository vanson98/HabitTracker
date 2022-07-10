import { InvestmentChannelOverviewDto } from "@/models/investment-channel/InvestmentChannelModels";
import PermissionModel from "@/models/PermissionModel";
import UserModel from "@/models/SessionModel";
import { defineStore } from "pinia";

export const appStore = defineStore("appStore", {
  state: () => ({
    userInfo: {} as UserModel,
    listPermision: [] as Array<PermissionModel>,
    channel: {
      buyFee: 0,
      marketValueOfStocks: 0,
      moneyInput: 0,
      moneyOutput: 0,
      totalBuyFee: 0,
      totalSellFee: 0,
      moneyStock: 0,
      nav: 0,
      sellFee: 0,
      valueOfStocks: 0,
    } as InvestmentChannelOverviewDto,
  }),
  getters: {
    getUserInfo(state) {
      return state.userInfo;
    },
    getChannelInfo(state) {
      return state.channel;
    },
  },
  actions: {
    setUserInfo(userInfo: UserModel) {
      this.userInfo = userInfo;
    },
    setChannelInfo(input: InvestmentChannelOverviewDto) {
      this.channel = input;
    },
  },
});
