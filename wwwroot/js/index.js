checkToken = () => {
  const token = localStorage.getItem("token");
 
  if (!token) {
    location.href = "../html/login.html";
    return;
  }
  try {
    
    const decodedToken = JSON.parse(atob(token.split('.')[1]));
    const expirationTime = decodedToken.exp; 

    const now = Math.floor(Date.now() / 1000); 

    if (expirationTime - now <= 60) {
      localStorage.removeItem("token");
      localStorage.removeItem("expirationTime")
      window.location.href = "../html/login.html";
    }
    else
    {
      localStorage.setItem("expirationTime",expirationTime)
      window.location.href="../html/tasks.html";
    }
  } catch (error) {
    console.error("Error decoding token:", error);
    localStorage.removeItem("token");
    if(localStorage.getItem("expirationTime"))
    {
      localStorage.removeItem("expirationTime");
    }
    window.location.href = "../html/login.html";
  }
};