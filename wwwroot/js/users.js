const dom = {
  usersDiv: document.getElementById("usersDiv"),
  newUserForm : document.getElementById("newUserForm"),
  newUserName:document.getElementById("newUserName"),
  newUserPassword:document.getElementById("password"),
  confirmNewUserPassword:document.getElementById("confirmPassword")
};
writeUserName("Hello To Administrator");
function displayNewUserForm(){
    newUserForm.style.display="block";
}
function addNewUser()
{

    if(dom.newUserName.value ==='' || dom.newUserPassword === '' || confirmNewUserPassword === '')
    {
        debugger
        alert("Please fill in all fields");
        debugger
        displayNewUserForm();
    }
    else if(dom.newUserPassword.value != dom.confirmNewUserPassword.value)
    {
        debugger
        alert("Password verification failed, try again");
        debugger
        displayNewUserForm();
    }
    else{
        const user = {
            id: 8,
            tasks:[],
            name:dom.newUserName.value,
            password:dom.newUserPassword.value,
        }
        debugger
        fetch(`/api/user`, {
            method: "POST",
            headers: {
              "Content-Type": "application/json",
              Authorization: `Bearer ${localStorage.getItem("token")}`,
            },
            body: JSON.stringify(user),
          })
            .then((response) => response.json())
            .then((newUser) => {
              debugger
              users.push(newUser);
              debugger
              window.location.reload();
            })
            .catch((error) => console.error("Unable to add item.", error));
        
    }
}

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
    .then((data) => {
      users = data;
      drawUsersDetails();
    })
    .catch((error) => console.error("Unable to get items.", error));
}

