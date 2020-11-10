<template>
  <div class="animated fadeIn">
    <!-- Dashboard stat tags -->
    <b-row>
      <b-col sm="6" lg="3">
        <b-card no-body class="bg-primary">
          <b-card-body class="pb-0">
            <h4 v-model="dishStat" class="mb-0">{{dishStat}}</h4>
            <p>Dishes</p>
          </b-card-body>
        </b-card>
      </b-col>
      <b-col sm="6" lg="3">
        <b-card no-body class="bg-warning">
          <b-card-body class="pb-0">
            <h4 v-model="ratingStat" class="mb-0">{{ratingStat}}</h4>
            <p>Reviews</p>
          </b-card-body>
        </b-card>
      </b-col>
      <b-col sm="6" lg="3">
        <b-card no-body class="bg-danger">
          <b-card-body class="pb-0">
            <h4 v-model="authStat" class="mb-0">{{authStat}}</h4>
            <p>Admins</p>
          </b-card-body>
        </b-card>
      </b-col>
    </b-row>


  </div>
</template>

<script>

export default {
  name: 'dashboard',
  components: {

  },
  data: function () {
    return {

      dishStat:0,
      ratingStat:0,
      authStat:0


    }
  },
  methods: {
    // Stat requests to API
    getDishStat(){

      this.$store.dispatch('getDishStat').then(res => {
        console.log(res);
        this.dishStat  = res
      }).catch(err=>{
        console.log(err.message)

      })

    },
    getRatingStat(){
      this.$store.dispatch('getRatingStat').then(res => {
        console.log(res);
        this.ratingStat  = res
      }).catch(err=>{
        console.log(err.message)

      });
    },
    getAuthStat(){
      this.$store.dispatch('getAuthStat').then(res => {
        console.log(res);
        this.authStat  = res
      }).catch(err=>{
        console.log(err.message)

      });
    }


  },
  beforeMount() {
    //Get all three stats from API
    console.log("mount");
    this.getDishStat()
    this.getRatingStat()
    this.getAuthStat()
  }

}
</script>

<style>
  /* IE fix */
  #card-chart-01, #card-chart-02 {
    width: 100% !important;
  }
</style>
