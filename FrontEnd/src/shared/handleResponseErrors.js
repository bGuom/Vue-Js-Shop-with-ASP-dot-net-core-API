
export const handleResponseError = function(reject, error) {
  if(error.response && error.response.status) {
    switch (error.response.status) {
      case 400: {
        console.log("Error " + error.response.status + "\n" + error.response.data.title + "\n\n" + JSON.stringify(error.response.data.errors));
        break;
      }
      case 401: {
        localStorage.removeItem("currentUser");
        document.cookie = "token=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;";
        window.location.replace("#");
        break;
      }
      case 500: {
        console.log("Error " + error.response.status + "\n" + error.response.statusText + "\n\n" + error.config.url);
        break;
      }
      default: {
        console.log("Error " + error.response.status + "\n" + error.response.statusText + "\n\n" + error.config.url);
        break;
      }
    }
  } else {
    localStorage.removeItem("currentUser");
    document.cookie = "token=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;";
    window.location.replace(loginUrl);
  }
  reject(error);
};
