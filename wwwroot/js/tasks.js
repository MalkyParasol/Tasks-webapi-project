
const dom = {
  adminDiv: document.getElementById("adminBtn"),
}

let tasks = [];

writeUserName("The Tasks List Of");

function drawAdminBtn(){
  fetch("/api/type",{
    method:"GET",
    headers:{
      Accept: "application/json",
      "Authorization": `Bearer ${localStorage.getItem("token")}`,
    }
  })
  .then((response)=> response.json())
  .then((data) =>{
    if(data.type =="Admin")
    {
      let a = document.createElement("a");
      a.href = "../html/users2.html"
      let btn = document.createElement("button");
      btn.innerHTML = "watch all users";
      a.appendChild(btn);
      dom.adminDiv.appendChild(a);
    }
  })
  .catch((error)=>console.error("unable to get user type",error))
  
}

drawAdminBtn();

function getItems() {
 checkTokenExpiration();
  fetch("/api/todo",{
    method:"GET",
    headers:{
      Accept: "application/json",
      "Authorization": `Bearer ${localStorage.getItem("token")}`,
    }
  })
    .then((response) => response.json())
    .then((data) => _displayItems(data))
    .catch((error) => console.error("Unable to get items.", error));

}

// function displayEditForm(id) {
  
//   const item = tasks.find((item) => item.id === id);

//   document.getElementById("edit-name").value = item.name;
//   document.getElementById("edit-id").value = item.id;
//   document.getElementById("edit-isDone").checked = item.isDone;
//   document.getElementById("editForm").style.display = "block";
// }

function addItem_tasks(uri,getFunc)
{
  const addNameTextbox = document.getElementById("add-name");
  addItem(uri,getFunc,addNameTextbox);
}

function updateItem_tasks(uri,getFunc)
{
   const taskId = document.getElementById("edit-id").value;
   const isDone=document.getElementById("edit-isDone").checked;
   const name= document.getElementById("edit-name").value.trim();
   updateItem(uri,getFunc,taskId,isDone,name);
}
function deleteItem(taskId,uri) {
  //const uri = "/api/todo";
  fetch(`${uri}/${taskId}`, {
    method: "DELETE",
    headers:{
      "Authorization": `Bearer ${localStorage.getItem("token")}`,
    }
    
  })
    .then(() => getItems())
    .catch((error) => console.error("Unable to delete item.", error));
}


function _displayCount(itemCount) {
  const name = itemCount === 1 ? "Task" : "Task kinds";

  document.getElementById("counter").innerText = `${itemCount} ${name}`;
}

function _displayItems(data) {

  const tBody = document.getElementById("Tasks");
  tBody.innerHTML = "";

  _displayCount(data.length);

  const button = document.createElement("button");

  data.forEach((item) => {
    let isDonebox = document.createElement("input");
    isDonebox.type = "checkbox";
    isDonebox.disabled = true;
    isDonebox.checked = item.isDone;

    let editButton = document.createElement('button')
    //let editButton = button.cloneNode(false);
    editButton.innerText = "Edit";
    editButton.addEventListener("click",()=>{
      document.getElementById("edit-name").value = item.name;
     document.getElementById("edit-id").value = item.id;
     document.getElementById("edit-isDone").checked = item.isDone;
     document.getElementById("editForm").style.display = "block";
    })
    //editButton.setAttribute("onclick", `displayEditForm(${item.id})`);

    let deleteButton = button.cloneNode(false);
    deleteButton.innerText = "Delete";
    //deleteButton.setAttribute("onclick", `deleteItem(${item.id})`);
    deleteButton.setAttribute("onclick", `deleteItem(${item.id}, '/api/todo')`);

    let tr = tBody.insertRow();

    let td1 = tr.insertCell(0);
    td1.appendChild(isDonebox);

    let td2 = tr.insertCell(1);
    let textNode = document.createTextNode(item.name);
    td2.appendChild(textNode);

    let td3 = tr.insertCell(2);
    td3.appendChild(editButton);

    let td4 = tr.insertCell(3);
    td4.appendChild(deleteButton);
  });

  tasks = data;
}

