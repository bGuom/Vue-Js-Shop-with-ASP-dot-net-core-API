// The Vue build version to load with the `import` command
// (runtime-only or standalone) has been set in webpack.base.conf with an alias.
import 'core-js/es6/promise'
import 'core-js/es6/string'
import 'core-js/es7/array'
import store from './store'
// import cssVars from 'css-vars-ponyfill'
import Vue from 'vue'
import BootstrapVue from 'bootstrap-vue'
import App from './App'
import routes from './router'
import VueRouter from 'vue-router'
import Vuelidate from 'vuelidate'
import {VueMasonryPlugin} from 'vue-masonry'
import StarRating from 'vue-star-rating'


// todo
// cssVars()

Vue.use(BootstrapVue);
Vue.use(VueRouter);
Vue.use(Vuelidate);
Vue.use(VueMasonryPlugin);
Vue.component('star-rating', StarRating);

const router = new VueRouter({
  routes,
  mode:'history',
});


router.beforeEach((to,from,next) => {
  if(to.matched.some(record=> record.meta.requiresAuth)){
    if(!store.getters.loggedIn) {
      next({
        name: 'Login',
      })
    }else{
      next()
    }

  }else{
    next()
  }
});

/* eslint-disable no-new */
new Vue({
  store,
  el: '#app',
  router,
  template: '<App/>',
  components: {
    App
  }
});


