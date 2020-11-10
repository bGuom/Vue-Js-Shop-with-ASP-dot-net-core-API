<template>
 <p>Hiiiiiiiiiiiiiiiii</p>
</template>

<script>

export default {
  name: 'home',
  components: {

  },
  data: function () {
    return {

    }
  },
  methods: {

  },
  beforeMount() {
    //Redirect to Menu
    console.log("reee")
    this.$router.push({name: "Menu"})
  }

}
</script>

<style>
  /* IE fix */
  #card-chart-01, #card-chart-02 {
    width: 100% !important;
  }
</style>
