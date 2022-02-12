import { InvestmentChannelOverviewDto } from "@/models/investment-channel/InvestmentChannelModels";
import PermissionModel from "@/models/PermissionModel";
import UserModel from "@/models/SessionModel";
import { defineStore } from "pinia";

export const appStore = defineStore("appStore", {
  state: () => ({
    userInfo: {} as UserModel,
    listPermision: [] as Array<PermissionModel>,
    channel: {} as InvestmentChannelOverviewDto,
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
