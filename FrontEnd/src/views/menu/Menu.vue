
<template>


  <div>


  <div>
    <!-- Navigation -->
    <nav class="navbar navbar-expand-lg navbar-dark bg-warning fixed-top">
      <div class="container">
        <a class="navbar-brand" href="#">7Sins</a>
        <button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarResponsive" aria-controls="navbarResponsive" aria-expanded="false" aria-label="Toggle navigation">
          <span class="navbar-toggler-icon"></span>
        </button>
        <div class="collapse navbar-collapse" id="navbarResponsive">
          <ul class="navbar-nav ml-auto">
            <li class="nav-item active">
              <a class="nav-link" href="#">Home
                <span class="sr-only">(current)</span>
              </a>
            </li>
            <li class="nav-item">
              <a class="nav-link" href="#">About</a>
            </li>
            <li class="nav-item">
              <a class="nav-link" href="#">Services</a>
            </li>
            <li class="nav-item">
              <a class="nav-link" href="#">Menu</a>
            </li>
            <li class="nav-item">
              <a class="nav-link" href="#">Contact</a>
            </li>
            <li class="nav-item">
              <a @click="login()" class="nav-link" href="#">Admin Login</a>
            </li>

          </ul>
        </div>
      </div>
    </nav>
    </div>
    <!-- Page Content -->
    <div class="container mt-5">



      <!-- Masonry List for Dish List -->
      <div v-masonry origin-left="false" transition-duration="1s" item-selector=".item">
      <div v-masonry-tile class="item mt-5" >
        <b-row>
          <!-- ForLoop -Dishes -->
          <b-col sm="6" md="3" v-for="(item, index) in items">
            <!-- Single Dish Card -->
            <b-link v-on:click="openDish(item)" class="card-link">
            <b-card  v-bind:title="item.dishName"  v-bind:img-src="loadimg(item.images[0].imageId)" img-alt="Image" img-top>
              <b-card-text>
                {{item.dishDescription}}
              </b-card-text>
              <b-row>
                <b-col sm="6" md="6">
                  <star-rating :rating="item.dishTotalRating" :read-only="true" :increment="0.1" :star-size="20"></star-rating>
                </b-col>
                <b-col sm="6" md="6">
                  <h4><b-badge class="ml-5 mt-1" variant="warning">{{item.dishPrice}} KR</b-badge></h4>
                </b-col>
              </b-row>
            </b-card>
            </b-link>
          </b-col>
        </b-row>
      </div>
    </div>

    </div>



  </div>
</template>

<script>

export default {
  name: 'cards',
  components: {

  },
  data: function () {
    return {
      show: true,
      items: [],
    }
  },
  methods: {
    login(){
      this.$router.push({name: "Login"})
    },
    //Get all Dishes from API
    getAllDishes(){

      this.$store.dispatch('retrieveDishes').then(res => {
        console.log(res);
        this.items = res
      }).catch(err=>{
        console.log(err.message)

      })
    },
    //Go to DishView page
    openDish(item){
      console.log(item);
      this.$router.push({name: 'DishView', query: { dishId: item.dishId } });
    },
    loadimg(id){
      return "http://localhost:25803/api/images/file?id=" + id
    },


  },
  beforeMount() {
    console.log("mount");
    //Load all dishes
    this.getAllDishes()
  }
}
</script>

<style scoped>
  .fade-enter-active {
    transition: all .3s ease;
  }
  .fade-leave-active {
    transition: all .8s cubic-bezier(1.0, 0.5, 0.8, 1.0);
  }
  .fade-enter, .fade-leave-to {
    transform: translateX(10px);
    opacity: 0;
  }
</style>
