<template>
  <div class="animated fadeIn">

    <b-row>
      <b-col md="12">
        <b-card>
          <div slot="header">
            <strong>Create</strong>  New Dish
          </div>
          <!-- Dish creation form -->
          <b-form>
          <b-form-group
            description="Give an attractive dish name."
            label="Dish Name"
            label-for="dishName"
            :label-cols="3"
            >
            <b-form-input id="dishName" v-model="dishName" type="text"></b-form-input>
            <b-alert class="mt-1" show variant="danger" v-if="!$v.dishName.required && hasError">Dish Name is a required field</b-alert>

          </b-form-group>

            <b-form-group
              label="Dish Description"
              label-for="dishDescription"
              :label-cols="3"
            >
              <b-form-textarea id="dishDescription" v-model="dishDescription" :rows="9" placeholder="Description.."></b-form-textarea>
              <b-alert class="mt-1" show variant="danger" v-if="!$v.dishDescription.required && hasError">Dish Description is a required field</b-alert>

            </b-form-group>


            <b-form-group
              label="Dish Price"
              label-for="dishPrice"
              :label-cols="3"
            >
              <b-form-input id="dishPrice" v-model="dishPrice" type="number" ></b-form-input>

              <b-alert class="mt-1" show variant="danger" v-if="!$v.dishPrice.required && hasError">Dish Price is a required field</b-alert>
              <b-alert class="mt-1" show variant="danger" v-if="!$v.dishPrice.nonNeg && hasError">Dish Price is invalid</b-alert>

            </b-form-group>


            <b-form-group
              label="Dish Images"
              label-for="dishImages"
              description="Select Images."
              :label-cols="3"
            >
              <!-- Image Upload  -->
              <label>Files
                <input type="file" id="files" ref="files" multiple v-on:change="handleFilesUpload()" accept="image/*"/>
              </label>

            </b-form-group>

            <!-- Show uploaded images -->
            <div class="large-12 medium-12 small-12 cell">
              <div v-for="(file, key) in files" class="file-listing">{{ file.name }}
                <b-button class="ml-2" variant="danger" v-on:click="removeFile( key )">
                  <i class="fa fa-trash-o"></i>&nbsp;
                </b-button>
              </div>
            </div>
            <br>

            <div slot="footer">
              <b-button  @click="createDish()" size="sm" variant="primary"><i class="fa fa-dot-circle-o"></i> Submit</b-button>
              <b-button class="ml-2" type="reset" size="sm" variant="danger"><i class="fa fa-ban"></i> Reset</b-button>
            </div>

            <b-alert show variant="danger" v-if="showAlert">{{errMessage}}</b-alert>


            </b-form>

        </b-card>

      </b-col>

    </b-row>

  </div>
</template>

<script>
  import { required } from 'vuelidate/lib/validators'

export default {
  name: 'createdish',
  data () {
    return {
      dishName:"",
      dishDescription:"",
      dishPrice:"",
      dishImages:[],

      files: [],
      successUploads:0,

      hasError:false,
      showAlert:false,
      errMessage:""

    }
  },

  validations: {
    dishName: {
      required
    },
    dishDescription: {
      required
    },
    dishPrice: {
      required,
      nonNeg(dishPrice){
        return (dishPrice>0)
      }
    }
  },


  methods: {
    // Validate inputs and start uploading images
    createDish(){
      this.hasError = false;
      this.showAlert=false;
      this.$v.$touch();
      if (!this.$v.$invalid) {
        this.submitFiles()
      }else{
        this.hasError=true
      }

    },

    // Adding selected images to list
    handleFilesUpload(){
      let uploadedFiles = this.$refs.files.files;
      //Adds the uploaded file to the files array
      for( var i = 0; i < uploadedFiles.length; i++ ){
        this.files.push( uploadedFiles[i] );
      }
      console.log(this.files)
    },
    removeFile( key ){
      this.files.splice( key, 1 );
    },

    // Upload images to API and on success add Dish data
    submitFiles(){
      this.successUploads =0;
      //Iteate over any file sent over appending the files  to the form data.
      for( var i = 0; i < this.files.length; i++ ){

        //Initialize the form data
        let formData = new FormData();

        let file = this.files[i];
        formData.append('Image', file);
        formData.append('Name', this.files[i].name);

        //Make the request to the POST /select-files URL
        this.$store.dispatch('uploadImage', formData).then(res => {
          this.dishImages.push(res.imageId);
          this.successUploads +=1;
          if(this.successUploads == this.files.length){
            this.$store.dispatch('createDish', {
              dishName: this.dishName,
              dishDescription: this.dishDescription,
              dishPrice: this.dishPrice,
              Images: this.dishImages
            }).then(res => {
              this.$router.push({name: "DishList"})
            }).catch(err=>{
              this.errMessage = err.message;
              this.showAlert =true
            })
          }

        }).catch(err=>{
          this.errMessage = err.message;
          this.showAlert =true
        })


      }


    },



  },




}
</script>

<style scoped>
.fade-enter-active,
.fade-leave-active {
  transition: opacity 0.5s;
}
.fade-enter,
.fade-leave-to {
  opacity: 0;
}
</style>
