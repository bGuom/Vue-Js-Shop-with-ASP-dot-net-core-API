import Vue from 'vue'
import Router from 'vue-router'

// Containers
const DefaultContainer = () => import('@/containers/DefaultContainer');

// Views
const Home = () => import('@/views/Home');
const Dashboard = () => import('@/views/dashboard/Dashboard');

// Views - Components
const Dishes = () => import('@/views/dish/List');
const newDish = () => import('@/views/dish/Create');
const editDish = () => import('@/views/dish/Edit');
const Ratings = () => import('@/views/ratings/List');

//User visible pages
const menu = () => import('@/views/menu/Menu');
const dishView = () => import('@/views/menu/Dish');

// Views - Pages
const Login = () => import('@/views/pages/Login');
const Register = () => import('@/views/pages/Register');




const routes = [
    {
      path: '/',
      redirect: '/home',
      name: 'Home',
      component: DefaultContainer,
      children: [
        {
          path: 'home',
          name: 'Home',
          component: Home
        },
        {
          path: 'dashboard',
          name: 'Dashboard',
          meta:{
            requiresAuth: true,
          },
          component: Dashboard
        },
        {
          path: 'dish',
          redirect: '/dish/list',
          name: 'Dish',
          meta:{
            requiresAuth: true,
          },
          component: {
            render (c) { return c('router-view') }
          },
          children: [
            {
              path: 'list',
              name: 'DishList',
              meta:{
                requiresAuth: true,
              },
              component: Dishes
            },
            {
              path: 'create',
              name: 'CreateDish',
              meta:{
                requiresAuth: true,
              },
              component: newDish
            },
            {
              path: 'edit',
              name: 'EditDish',
              meta:{
                requiresAuth: true,
              },
              component: editDish
            }
          ]
        },

        {
          path: 'ratings',
          redirect: '/ratings/list',
          name: 'Ratings',
          meta:{
            requiresAuth: true,
          },
          component: {
            render (c) { return c('router-view') }
          },
          children: [
            {
              path: 'list',
              name: 'RatingList',
              meta:{
                requiresAuth: true,
              },
              component: Ratings
            },
            {
              path: 'create',
              name: 'Create Dish',
              meta:{
                requiresAuth: true,
              },
              component: newDish
            },

          ]
        },

      ]
    },
    {
      path: '/pages',
      redirect: '/pages/404',
      name: 'Pages',
      component: {
        render (c) { return c('router-view') }
      },
      children: [
        {
          path: 'login',
          name: 'Login',
          component: Login
        },
        {
          path: 'register',
          name: 'Register',
          component: Register
        }
      ]
    },
    {
    path: '/menu',
    redirect: '/menu/Menu',
    name: 'Menu',
    component: {
      render (c) { return c('router-view') }
    },
    children: [
      {
        path: 'Menu',
        name: 'MenuList',
        component: menu
      },
      {
        path: 'Dish',
        name: 'DishView',
        component: dishView
      },



    ]
  },
  ];

export default routes


//export default new Router({
  //mode: 'history', // https://router.vuejs.org/api/#mode
  //linkActiveClass: 'open active',
  //scrollBehavior: () => ({ y: 0 }),
  //routes: configRoutes()
//})
