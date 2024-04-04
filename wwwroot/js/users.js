const dom = {
  usersDiv: document.getElementById("usersDiv"),
};
writeUserName("Hello To Administrator");

let users = [];

function getUsers() {
  checkTokenExpiration();
  fetch("/api/user", {
    method: "GET",
    headers: {
      Accept: "application/json",
      Authorization: `Bearer ${localStorage.getItem("token")}`,
    },
  })
    .then((response) => response.json())
    .then((data) => displayUsers(data))
    .catch((error) => console.error("Unable to get items.", error));
}

const displayUsers = (usersList) => {
  usersList.forEach((user) => {
    let userContent = document.createElement("div");
    userContent.id = "userConent";

    let title = document.createElement("h3");
    title.innerHTML = `The Tasks List Of ${user.name}`;
    title.id = "userTitle";
    userContent.appendChild(title);

    let addTitle = document.createElement("h5");
    addTitle.innerHTML = "Add";
    userContent.appendChild(addTitle);

    let addForm = document.createElement("form");
    addForm.action = "javascript:void(0);";
    addForm.method = "POST";

    let newTask = document.createElement("input");
    newTask.type = "text";
    newTask.id = "add-name";
    newTask.placeholder = "New Task";
    addForm.appendChild(newTask);

    let add = document.createElement("input");
    add.type = "submit";
    add.value = "Add";
    addForm.appendChild(add);

    addForm.onsubmit = addItem.bind(
      addForm,
      `/api/user/${user.id}/todo`,
      reload,
      newTask
    );

    userContent.appendChild(addForm);
    let editContent = document.createElement("div");
    editContent.id = "edit-form";
    let editTitle = document.createElement("h5");
    editTitle.innerHTML = "Edit";
    editContent.appendChild(editTitle);

    let form = document.createElement("form");
    form.action = "javascript:void(0);";
    let editId = document.createElement("input");
    editId.type = "hidden";
    editId.id = "edit-id";
    form.appendChild(editId);

    let editIsDone = document.createElement("input");
    editIsDone.type = "checkbox";
    editIsDone.id = "edit-isDone";
    form.appendChild(editIsDone);

    let editName = document.createElement("input");
    editName.type = "text";
    editName.id = "edit-name";
    form.appendChild(editName);

    let save = document.createElement("input");
    save.type = "submit";
    save.value = "Save";
    form.appendChild(save);

    let a = document.createElement("a");
    a.addEventListener("click",()=>{
      editContent.style.display="none";
    })
    //a.onclick = closeInput.bind(a);
    a.ariaLabel = "Close";
    a.innerHTML = "&#10006;";
    form.appendChild(a);

    form.onsubmit = updateItem.bind(
      form,
      `api/user/${user.id}/todo`,
      reload,
      editId.value,
      editIsDone.checked,
      editName.value.trim()
    );
    editContent.appendChild(form);
    userContent.appendChild(editContent);

    let counter = document.createElement("p");
    counter.id = "counter";
    const name = user.tasks.length === 1 ? "Task" : "Task kinds";
    counter.innerHTML = `${user.tasks.length} ${name}`;
    userContent.appendChild(counter);
    let table = document.createElement("table");
    let tr = document.createElement("tr");
    let th1 = document.createElement("th");
    th1.innerHTML = "Is Done?";
    tr.appendChild(th1);
    let th2 = document.createElement("th");
    th2.innerHTML = "Name";
    tr.appendChild(th2);
    let th3 = document.createElement("th");
    tr.appendChild(th3);
    let th4 = document.createElement("th");
    tr.appendChild(th4);
    table.appendChild(tr);
    let tbody = document.createElement("tbody");
    tbody.id = "Tasks";
    user.tasks.forEach(task => {
      let trTask = document.createElement("tr");
      let tdIsDone = document.createElement("td");
      let isDoneTask = document.createElement("input");
      isDoneTask.type = "checkbox";
      isDoneTask.checked = task.isDone;
      isDoneTask.disabled=true;
      tdIsDone.appendChild(isDoneTask);
      trTask.appendChild(tdIsDone);
      let tdName = document.createElement("td");
      tdName.innerHTML = task.name;
      trTask.appendChild(tdName);
      let tdEdit = document.createElement("td");
      let editBtn = document.createElement("button");
      editBtn.addEventListener("click",()=>{
        editContent.style.display = "block";
        editName.value=task.name;
        editId.value = task.id;
        editIsDone.checked=task.isDone;
        
      })
      editBtn.innerHTML="Edit";
      tdEdit.appendChild(editBtn);
      trTask.appendChild(tdEdit);
      let tdDelete = document.createElement("td");
      let deleteBtn = document.createElement("button");
      deleteBtn.addEventListener("click",()=>{
        deleteItem(task.id,user.id);
      })
      deleteBtn.innerHTML="Delete";
      tdDelete.appendChild(deleteBtn);
      trTask.appendChild(tdDelete);
      tbody.appendChild(trTask);
    });
    

    table.appendChild(tbody);
    userContent.appendChild(table);

    dom.usersDiv.appendChild(userContent);
  });
};
function reload() {
  dom.usersDiv.innerHTML = "";
  window.location.reload();
}

function deleteItem(userId, taskId) {
  fetch(`api/user/${userId}/todo/${taskId}`, {
    method: "DELETE",
    headers: {
      Authorization: `Bearer ${localStorage.getItem("token")}`,
    },
  })
    .then(() => reload())
    .catch((error) => console.error("Unable to delete item.", error));
}
// //////
// function _displayItems(data) {

//   //const tBody = document.getElementById("Tasks");
//   //tBody.innerHTML = "";

//   //_displayCount(data.length);

//   //const button = document.createElement("button");

//   data.forEach((item) => {
//     //let isDonebox = document.createElement("input");
//     //isDonebox.type = "checkbox";
//     //isDonebox.disabled = true;
//     //isDonebox.checked = item.isDone;

//     let editButton = button.cloneNode(false);
//     editButton.innerText = "Edit";
//     editButton.setAttribute("onclick", `displayEditForm(${item.id})`);

//     let deleteButton = button.cloneNode(false);
//     deleteButton.innerText = "Delete";
//     //deleteButton.setAttribute("onclick", `deleteItem(${item.id})`);
//     deleteButton.setAttribute("onclick", `deleteItem(${item.id}, '/api/todo')`);

//     let tr = tBody.insertRow();

//     let td1 = tr.insertCell(0);
//     td1.appendChild(isDonebox);

//     let td2 = tr.insertCell(1);
//     let textNode = document.createTextNode(item.name);
//     td2.appendChild(textNode);

//     let td3 = tr.insertCell(2);
//     td3.appendChild(editButton);

//     let td4 = tr.insertCell(3);
//     td4.appendChild(deleteButton);
//   });

//   tasks = data;
// }
