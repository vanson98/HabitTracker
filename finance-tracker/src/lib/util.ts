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
};
export default util;
