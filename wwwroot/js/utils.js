const expirationTime = localStorage.getItem("expirationTime");

const checkTokenExpiration=()=> {
  const now = Math.floor(Date.now()/1000);
  if(expirationTime-now <= 60){
    window.location.href = "../html/login";
  }
}

const utilsDom = {
    title: document.getElementById("title"),
}

function writeUserName(preliminaryText){
    checkTokenExpiration();
    fetch("/api/me",{
      method:"GET",
      headers:{
        Accept: "application/json",
        "Authorization": `Bearer ${localStorage.getItem("token")}`,
      }
    })
    .then((response)=> response.json())
    .then((data) => utilsDom.title.innerHTML = `${preliminaryText} ${data.name}`)
    .catch((error) => console.error("Unable to get user.", error));
}

function addItem(uri,getFunc,addNameTextbox) {
    //const addNameTextbox = document.getElementById("add-name");
  
    const item = {
      id:0,
      isDone: false,
      name: addNameTextbox.value.trim(),
    };
  
    fetch(uri, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
        "Authorization": `Bearer ${localStorage.getItem("token")}`,
      },
      body: JSON.stringify(item),
    })
      .then((response) => response.json())
      .then(() => {
        getFunc();
        addNameTextbox.value = "";
        
      })
      .catch((error) => console.error("Unable to add item.", error));
}

// function deleteItem(uri,taskId,getFunc){
   
//   console.log(`uri: ${uri}, taskId : ${taskId}, getFunc : ${getFunc}`);
//   fetch(`api/todo/${Number(taskId)}`, {
//     method: "DELETE",
//     headers:{
//       Accept: "application/json",
//       "Content-Type": "application/json",
//       "Authorization": `Bearer ${localStorage.getItem("token")}`,
//     }
    
//   })
//     .then(() => getFunc())
//     .catch((error) => console.error("Unable to delete item.", error));
// }

function updateItem(uri,getFunc,taskId,isDone,name) {
  //const taskId = document.getElementById("edit-id").value;
  const item = {
    id: parseInt(taskId, 10),
    isDone,
    name,
    //isDone: document.getElementById("edit-isDone").checked,
    //name: document.getElementById("edit-name").value.trim(),
  };

  fetch(`${uri}/${taskId}`, {
    method: "PUT",
    headers: {
      Accept: "application/json",
      "Content-Type": "application/json",
      "Authorization": `Bearer ${localStorage.getItem("token")}`,
    },
    body: JSON.stringify(item),
  })
    .then(() => getFunc())
    .catch((error) => console.error("Unable to update item.", error));

  closeInput();

  return false;
}

function closeInput() {
  document.getElementById("editForm").style.display = "none";
}