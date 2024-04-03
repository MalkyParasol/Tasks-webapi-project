const dom = {
    name: document.getElementById("user-name"),
    password: document.getElementById("password")
}
const uri = "/api/Login";
let token = "";

const loginUser=()=>{
    const user={
        name:dom.name.value,
        password:dom.password.value,
    }
    console.log(`user: ${user}`)
    fetch(uri,{
        method:'POST',
        headers:{
            Accept: "application/json",
            "Content-Type": "application/json",
          },
        body: JSON.stringify(user)
    })
    .then((response) => response.json())
    .then((jwt) => writeToken(jwt))
    .catch((error) => console.error("Unable to login", error));
}
const writeToken=(jwtToken)=>{
    console.log(`jwt token: ${jwtToken}`)
    localStorage.setItem('token', jwtToken);
    window.location.href = '../html/index.html'; 
}


