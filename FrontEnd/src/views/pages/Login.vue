<template>
  <div class="app flex-row align-items-center">
    <div class="container">
      <b-row class="justify-content-center">
        <b-col md="5">
          <b-card-group>
            <b-card no-body class="p-4">
              <b-card-body>
                <!-- Login Form -->
                <b-form @submit.prevent="login">
                  <h1>Login</h1>
                  <p class="text-muted">Sign In to your account</p>
                  <b-input-group class="mb-3">
                    <b-input-group-prepend><b-input-group-text><i class="icon-user"></i></b-input-group-text></b-input-group-prepend>
                    <b-form-input type="text" v-model="username" class="form-control" placeholder="Email" autocomplete="email" />
                  </b-input-group>
                  <b-alert show variant="danger" v-if="!$v.username.required && error">Email is a required field</b-alert>
                  <b-input-group class="mb-4">
                    <b-input-group-prepend><b-input-group-text><i class="icon-lock"></i></b-input-group-text></b-input-group-prepend>
                    <b-form-input type="password" v-model="password" class="form-control" placeholder="Password" autocomplete="current-password" />
                  </b-input-group>
                  <b-alert show variant="danger" v-if="!$v.password.required && error">Password is a required field</b-alert>
                  <b-alert show variant="danger" v-if="showAlert">{{errMessage}}</b-alert>
                  <b-row>
                    <b-col cols="6">
                      <b-button type="submit" variant="primary" class="px-4">Login</b-button>
                    </b-col>
                    <b-col cols="6" class="text-right">
                      <b-button variant="link" class="px-0">Forgot password?</b-button>
                    </b-col>
                  </b-row>
                </b-form>
              </b-card-body>
            </b-card>

          </b-card-group>
        </b-col>
      </b-row>
    </div>
  </div>
</template>

<script>
  import { required } from 'vuelidate/lib/validators'


  export default {

  name: 'Login',
  data(){
    return{
      error:false,
      username:"",
      password:"",

      errMessage:"",
      showAlert: false
    }
  },

    validations: {
      username: {
        required
      },
      password: {
        required
      }
    },

    methods:{
    //Send Login request to API and redirect to dashboard on success
    login(){
      this.error=false;
      this.showAlert =false;
      this.$v.$touch();
      if (!this.$v.$invalid) {
        this.$store.dispatch('retrieveToken', {email: this.username, password: this.password}).then(res => {
          console.log("User Logged In");
          this.$router.push({name: "Dashboard"})
        }).catch(err=>{
          console.log(err.message);
          this.errMessage = err.message;
          this.showAlert =true
        })
      }else{
        this.error=true;

      }

    },
    register(){
      this.$router.push({name:'Register'})
    },

  }
}
</script>
