import { createPinia } from "pinia";
import { createApp, h, onMounted } from "vue";
import App from "./App.vue";
import ajax from "./lib/ajax";
import SessionModel from "./models/SessionModel";
import router from "./router";
import authService from "./services/auth.service";
import { appStore } from "./store/appStore";
import util from "./lib/util";
import "./assets/tailwind.css";
import ElementPlus from "element-plus";
import "element-plus/dist/index.css";

ajax.get("/AbpUserConfiguration/GetAll").then((res) => {
  util.sessionData.userId = res.data.result.session.userId;
  const app = createApp({
    render() {
      return h(App);
    },
    async onMounted() {
      const sessionStore = appStore();
      const res = await authService.getUserInfo();
      const userInfo = res.data.result.user;
      if (userInfo != null) {
        const sessionModel: SessionModel = {
          userId: userInfo.id,
          email: userInfo.emailAddress,
          userName: userInfo.userName,
        };
        sessionStore.setUserInfo(sessionModel);
      } else {
        sessionStore.setUserInfo({
          userId: undefined,
          email: undefined,
          userName: undefined,
        });
      }
    },
  });
  app.use(ElementPlus);
  app.use(createPinia());
  app.use(router);
  app.mount("#app");
});
