import requestModule from '../../shared/requestModule';

const API_URL ="http://localhost:25803";

const GET_DISHES_URL ="/api/dishes";
const GET_RATING_URL ='/api/ratings';
const GET_IMAGE_URL ='/api/images';

//STATES ======
const state ={
  DishArray: []
};

//GETTERS ======
const getters = {


};


//ACTIONS ======
const actions ={

 // Get  number of dishes
  getDishStat(context){
    return new Promise((resolve,reject) => {
      requestModule().get(API_URL + GET_DISHES_URL + "/stat").then(res => {
          console.log('DishStat', res);
          if (res.status == "200") {
            resolve(res.data);
          }
        }
      ).catch(err => {
        console.log('DishStatError : ', err.response.data);
        reject(err.response.data);
      })
    })

  },

  //Get number of ratings
  getRatingStat(context){
    return new Promise((resolve,reject) => {
      requestModule().get(API_URL + GET_RATING_URL + "/stat").then(res => {
          console.log('RaatingStat', res);
          if (res.status == "200") {
            resolve(res.data);
          }
        }
      ).catch(err => {
        console.log('RatingStatError : ', err.response.data);
        reject(err.response.data);
      })
    })
  },

  //Create dish POST request
  //Need Authentication
  createDish(context,body){
    return new Promise((resolve,reject) =>
    {
      const config = {
        headers: { Authorization: `Bearer ${localStorage.getItem("access_token")}` }
      };
      requestModule().post(API_URL + GET_DISHES_URL,body,config).then(res => {
          console.log('createDish',res);
          if (res.status == "201") {
            resolve(res.data)
          }
        }
      ).catch(err => {
        console.log('createDishError : ',err.response.data);
        reject(err.response.data)
      })
    })
  },

  //Edit dish PUT request
  //Need Authentication
  editDish(context,body){
    return new Promise((resolve,reject) =>
    {
      const config = {
        headers: { Authorization: `Bearer ${localStorage.getItem("access_token")}` }
      };
      requestModule().put(API_URL + GET_DISHES_URL,body,config).then(res => {
          console.log('editDish',res);
          if (res.status == "200") {
            resolve(res.data)
          }
        }
      ).catch(err => {
        console.log('editDishError : ',err.response.data);
        reject(err.response.data)
      })
    })
  },


  //Get All dishes GET request
  retrieveDishes(context){
    return new Promise((resolve,reject) =>
    {
      requestModule().get(API_URL + GET_DISHES_URL).then(res => {
          console.log('DishList',res);
          if (res.status == "200") {
            const dishArray = res.data;
            //Committing mutation after action
            context.commit("retrieveDishes", dishArray);
            resolve(res.data)
          }
        }
      ).catch(err => {
        console.log('GetDishError : ',err.response.data);
        reject(err.response.data)
      })
    })
  },

  // Get Dish by Id GET request
  retrieveDishById(context,id){
    return new Promise((resolve,reject) =>
    {
      requestModule().get(API_URL + GET_DISHES_URL + "/" + id).then(res => {
          console.log('Dish',res);
          if (res.status == "200") {
            resolve(res.data)
          }
        }
      ).catch(err => {
        console.log('GetDishError : ',err.response.data);
        reject(err.response.data)
      })
    })
  },

  //Delete dish by Id DELETE request
  //Need Authentication
  deleteDish(context,index){
    return new Promise((resolve,reject) =>
    {
      const config = {
        headers: { Authorization: `Bearer ${localStorage.getItem("access_token")}` }
      };
      requestModule().delete(API_URL + GET_DISHES_URL + "/" + index,config).then(res => {
          console.log('deleteDish',res);
          if (res.status == "204") {
            resolve(res.data)
          }
        }
      ).catch(err => {
        console.log('deleteDishError : ',err.response.data);
        reject(err.response.data)
      })
    })
  },

  //Upload image to server POST request
  //Need Authentication
  uploadImage(context,formdata){
    return new Promise((resolve,reject) =>
    {
      const config = {
        headers: { Authorization: `Bearer ${localStorage.getItem("access_token")}`, 'Content-Type': 'multipart/form-data' }
      };
      requestModule().post(API_URL + GET_IMAGE_URL ,formdata,config).then(res => {
          console.log('uploadImage',res);
          if (res.status == "200") {
            resolve(res.data)
          }
        }
      ).catch(err => {
        console.log('uploadImageError : ',err.response.data);
        reject(err.response.data)
      })
    })
  },

  //Create new rating for dish POST request
  postRating(context,body){
    return new Promise((resolve,reject) =>
    {
      requestModule().post(API_URL + GET_RATING_URL,body).then(res => {
          console.log('postRating',res);
          if (res.status == "200") {
            resolve(res.data)
          }
        }
      ).catch(err => {
        console.log('postRatingError : ',err.response.data);
        reject(err.response.data)
      })
    })
  },

  //Get Ratings for given dish GET request
  retrieveRatings(context,index){
    return new Promise((resolve,reject) =>
    {
      requestModule().get(API_URL + GET_RATING_URL + "/" + index).then(res => {
          console.log('RatingList',res);
          if (res.status == "200") {
            const ratingArray = res.data;
            //Committing mutation after action
            resolve(res.data)
          }
        }
      ).catch(err => {
        console.log('RatingListError : ',err.response.data);
        reject(err.response.data)
      })
    })
  },

  //Delete a rating DELETE request
  //Need Authentication
  deleteRating(context,index){
    return new Promise((resolve,reject) =>
    {
      const config = {
        headers: { Authorization: `Bearer ${localStorage.getItem("access_token")}` }
      };
      requestModule().delete(API_URL + GET_RATING_URL + "/" + index,config).then(res => {
          console.log('deleteRating',res);
          if (res.status == "204") {
            resolve(res)
          }
        }
      ).catch(err => {
        console.log('deleteRatingError : ',err.response.data);
        reject(err.response.data)
      })
    })
  },








};


//MUTATIONS ======
const mutations ={
  retrieveDishes(state,arr){
    state.DishArray=arr
  },

};

export default {
  state,
  getters,
  actions,
  mutations
}

