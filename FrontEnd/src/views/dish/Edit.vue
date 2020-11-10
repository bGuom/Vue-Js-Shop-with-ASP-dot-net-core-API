<template>
  <div class="animated fadeIn">

    <b-row>
      <b-col md="12">
        <b-card>
          <div slot="header">
            <strong>Edit</strong> Dish
          </div>
          <!-- Edit dish form -->
          <b-form>
            <b-form-group
              description="Give an attractive dish name."
              label="Dish Name"
              label-for="dishName"
              :label-cols="3"
            >
              <b-form-input id="dishName" v-model="item.dishName" type="text"></b-form-input>
              <b-alert class="mt-1" show variant="danger" v-if="!$v.item.dishName.required && hasError">Dish Name is a required field</b-alert>

            </b-form-group>

            <b-form-group
              label="Dish Description"
              label-for="dishDescription"
              :label-cols="3"
            >
              <b-form-textarea id="dishDescription" v-model="item.dishDescription" :rows="9" placeholder="Description.."></b-form-textarea>
              <b-alert class="mt-1" show variant="danger" v-if="!$v.item.dishDescription.required && hasError">Dish Description is a required field</b-alert>

            </b-form-group>


            <b-form-group
              label="Dish Price"
              label-for="dishPrice"
              :label-cols="3"
            >
              <b-form-input id="dishPrice" v-model="item.dishPrice" type="number" ></b-form-input>

              <b-alert class="mt-1" show variant="danger" v-if="!$v.item.dishPrice.required && hasError">Dish Price is a required field</b-alert>
              <b-alert class="mt-1" show variant="danger" v-if="!$v.item.dishPrice.nonNeg && hasError">Dish Price is invalid</b-alert>

            </b-form-group>


            <b-form-group
              label="Dish Images"
              label-for="dishImages"
              description="Select Images."
              :label-cols="3"
            >
              <label>Files
                <input type="file" id="files" ref="files" multiple v-on:change="handleFilesUpload()" accept="image/*"/>
              </label>

            </b-form-group>

            <!-- Show uploaded images -->
            <div class="large-12 medium-12 small-12 cell">
              <div v-for="(file, key) in fakeFiles" class="file-listing mt-2">{{ file.name }}
                <b-button class="ml-2" variant="danger" v-on:click="removeFile( key )">
                  <i class="fa fa-trash-o"></i>&nbsp;
                </b-button>
              </div>
            </div>
            <br>

            <div slot="footer">
              <b-button  @click="updateDish()" size="sm" variant="primary"><i class="fa fa-dot-circle-o"></i> Update</b-button>
              <b-button class="ml-2" @click="getDish()" size="sm" variant="danger"><i class="fa fa-ban"></i> Reset</b-button>
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
        item:{
          dishId:"",
          dishName:"",
          dishDescription:"",
          dishPrice:"",
          dishTotalRating:"",
          images:[]
        },

        // ImageID array
        dishImages:[],

        //fakeFiles are used to show both existing and newly added images
        fakeFiles: [],
        //Real image files
        files: [],
        successUploads:0,

        hasError:false,
        showAlert:false,
        errMessage:""

      }
    },

    validations: {
      item : {
        dishName: {
          required
        },
        dishDescription: {
          required
        },
        dishPrice: {
          required,
          nonNeg(dishPrice) {
            return (dishPrice > 0)
          }
        }
      }
    },


    methods: {
      //Get dish data from API and fill the form
      getDish(){
        this.dishImages = [];
        this.fakeFiles = [];
        var dishId = this.$route.query.dishId;
        this.$store.dispatch('retrieveDishById',dishId).then(res => {
          console.log(res);
          this.item = res;
          this.item.images = res.images;
          for( var i = 0; i < res.images.length; i++ ){
            this.dishImages.push(res.images[i].imageId);
            this.fakeFiles.push({name:res.images[i].name})
          }
          console.log(this.dishImages);
          console.log(this.fakeFiles)

        }).catch(err=>{
          console.log(err.message)

        })


      },
      //Update the Dish
      updateDish(){
        this.hasError = false;
        this.showAlert=false;
        this.$v.$touch();
        if (!this.$v.$invalid) {
          this.submitFiles()
        }else{
          this.hasError=true
        }

      },

      handleFilesUpload(){
        let uploadedFiles = this.$refs.files.files;
        //Adds the uploaded file to the files array
        for( var i = 0; i < uploadedFiles.length; i++ ){
          this.files.push( uploadedFiles[i] );
          this.fakeFiles.push({name:uploadedFiles[i].name})
        }
        console.log(this.files)
      },
      removeFile( key ){
        if(key<this.item.images.length){
          this.dishImages.splice( key, 1 );
          this.fakeFiles.splice( key, 1 );
        }else {
          this.files.splice(key, 1);
        }
      },

      submitFiles(){
        this.successUploads =0;
        //Iteate over any file sent over appending the files  to the form data.
        if(this.files.length>0) {
          for (var i = 0; i < this.files.length; i++) {

            //Initialize the form data
            let formData = new FormData();

            let file = this.files[i];
            formData.append('Image', file);
            formData.append('Name', this.files[i].name);

            //Make the request to the POST /select-files URL
            this.$store.dispatch('uploadImage', formData).then(res => {
              this.dishImages.push(res.imageId);
              this.successUploads += 1;
              if (this.successUploads == this.files.length) {
                this.$store.dispatch('createDish', {
                  dishId:this.item.dishId,
                  dishName: this.item.dishName,
                  dishDescription: this.item.dishDescription,
                  dishPrice: this.item.dishPrice,
                  Images: this.dishImages
                }).then(res => {
                  this.$router.push({name: "DishList"})
                }).catch(err => {
                  this.errMessage = err.message;
                  this.showAlert = true
                })
              }

            }).catch(err => {
              this.errMessage = err.message;
              this.showAlert = true
            })


          }
        }else{
          this.$store.dispatch('editDish', {
            dishId:this.item.dishId,
            dishName: this.item.dishName,
            dishDescription: this.item.dishDescription,
            dishPrice: this.item.dishPrice,
            images: this.dishImages
          }).then(res => {
            this.$router.push({name: "DishList"})
          }).catch(err=>{
            this.errMessage = err.message;
            this.showAlert =true
          })
        }


      },



    },
  beforeMount() {
    console.log("mount");
    this.getDish()
  }




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
