checkToken=()=>{
    const token = localStorage.getItem("token");
    if(!token)
    {
      alert("not token")
      location.href = "../html/login.html";
      return;
    }
    const expiresIn = parseInt(getResponseHeader("ExpiresIn")); 
    console.log(`expiresIn = ${expiresIn}`);
    const now = Date.now() / 1000; 
    
    if (expiresIn - now <= 60) { 
        alert("time over!");
      localStorage.removeItem("token"); 
      window.location.href = "../html/tasks.html"; 
    }
  }
  
  getResponseHeader=(name)=>{
      const headers = new Headers(document.head.querySelector('meta[name="http-response-headers"]').textContent);
      return headers.get(name);
  }