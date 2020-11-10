<template>
  <div class="animated fadeIn">
    <!-- Dish List Component -->
    <b-row>
      <!-- Create New Dish Button -->
      <b-col lg="12">
        <b-card-body>
          <b-row>
            <b-col sm xs="12" >
              <b-button variant="primary" v-on:click="newDish">
                <i class="fa fa-plus-circle"></i>&nbsp;Create New Dish
              </b-button>
            </b-col>
          </b-row>
        </b-card-body>
      </b-col>

      <!-- Dish table -->
      <b-col lg="12">
        <c-table :table-data="items" :reload="getAllDishes" fixed bordered hover caption="<i class='fa fa-align-justify'></i> Dishes Table"></c-table>
      </b-col>


    </b-row><!--/.row-->


  </div>

</template>

<script>
import { shuffleArray } from '@/shared/utils'
import cTable from './Table.vue'

const someData = () => shuffleArray([
  {
    dishId: "1d4057b8-de2b-4870-ac8e-65f18af3d882",
    dishName: "Pizza za Desczza  ",
    dishDescription: "This is new Pizza Descripnew Pizza Description.  This is new Pizza Description. ",
    dishPrice: 18.0,
    dishTotalRating: 0,
    images: [
      {
        imageId: "32c59001-1969-4f73-924c-90b570626e26",
        name: "Image1",
        path: "http://localhost:25803/api/dish_images/fba9cdd0-e663-4434-9e87-5c5b944d26c4_pizza.jpg",
        isDeleted: false,
        dateCreated: "0001-01-01T00:00:00",
        dateModified: "0001-01-01T00:00:00"
      }
    ],
    ratings: [],
    isDeleted: false,
    dateCreated: "2020-04-15T09:24:09.9543879",
    dateModified: "2020-04-15T09:24:09.954388"
  },
  {
    dishId: "1d40sdfe-de2b-4870-ac8e-65f18af3d882",
    dishName: "Bun",
    dishDescription: "This is new Bun Description. ",
    dishPrice: 58.0,
    dishTotalRating: 5,
    images: [
      {
        imageId: "32c59001-1969-4f73-924c-90b570626e26",
        name: "Image1",
        path: "http://localhost:25803/api/dish_images/fba9cdd0-e663-4434-9e87-5c5b944d26c4_pizza.jpg",
        isDeleted: false,
        dateCreated: "0001-01-01T00:00:00",
        dateModified: "0001-01-01T00:00:00"
      }
    ],
    ratings: [],
    isDeleted: false,
    dateCreated: "2020-04-15T09:24:09.9543879",
    dateModified: "2020-04-15T09:24:09.954388"
  }


]);

export default {
  name: 'tables',
  components: {cTable},
  data: () => {
    return {
      items: [],
    }
  },
  methods: {
    newDish(){
      this.$router.push({name:'CreateDish'})
    },
    //Get All dishes from API
    getAllDishes(){

        this.$store.dispatch('retrieveDishes').then(res => {
          console.log(res);
          this.items = res
        }).catch(err=>{
          console.log(err.message)

        })


    },



  },
  beforeMount() {
    console.log("mount");
    this.getAllDishes()
  }
}
</script>
