import SessionDataModel from "@/models/SessionDataModel";

/* eslint-disable */
const util = {
  sessionData: {} as SessionDataModel,
  getRouterObjByName(routers: Array<any>, name?: string): any {
    if (!name || !routers || !routers.length) {
      return null;
    }
    let routerObj = null;
    for (const item of routers) {
      if (item.name === name) {
        return item;
      }
      routerObj = this.getRouterObjByName(item.children, name);
      if (routerObj) {
        return routerObj;
      }
    }
    return null;
  },
  formatCurrency(input: number): string {
    return input.toLocaleString("vi", {
      style: "currency",
      currency: "VND",
      maximumFractionDigits: 2,
    });
  },
  formatCurrencyNoSymbol(input: number): string {
    return input
      .toLocaleString("vi", {
        style: "currency",
        currency: "VND",
        maximumFractionDigits: 3,
      })
      .slice(0, -1);
  },
  getEnumKeys(input: object): string[] {
    return Object.keys(input).filter((k) => isNaN(Number(k)));
  },
};
export default util;
