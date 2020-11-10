import  Vuex from 'vuex';
import Vue from 'vue';
import Auth from './modules/auth';
import Data from './modules/data';

//Load Vuex
Vue.use(Vuex);

//Create Store

export default new Vuex.Store({
  modules:{
    Auth,
    Data
  }
});


