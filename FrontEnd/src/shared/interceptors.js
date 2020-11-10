import Vue from 'vue';
import axios from 'axios';
import VueCookies from "vue-cookies";

Vue.use(VueCookies);

export default function setup() {
  axios.interceptors.request.use(function (config) {
    const token = $cookies.get("token");
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  }, function (err) {
    return Promise.reject(err);
  });

  axios.interceptors.response.use(function (response) {
    return response;
  }, function (error) {
    return Promise.reject(error);
  });
}
