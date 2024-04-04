const expirationTime = localStorage.getItem("expirationTime");

const checkTokenExpiration=()=> {
  const now = Math.floor(Date.now()/1000);
  if(expirationTime-now <= 60){
    window.location.href = "../html/login";
  }
}
const dom = {
  title: document.getElementById("title"),
  adminDiv: document.getElementById("adminBtn"),
}

let tasks = [];

function writeUserName(){
  checkTokenExpiration();
  fetch("/api/me",{
    method:"GET",
    headers:{
      Accept: "application/json",
      "Authorization": `Bearer ${localStorage.getItem("token")}`,
    }
  })
  .then((response)=> response.json())
  .then((data) => dom.title.innerHTML = `Hello ${data.name}`)
  .catch((error) => console.error("Unable to get user.", error));
}
writeUserName();

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
      a.href = "../html/users.html"
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

function addItem() {
  const addNameTextbox = document.getElementById("add-name");

  const item = {
    id:0,
    isDone: false,
    name: addNameTextbox.value.trim(),
  };

  fetch("/api/todo", {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
      "Authorization": `Bearer ${localStorage.getItem("token")}`,
    },
    body: JSON.stringify(item),
  })
    .then((response) => response.json())
    .then(() => {
      getItems();
      addNameTextbox.value = "";
      
    })
    .catch((error) => console.error("Unable to add item.", error));
}

function deleteItem(taskId) {
  fetch(`/api/todo/${taskId}`, {
    method: "DELETE",
    headers:{
      "Authorization": `Bearer ${localStorage.getItem("token")}`,
    }
    
  })
    .then(() => getItems())
    .catch((error) => console.error("Unable to delete item.", error));
}

function displayEditForm(id) {
  
  const item = tasks.find((item) => item.id === id);

  document.getElementById("edit-name").value = item.name;
  document.getElementById("edit-id").value = item.id;
  document.getElementById("edit-isDone").checked = item.isDone;
  document.getElementById("editForm").style.display = "block";
}

function updateItem() {
  const taskId = document.getElementById("edit-id").value;
  const item = {
    id: parseInt(taskId, 10),
    isDone: document.getElementById("edit-isDone").checked,
    name: document.getElementById("edit-name").value.trim(),
  };

  fetch(`/api/todo/${taskId}`, {
    method: "PUT",
    headers: {
      Accept: "application/json",
      "Content-Type": "application/json",
      "Authorization": `Bearer ${localStorage.getItem("token")}`,
    },
    body: JSON.stringify(item),
  })
    .then(() => getItems())
    .catch((error) => console.error("Unable to update item.", error));

  closeInput();

  return false;
}

function closeInput() {
  document.getElementById("editForm").style.display = "none";
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

    let editButton = button.cloneNode(false);
    editButton.innerText = "Edit";
    editButton.setAttribute("onclick", `displayEditForm(${item.id})`);

    let deleteButton = button.cloneNode(false);
    deleteButton.innerText = "Delete";
    deleteButton.setAttribute("onclick", `deleteItem(${item.id})`);

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

