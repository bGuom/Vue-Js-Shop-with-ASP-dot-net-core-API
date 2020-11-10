<template>
  <div class="app flex-row align-items-center">
    <div class="container">
      <b-row class="justify-content-center">
        <b-col md="6" sm="8">
          <b-card no-body class="mx-4">
            <b-card-body class="p-4">
              <b-form @submit.prevent="register">
                <h1>Register</h1>
                <p class="text-muted">Create your account</p>
                <b-input-group class="mb-3">
                  <b-input-group-prepend>
                    <b-input-group-text><i class="icon-user"></i></b-input-group-text>
                  </b-input-group-prepend>
                  <b-form-input type="text" v-model="username" class="form-control" placeholder="Username" autocomplete="username" />
                </b-input-group>
                <b-alert show variant="danger" v-if="!$v.username.required && hasError">Username is a required field</b-alert>

                <b-input-group class="mb-3">
                  <b-input-group-prepend>
                    <b-input-group-text><i class="icon-user"></i></b-input-group-text>
                  </b-input-group-prepend>
                  <b-form-input type="text" v-model="displayname" class="form-control" placeholder="Display Name" autocomplete="displayname" />
                </b-input-group>
                <b-alert show variant="danger" v-if="!$v.displayname.required && hasError">DisplayName is a required field</b-alert>

                <b-input-group class="mb-3">
                  <b-input-group-prepend>
                    <b-input-group-text>@</b-input-group-text>
                  </b-input-group-prepend>
                  <b-form-input type="text" v-model="email" class="form-control" placeholder="Email" autocomplete="email" />
                </b-input-group>
                <b-alert show variant="danger" v-if="!$v.email.required && hasError">Email is a required field</b-alert>

                <b-input-group class="mb-3">
                  <b-input-group-prepend>
                    <b-input-group-text><i class="icon-lock"></i></b-input-group-text>
                  </b-input-group-prepend>
                  <b-form-input type="password" v-model="password" class="form-control" placeholder="Password" autocomplete="new-password" />
                </b-input-group>
                <b-alert show variant="danger" v-if="!$v.password.required && hasError">Password is a required field</b-alert>
                <b-alert show variant="danger" v-if="!$v.password.minLength && hasError">Password should be longer than 5 characters</b-alert>
                <b-alert show variant="danger" v-if="!$v.password.maxLength && hasError">Password should be no longer than 25 characters</b-alert>
                <b-alert show variant="danger" v-if="!$v.password.strong && hasError">Password should contain lower case , uppercase , numbers and special characters</b-alert>
                <b-input-group class="mb-4">
                  <b-input-group-prepend>
                    <b-input-group-text><i class="icon-lock"></i></b-input-group-text>
                  </b-input-group-prepend>
                  <b-form-input type="password" v-model="repassword" class="form-control" placeholder="Repeat password" autocomplete="new-password" />
                </b-input-group>
                <b-alert show variant="danger" v-if="!$v.repassword.required && hasError">RePassword is a required field</b-alert>
                <b-alert show variant="danger" v-if="!$v.repassword.matched">Password does not match</b-alert>

                <b-button type="submit" variant="success" block>Create Account</b-button>
                <br>
                <b-alert show variant="danger" v-if="showAlert">{{errMessage}}</b-alert>
              </b-form>
            </b-card-body>

          </b-card>
        </b-col>
      </b-row>
    </div>
  </div>
</template>

<script>

  import { required,email,minLength,maxLength } from 'vuelidate/lib/validators'

export default {
  name: 'Register',
  data(){
    return{
      username:"",
      displayname:"",
      email:"",
      password:"",
      repassword:"",

      hasError:false,
      passwordsMatch:true,

      showAlert:false,
      errMessage:""
    }
  },

  validations: {
    username: {
      required
    },
    displayname: {
      required
    },
    email: {
      required,email
    },
    password: {
      required,
      minLength: minLength(5),
      maxLength: maxLength(25),
      strong(password){
        return (/(?=^.{8,}$)((?=.*\d)|(?=.*\W+))(?![.\n])(?=.*[A-Z])(?=.*[a-z]).*$/.test(password))
      }
    },
    repassword: {
      required,
      matched() {
        return (this.repassword==this.password);
      }
    },
  },

  methods:{
    register(){
      this.hasError = false;
      this.showAlert=false;
      this.$v.$touch();
      var passwordsMatch = this.password === this.repassword;
      if (!passwordsMatch) {
        this.hasError = true
      }
      if (!this.$v.$invalid && passwordsMatch) {
        this.$store.dispatch('register', {
          userName: this.username,
          displayName: this.displayname,
          email: this.email,
          password: this.password
        }).then(res => {
          this.$router.push({name: "Login"})
        }).catch(err=>{
          this.errMessage = err.message;
          this.showAlert =true
        })
      }else{
        this.hasError=true
      }

    }
  }
}
</script>
