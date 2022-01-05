<template>
  <header>
    <div class="container">
      <nav>
        <ul class="flex justify-center items-center">
          <li
            v-for="menuItem in listMenu"
            :key="menuItem.routerName"
            class="m-3"
          >
            <router-link :to="{ name: menuItem.routerName }">{{
              menuItem.displayName
            }}</router-link>
          </li>
        </ul>
      </nav>
    </div>
  </header>
</template>
<script lang="ts" setup>
import { MenuModel } from "@/models/MenuModel";

const listMenu: MenuModel[] = [
  {
    displayName: "Investment",
    routerName: "investment",
  },
  {
    displayName: "Overview",
    routerName: "overview",
  },
  {
    displayName: "Transactions",
    routerName: "transactions",
  },
];
</script>
