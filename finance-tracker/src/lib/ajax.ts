import axios from "axios";
import Cookies from "js-cookie";
import AppConsts from "./appconst";

const ajax = axios.create({
  baseURL: AppConsts.remoteServiceBaseUrl,
  timeout: 300000,
});

// Interceptors Request
ajax.interceptors.request.use(function (config) {
  const authToken = Cookies.get(AppConsts.authorization.tokenName);
  if (authToken != "" && authToken != null) {
    config.headers.common["Authorization"] = "Bearer " + authToken;
  }
  return config;
});

// Interceptor Response
ajax.interceptors.response.use(
  (response) => {
    return response;
  },
  (error) => {
    if (
      !!error.response &&
      !!error.response.data.error &&
      !!error.response.data.error.message &&
      error.response.data.error.details
    ) {
      alert(error.response.data.error.message);
    } else if (
      !!error.response &&
      !!error.response.data.error &&
      !!error.response.data.error.message
    ) {
      alert(error.response.data.error.message);
    } else if (!error.response) {
      alert("UnknownError");
    }
    // setTimeout(() => {
    //   vm.$Message.destroy();
    // }, 1000);
    return Promise.reject(error);
  },
);

// Export
export default ajax;
