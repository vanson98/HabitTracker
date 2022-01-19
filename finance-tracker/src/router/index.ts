import { createRouter, createWebHistory, RouteRecordRaw } from "vue-router";
import Layout from "../views/Layout.vue";
import util from "@/lib/util";

const routes: Array<RouteRecordRaw> = [
  {
    path: "/",
    name: "login",
    meta: {
      title: "LogIn",
    },
    component: () => import("../views/Login.vue"),
  },
  {
    path: "/manage",
    name: "manage",
    component: Layout,
    children: [
      {
        path: "overview",
        name: "overview",
        component: () => import("../views/investment/ManageInvestment.vue"),
      },
      {
        path: "investment",
        name: "investment",
        component: () => import("../views/investment/ManageInvestment.vue"),
      },
      {
        path: "transactions",
        name: "transactions",
        component: () => import("../views/transaction/Index.vue"),
      },
    ],
  },
];

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes,
});

router.beforeEach((to, from, next) => {
  if (util.sessionData.userId == null && to.name != "login") {
    // TH chưa đăng nhập hoặc token hết hạn
    next({
      name: "login",
    });
  } else if (to.name == "login" && util.sessionData.userId != null) {
    // TH user đã đăng nhập
    next({
      name: "transactions",
    });
  } else {
    // check quyền...
    next();
  }
});
export default router;
