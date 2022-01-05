import PermissionModel from "@/models/PermissionModel";
import UserModel from "@/models/SessionModel";
import { defineStore } from "pinia";

export const appStore = defineStore("appStore", {
  state: () => ({
    userInfo: {} as UserModel,
    listPermision: [] as Array<PermissionModel>,
  }),
  getters: {
    getUserInfo(state) {
      return state.userInfo;
    },
  },
  actions: {
    setUserInfo(userInfo: UserModel) {
      this.userInfo = userInfo;
    },
  },
});
