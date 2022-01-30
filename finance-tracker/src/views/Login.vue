<template>
  <div class="login-container">
    <form>
      <div>
        <input
          type="text"
          v-model="loginModel.userNameOrEmailAddress"
          placeholder="username"
        />
      </div>
      <div>
        <input
          type="password"
          placeholder="password"
          v-model="loginModel.password"
        />
      </div>
      <div>
        <button @click.prevent="login()">Login</button>
      </div>
    </form>
  </div>
</template>
<script lang="ts" setup>
import router from "@/router";
import { computed, ref } from "vue";
import authService from "@/services/auth.service";
import LoginModel from "@/models/LoginModel";

let loginModel = ref<LoginModel>({
  userNameOrEmailAddress: null,
  password: null,
  rememberClient: false,
});

async function login() {
  await authService.login(loginModel.value);
  location.reload();
}
</script>
