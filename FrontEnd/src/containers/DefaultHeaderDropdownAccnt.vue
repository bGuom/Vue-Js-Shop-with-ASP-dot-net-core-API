<template>
  <div>
  <AppHeaderDropdown right no-caret>
    <template slot="header">
      <img
        src="img/avatars/6.jpg"
        class="img-avatar"/>
    </template>\
    <template slot="dropdown">
      <b-dropdown-header tag="div" class="text-center"><strong>Account</strong></b-dropdown-header>
      <b-dropdown-item @click="logoutReq"><i class="fa fa-lock" /> Logout</b-dropdown-item>

    </template>
  </AppHeaderDropdown>
    <b-modal
      title="Logout"
      variant="warning"
      header-bg-variant="warning"
      content-class="border-warning"
      v-model="Modal"
      @ok="doLogout"
      ok-variant="warning"
    >
      "Are you sure to logout ?"
    </b-modal>
  </div>
</template>

<script>
import { HeaderDropdown as AppHeaderDropdown } from '@coreui/vue'
export default {
  name: 'DefaultHeaderDropdownAccnt',
  components: {
    AppHeaderDropdown
  },
  data: () => {
    return { Modal:false }
  },methods : {
    logoutReq(){
      this.Modal = true
    },
    //Delete request to API
    doLogout(){
      this.Modal = false;
      this.$store.dispatch('logout')
    },
  }
}
</script>
