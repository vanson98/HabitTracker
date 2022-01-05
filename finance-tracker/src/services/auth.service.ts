import ajax from "@/lib/ajax";
import LoginModel from "@/models/LoginModel";
import Cookies from "js-cookie";
import AppConsts from "@/lib/appconst";

const authService = {
  // Login
  async login(data: LoginModel): Promise<any> {
    const res = await ajax.post("/api/TokenAuth/Authenticate", data);
    const tokenExpireDate = data.rememberClient
      ? new Date(new Date().getTime() + 1000 * res.data.result.expireInSeconds)
      : undefined;
    Cookies.set(
      AppConsts.authorization.tokenName,
      res.data.result.accessToken,
      tokenExpireDate,
    );
    Cookies.set(
      AppConsts.authorization.encrptedAuthTokenName,
      res.data.result.encryptedAccessToken,
      tokenExpireDate,
    );
  },
  // Get user info
  async getUserInfo(): Promise<any> {
    const res = await ajax.get(
      "/api/services/app/Session/GetCurrentLoginInformations",
    );
    return res;
  },
};

export default authService;
