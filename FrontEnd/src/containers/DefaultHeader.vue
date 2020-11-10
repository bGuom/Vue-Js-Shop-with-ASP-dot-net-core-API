<template>
    <AppHeader fixed>
      <SidebarToggler class="d-lg-none" display="md" mobile />
      <b-link class="navbar-brand" to="#">
        7Sins Admin

      </b-link>
      <SidebarToggler class="d-md-down-none" display="lg" :defaultOpen=true />
      <b-navbar-nav class="d-md-down-none">
        <b-nav-item class="px-3" to="/dashboard">Dashboard</b-nav-item>
        <b-nav-item class="px-3" to="/dish/list" exact>Dish</b-nav-item>
        <b-nav-item class="px-3" to="/menu/menu" exact>Menu</b-nav-item>
      </b-navbar-nav>
      <b-navbar-nav class="ml-auto">


        <DefaultHeaderDropdownAccnt/>
      </b-navbar-nav>

    </AppHeader>
</template>
<script>
import { Header as AppHeader, SidebarToggler } from '@coreui/vue'
import DefaultHeaderDropdownAccnt from './DefaultHeaderDropdownAccnt'

export default {
  name: 'DefaultHeader',
  components: {

    //<img class="navbar-brand-full" src="img/brand/logo.jpeg" width="89" height="25" alt="7Sins Admin">
    //<img class="navbar-brand-minimized" src="img/brand/sygnet.svg" width="30" height="30" alt="CoreUI Logo">

    AppHeader,
    DefaultHeaderDropdownAccnt,
    SidebarToggler
  }
}
</script>
