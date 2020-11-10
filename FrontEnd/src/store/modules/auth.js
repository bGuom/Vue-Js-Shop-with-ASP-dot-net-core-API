import requestModule from '../../shared/requestModule';

const API_URL ="http://localhost:25803";
const AUTH_URL ="/api/auth";
const GETTOKEN_URL ="/api/auth/GetToken";
const REGISTER_URL ='/api/auth/register';

//STATES ======
const state ={
  token: localStorage.getItem("access_token") || null,
};

//GETTERS ======
const getters = {
  loggedIn(state){
    console.log("LoggedIN : ", state.token!= null)
    return state.token!= null
  }
};


//ACTIONS ======
const actions ={
  //Get How may registered users
  getAuthStat(context){
    return new Promise((resolve,reject) => {
      requestModule().get(API_URL + AUTH_URL + "/stat").then(res => {
          console.log('AuthStat', res);
          if (res.status == "200") {
            resolve(res.data);
          }
        }
      ).catch(err => {
        console.log('AuthStatError : ', err.response.data);
        reject(err.response.data);
      })
    })

  },

  //Get Token by sending username and password
  retrieveToken(context,credentials){
    return new Promise((resolve,reject) =>
    {
      requestModule().post(API_URL + GETTOKEN_URL, {
        Email: credentials.email,
        Password: credentials.password
      }).then(res => {
          console.log('GetToken',res.data);
          if (res.data.status != "error") {
            const token = res.data.token;
            localStorage.setItem("access_token", token);
            context.commit("retrieveToken", token);
            resolve(res.data)
          }
        }
      ).catch(err => {
        console.log('GetToken Error : ',err.response.data);
        reject(err.response.data)
      })
    })
  },

  //Create account request
  register(context,data){
    return new Promise((resolve,reject) => {
      requestModule().post(API_URL + REGISTER_URL, {
        "userName": data.userName,
        "displayName": data.displayName,
        "email": data.email,
        "role":"registerduser",
        "password": data.password
      }).then(res => {
        console.log(res);
        if(res.data.status!="Error"){
          resolve(res.data)

        }else{
          reject(res.data)
        }

      }).catch(err => {
        console.log(err.response.data);
        reject(res.response.data)
      })
    })
  },

  //Logout and destroy the token
  logout(context){
    console.log("Logout Request")
    localStorage.removeItem("access_token");
    context.commit("destroyToken")

  }

};


//MUTATIONS ======
const mutations ={
  retrieveToken(state,token){
    state.token=token
  },
  destroyToken(state){
    state.token=null
  }
};

export default {
  state,
  getters,
  actions,
  mutations
}

