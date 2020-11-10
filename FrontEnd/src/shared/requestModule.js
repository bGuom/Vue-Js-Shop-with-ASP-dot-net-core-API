import axios from 'axios';
import interceptorsSetup from "./interceptors";
import { handleResponseError } from "./handleResponseErrors";

export default function serviceCall() {
  interceptorsSetup();
  return {
    get: (url, data = {}, config) => {
      return new Promise(function (resolve, reject) {
        axios
          .get(url, data,config)
          .then(function (res) {
            resolve(res);
            return res;
          })
          .catch(function (err) {
            handleResponseError(reject, err);
            reject(err);
          });
      });
    },
    post: (url, data = {}, config) => {
      return new Promise(function (resolve, reject) {
        axios
          .post(url, data,config)
          .then(function (res) {
            resolve(res);
            return res;
          })
          .catch(function (err) {
            handleResponseError(reject, err);
            reject(err);
          });
      });
    },
    put: (url, data = {}, config) => {
      return new Promise(function (resolve, reject) {
        axios
          .put(url, data,config)
          .then(function (res) {
            resolve(res);
            return res;
          })
          .catch(function (err) {
            handleResponseError(reject, err);
            reject(err);
          });
      });
    },
    delete: (url, config) => {
      return new Promise(function (resolve, reject) {
        axios
          .delete(url,config)
          .then(function (res) {
            resolve(res);
            return res;
          })
          .catch(function (err) {
            handleResponseError(reject, err);
            reject(err);
          });
      });
    }
  };
};
