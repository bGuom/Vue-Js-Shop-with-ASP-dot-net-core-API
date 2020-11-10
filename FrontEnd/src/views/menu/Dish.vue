<template>

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
          </ul>
        </div>
      </div>
    </nav>

    <!-- Page Content -->
    <div class="container mt-5">

      <div class="row ">

        <!-- Content Column -->
        <div class="col-lg-10">

          <!-- Title -->
          <h1 class="mt-4">{{item.dishName}}</h1>

          <!-- Dish Current Rating -->
          <p class="lead">
            <star-rating :rating="item.dishTotalRating" :read-only="true" :increment="0.1" :star-size="25"></star-rating>
          </p>

          <hr>

          <!-- Date/Time -->
          <p>Posted on {{getCreatedDate()}}</p>

          <hr>

          <!-- Preview Image -->
          <img class="img-fluid rounded" v-bind:src="loadimg(item.images[0].imageId)" alt="">

          <hr>
          <!-- Dish Price -->
          <h1><b-badge variant="warning">{{item.dishPrice}} KR</b-badge></h1>

          <!-- Dish Description -->
          <p class="lead">{{item.dishDescription}}</p>


          <hr>

          <!-- Rating Form -->
          <div class="card my-4 m-3">
            <h5 class="card-header">Rate this Dish</h5>
            <div class="card-body" >
              <form>
                <div class="form-group " >
                  <h6>How much do you love this dish?</h6>
                  <star-rating v-model="rateData.Rate" :star-size="55"></star-rating>
                  <b-alert class="mt-1" show variant="danger" v-if="!$v.rateData.Rate.nonZero && hasError">Rating is a required</b-alert>
                  <br>
                  <h6>Name</h6>
                  <textarea v-model="rateData.UserName" class="form-control" rows="1"></textarea>
                  <b-alert class="mt-1" show variant="danger" v-if="!$v.rateData.UserName.required && hasError">User Name is a required field</b-alert>
                  <br>
                  <h6>Email</h6>
                  <textarea v-model="rateData.UserEmail" class="form-control" rows="1"></textarea>
                  <b-alert class="mt-1" show variant="danger" v-if="!$v.rateData.UserEmail.required && hasError">Email is a required field</b-alert>
                  <b-alert class="mt-1" show variant="danger" v-if="!$v.rateData.UserEmail.email && hasError">Email is incorrect</b-alert>

                  <br>
                  <h6>Any Reviews?</h6>
                  <textarea v-model="rateData.Review" class="form-control" rows="3"></textarea>
                </div>
                <b-button @click="sendRating()"  size="m" variant="primary">Submit</b-button>
              </form>
            </div>
          </div>
          <!-- For Loop for Review -->
          <div v-for="(rating, index) in item.ratings">
          <!-- Single Review -->
          <div class="media mb-4">
            <img class="d-flex mr-3 rounded-circle" v-bind:src="getAvatar(rating.userEmail)" alt="">
            <div class="media-body">
              <h5 class="mt-0">{{rating.userName}}</h5>
              <h6 class="mt-0">{{rating.userEmail}}</h6>
              <star-rating :rating="rating.rate" :read-only="true" :increment="0.1" :star-size="20"></star-rating>
              {{rating.review}}
            </div>
          </div>
        </div>



        </div>



      </div>
      <!-- /.row -->



    </div>
    <!-- /.container -->

    <!-- Footer -->
    <footer class="py-5 bg-dark">
      <div class="container">
        <p class="m-0 text-center text-white">Copyright &copy; 7Sins 2020</p>
        <p @click="login()"  class="m-0 text-center btn-link">Admin Login</p >
      </div>
      <!-- /.container -->
    </footer>

  </div>
</template>

<script>
  import moment from "moment";
  import { required,email } from 'vuelidate/lib/validators'