function drawUsersDetails() {
  users.forEach((user) => {
    let userContent = document.createElement("div");
    userContent.id = "userContent";
    drawUserTitle(userContent, user);

    let [addForm, taskToAdd] = drawAddForm(userContent);
    let [editFormDiv, editForm, id, isDone, taskName, submitEdit, closeBtn] =
      drawEditForm(userContent);
    let tasksAmount = drawCounter(userContent, user);

    let table = document.createElement("table");
    let tableHead = drawHeadTabel(table);
    let tableBody = document.createElement("tbody");
    addForm.addEventListener("submit", () => {
      const item = {
        id: 0,
        isDone: false,
        name: taskToAdd.value,
      };

      fetch(`/api/user/${user.id}/todo`, {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${localStorage.getItem("token")}`,
        },
        body: JSON.stringify(item),
      })
        .then((response) => response.json())
        .then((newTask) => {
          drawSingleTask(tableBody, newTask);
          user.tasks.push(newTask);
          taskToAdd.value = "";
          window.location.reload();
        })
        .catch((error) => console.error("Unable to add item.", error));
    });
    user.tasks.forEach((task) => {
      let [isDoneInput, td2, editBtn, deleteBtn] = drawSingleTask(
        tableBody,
        task
      );
      editBtn.addEventListener("click", () => {
        editFormDiv.style.display = "block";
        id.innerHTML = task.id;
        isDone.checked = task.isDone;
        taskName.value = task.name;
      });
      deleteBtn.addEventListener("click", () => {
        fetch(`/api/user/${user.id}/todo/${task.id}`, {
          method: "DELETE",
          headers: {
            Authorization: `Bearer ${localStorage.getItem("token")}`,
          },
        })
          .then((response) => {
            if (response.ok) {
              for (const row of tableBody.children) {
                if (row.id == task.id) {
                  user.tasks = user.tasks.filter((t) => t.id != task.id);
                  alert("task deletd succcesfuly");
                  tableBody.removeChild(row);
                  break;
                }
              }
            } else {
              alert("cannot delete this task");
            }
          })
          .catch((error) => console.error("Unable to delete item.", error));
      });
    });
    table.appendChild(tableBody);
    userContent.appendChild(table);
    usersDiv.appendChild(userContent);
    closeBtn.addEventListener("click", () => {
      editFormDiv.style.display = "none";
    });
    closeBtn.addEventListener("mouseover", () => {
      closeBtn.style.cursor = "pointer";
    });
    editForm.addEventListener("submit", () => {
      const item = {
        id: id.innerHTML,
        isDone: isDone.checked,
        name: taskName.value,
      };
      fetch(`/api/user/${user.id}/todo/${id.innerHTML}`, {
        method: "PUT",
        headers: {
          Accept: "application/json",
          "Content-Type": "application/json",
          Authorization: `Bearer ${localStorage.getItem("token")}`,
        },
        body: JSON.stringify(item),
      })
        .then((response) => response.json())
        .then((newTask) => {
          for (const row of tableBody.children) {
            if (row.id == newTask.id) {
              const checkbox = row.querySelector('#td1 input[type="checkbox"]');
              const td2 = row.querySelector("#td2");
              checkbox.checked = newTask.isDone;
              td2.innerHTML = newTask.name;
            }
          }
          const taskIndex = user.tasks.findIndex(
            (task) => task.id == newTask.id
          );
          user.tasks[taskIndex].name = newTask.name;
          user.tasks[taskIndex].isDone = newTask.isDone;
          console.log(users);
          editFormDiv.style.display = "none";
        })
        .catch((error) => console.error("Unable to update item.", error));
    });
    drawRemoveUsers(userContent, user);
  });
}

function drawUserTitle(root, user) {
  let h3 = document.createElement("h3");
  h3.id = "userTitle";
  h3.innerHTML = `The Tasks List of: ${user.name}`;
  root.appendChild(h3);
}
function drawAddForm(root) {
  let h5 = document.createElement("h5");
  h5.innerHTML = "Add";
  root.appendChild(h5);

  let form = document.createElement("form");
  form.id = "addForm";
  form.action = "javascript:void(0);";
  form.method = "POST";

  let taskName = document.createElement("input");
  taskName.type = "text";
  taskName.id = "add-name";
  taskName.placeholder = "New Task";

  let taskSubmit = document.createElement("input");
  taskSubmit.type = "submit";
  taskSubmit.value = "Add";

  form.appendChild(taskName);
  form.appendChild(taskSubmit);
  root.appendChild(form);

  return [form, taskName];
}
function drawEditForm(root) {
  let editFormDiv = document.createElement("div");
  editFormDiv.id = "edit-form-div";
  editFormDiv.style.display = "none";

  let h5 = document.createElement("h5");
  h5.innerHTML = "Edit";
  editFormDiv.appendChild(h5);

  let form = document.createElement("form");
  form.id = "editForm";
  form.action = "javascript:void(0);";
  form.method = "PUT";

  let id = document.createElement("p");
  id.style.display = "none";
  id.id = "edit-id";

  let isDone = document.createElement("input");
  isDone.type = "checkbox";

  let taskName = document.createElement("input");
  taskName.type = "text";
  taskName.id = "edit-name";

  let submitEdit = document.createElement("input");
  submitEdit.type = "submit";
  submitEdit.value = "Save";

  let a = document.createElement("a");
  a.ariaLabel = "Close";
  a.innerHTML = "&#10006;";

  form.appendChild(id);
  form.appendChild(isDone);
  form.appendChild(taskName);
  form.appendChild(submitEdit);
  form.appendChild(a);
  editFormDiv.appendChild(form);
  root.appendChild(editFormDiv);

  return [editFormDiv, form, id, isDone, taskName, submitEdit, a];
}
function drawCounter(root, user) {
  let p = document.createElement("p");
  p.id = "counter";
  let amount = user.tasks.length;
  p.innerHTML = `${amount} ${amount === 1 ? "Task" : "Task kinds"}`;
  root.appendChild(p);

  return p;
}
function drawHeadTabel(table) {
  let tbody = document.createElement("tbody");
  let tr = document.createElement("tr");
  let th1 = document.createElement("th");
  th1.innerHTML = "Is Done?";
  let th2 = document.createElement("th");
  th2.innerHTML = "Name";
  let th3 = document.createElement("th");
  let th4 = document.createElement("th");
  tr.appendChild(th1);
  tr.appendChild(th2);
  tr.appendChild(th3);
  tr.appendChild(th4);
  tbody.appendChild(tr);
  table.appendChild(tbody);
  return tbody;
}
function drawSingleTask(tbody, task) {
  let tr = document.createElement("tr");
  tr.id = task.id;
  let td1 = document.createElement("td");
  td1.id = "td1";
  let isDoneInput = document.createElement("input");
  isDoneInput.type = "checkBox";
  isDoneInput.checked = task.isDone;
  isDoneInput.disabled = true;
  td1.appendChild(isDoneInput);
  let td2 = document.createElement("td");
  td2.id = "td2";
  td2.innerHTML = task.name;
  let td3 = document.createElement("td");
  td3.id = "td3";
  let editBtn = document.createElement("button");
  editBtn.innerHTML = "Edit";
  td3.appendChild(editBtn);
  let td4 = document.createElement("td");
  td4.id = "td4";
  let deleteBtn = document.createElement("button");
  deleteBtn.innerHTML = "Delete";
  td4.appendChild(deleteBtn);
  tr.appendChild(td1);
  tr.appendChild(td2);
  tr.appendChild(td3);
  tr.appendChild(td4);
  tbody.appendChild(tr);
  return [isDoneInput, td2, editBtn, deleteBtn];
}

function drawRemoveUsers(userContent, user) {
  let btn = document.createElement("button");
  btn.id = "remove-user";
  btn.innerHTML = "deleteUser";

  btn.onclick = () => {
    let div = document.createElement("div");
    div.id = "delete-message";

    let p = document.createElement("p");
    p.innerHTML = `are you shure that you want to delete user: ${user.name}?`;

    let btnYes = document.createElement("button");
    btnYes.id = "btnYes";
    btnYes.innerHTML = "Yes";
    btnYes.onclick = () => {
      fetch(`/api/user/${user.id}`, {
        method: "DELETE",
        headers: {
          Authorization: `Bearer ${localStorage.getItem("token")}`,
        },
      })
        .then((response) => {
          if (response.ok) {
            users.filter((u) => u.id != user.id);
            alert("user deleted succesfully!");
            window.location.reload();
          } else {
            alert("cannot delete this user!");
            window.location.reload();
          }
        })
        .catch((error) => console.error("Unable to delete item.", error));
    };

    let btnNo = document.createElement("button");
    btnNo.id = btnNo;
    btnNo.innerHTML = "No";
    btnNo.onclick = () => {
      div.style.display = "none";
    };

    div.appendChild(p);
    div.appendChild(btnYes);
    div.appendChild(btnNo);
    userContent.appendChild(div);
  };
  userContent.appendChild(btn);
}
