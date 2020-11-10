<template>
  <div class="animated fadeIn">

    <b-row>

      <!-- Rating List -->
      <b-col lg="12">
        <c-table :table-data="items" :reload="getRatingsForDish" fixed bordered hover caption="<i class='fa fa-align-justify'></i> All Reviews For the Dish"></c-table>
      </b-col>


    </b-row><!--/.row-->


  </div>

</template>

<script>
import { shuffleArray } from '@/shared/utils'
import cTable from './Table.vue'

const someData = () => shuffleArray([
  {
    "ratingId": "638fbda5-5d43-4fc7-b890-13c1b2a1f142",
    "userName": "PizzaBoy",
    "userEmail": "rev@gmail.com",
    "rate": 4,
    "review": "Good Pizza",
    "isDeleted": false,
    "dateCreated": "0001-01-01T00:00:00",
    "dateModified": "0001-01-01T00:00:00"
  },
  {
    "ratingId": "638fbda5-5d43-4fc7-b890-13c1b2a1f142",
    "userName": "PizzaBoy",
    "userEmail": "rev@gmail.com",
    "rate": 4,
    "review": "Good Pizza",
    "isDeleted": false,
    "dateCreated": "0001-01-01T00:00:00",
    "dateModified": "0001-01-01T00:00:00"
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
      console.log("dfsdfsdf")
    },
    //Get rating for given ID from API
    getRatingsForDish(){
      // Get the dish id from url
      var dishId = this.$route.query.dishId;
      console.log(dishId);
      this.$store.dispatch('retrieveRatings', dishId).then(res => {
        console.log(res);
        this.items = res
      }).catch(err=>{
        console.log(err.message)

      })


    },
  },
  beforeMount() {
    console.log("mount");
    this.getRatingsForDish()
  }
}
</script>