export default {
  name: 'cards',
  components: {

  },
  data: function () {
    return {
      show: true,
      item:null,
      itemx: {
        "dishId": "1d4057b8-de2b-4870-ac8e-65f18af3d882",
        "dishName": "Pizza",
        "dishDescription": "This is new Pizza Description, This oizza is very good and everyone likes to eat this pizza.. We also love to eat this piza. But pizza is god pizza so pizzz is noyt pizza. Hwever  pizza are very good for health     This is new Pizza Description, This oizza is very good and everyone likes to eat this pizza.. We also love to eat this piza. But pizza is god pizza so pizzz is noyt pizza. Hwever  pizza are very good for health",
        "dishPrice": 18.0,
        "dishTotalRating": 4.0,
        "images": [
          {
            "imageId": "32c59001-1969-4f73-924c-90b570626e26",
            "name": "Image1",
            "path": "F:/Fiverr/WEB_DEV/SHOP/API/Backend_API/Backend_API/dish_images/fba9cdd0-e663-4434-9e87-5c5b944d26c4_pizza.jpg",
            "isDeleted": false,
            "dateCreated": "0001-01-01T00:00:00",
            "dateModified": "0001-01-01T00:00:00"
          }
        ],
        "ratings": [
          {
            "ratingId": "5a9bbc99-ea01-4535-a6d4-050f05a11258",
            "userName": "PizzaBoy",
            "userEmail": "rev@gmail.com",
            "rate": 4,
            "review": "Good Pizza",
            "isDeleted": false,
            "dateCreated": "0001-01-01T00:00:00",
            "dateModified": "0001-01-01T00:00:00"
          },
          {
            "ratingId": "2274fac0-6785-4d41-9ad4-0a24016a8e5a",
            "userName": "DAD",
            "userEmail": "DAD@gmail.com",
            "rate": 5,
            "review": "DAAAD Good Pizza",
            "isDeleted": false,
            "dateCreated": "0001-01-01T00:00:00",
            "dateModified": "0001-01-01T00:00:00"
          },
          {
            "ratingId": "d23edc25-eb5e-4304-9b06-b169d0bfa21a",
            "userName": "PizzaBoy",
            "userEmail": "rev@gmail.com",
            "rate": 4,
            "review": "Good Pizza",
            "isDeleted": false,
            "dateCreated": "0001-01-01T00:00:00",
            "dateModified": "0001-01-01T00:00:00"
          },
          {
            "ratingId": "9ed0fb93-9d6b-47e6-b3e9-e08f2b681628",
            "userName": "PizzaMAD",
            "userEmail": "DAv@gmail.com",
            "rate": 4,
            "review": "MAAD Good Pizza",
            "isDeleted": false,
            "dateCreated": "0001-01-01T00:00:00",
            "dateModified": "0001-01-01T00:00:00"
          }
        ],
        "isDeleted": false,
        "dateCreated": "2020-04-15T09:24:09.9543879",
        "dateModified": "2020-04-15T09:24:09.954388"
      },

      rateData : {
        DishId : '',
        UserName : '',
        UserEmail:'',
        Rate:0,
        Review:''

      },

      hasError:false,
    }
  },

  validations: {
    rateData : {
      UserName: {
        required
      },
      UserEmail: {
        required,
        email
      },
      Rate: {
        nonZero() {
          return (this.rateData.Rate!=0)
        },
      }
    }
  },

  methods: {
    loadimg(id){
      return "http://localhost:25803/api/images/file?id=" + id
    },

    login(){
      this.$router.push({name: "Login"})
    },

    //Get Dish from API
    getDish(){
      var dishId = this.$route.query.dishId;
      this.$store.dispatch('retrieveDishById',dishId).then(res => {
        console.log(res);
        this.item = res
      }).catch(err=>{
        console.log(err.message)

      })
    },

    openDish(item){
      console.log(item)
    },
    getCreatedDate(){
      return moment(this.item.dateCreated).format("DD MMM, YYYY HH:mm")
    },
    getAvatar(name){
      return "https://api.adorable.io/avatars/50/" + name + ".png"
    },

    //Send Rating data to API
    sendRating(){
      console.log('sendingI');
      this.hasError =false;
      this.$v.$touch();
      if (!this.$v.$invalid) {
        this.rateData.DishId = this.item.dishId;
        this.$store.dispatch('postRating', this.rateData).then(res => {
          this.rateData.UserEmail = '';
          this.rateData.UserName ='';
          this.rateData.Review='';
          this.rateData.Rate=0;
          this.getDish()
        }).catch(err=>{
          this.errMessage = err.message
        })
      }else{
        this.hasError=true
      }

    }


  },
  beforeMount() {
    console.log("mount");
    //Load Dish
    this.getDish()
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
