const dom = {
  userTitle: document.getElementById("userTitle"),
  tableBody: document.getElementById("tableBody"),
  editFormDiv: document.getElementById("edit-form-div"),
  editForm: document.getElementById("editForm"),
  id: document.getElementById("edit-id"),
  isDone: document.getElementById("edit-isDone"),
  taskName: document.getElementById("edit-name"),
  closeBtn: document.getElementById("closeBtn"),
  addForm: document.getElementById("addForm"),
  taskToAdd: document.getElementById("add-name"),
  adminDiv: document.getElementById("adminBtn"),
  counter: document.getElementById("counter"),
};
let tasks = [];
const userId = 0;

writeUserName("the Tasks List of: ");

function getTasks() {
  checkTokenExpiration();
  fetch("/api/todo", {
    method: "GET",
    headers: {
      Accept: "application/json",
      Authorization: `Bearer ${localStorage.getItem("token")}`,
    },
  })
    .then((response) => response.json())
    .then((data) => {
      tasks = data;
      drawUserDetails();
    })
    .catch((error) => console.error("Unable to get items.", error));
}

function drawUserDetails() {
  let amount = tasks.length;
  dom.counter.innerHTML = `${amount} ${amount === 1 ? "Task" : "Task kinds"}`;
  dom.addForm.addEventListener("submit", () => {
    const item = {
      id: 0,
      isDone: false,
      name: dom.taskToAdd.value,
    };

    fetch(`/api/todo`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${localStorage.getItem("token")}`,
      },
      body: JSON.stringify(item),
    })
      .then((response) => response.json())
      .then((newTask) => {
        drawSingleTask(dom.tableBody, newTask);
        tasks.push(newTask);
        dom.taskToAdd.value = "";
        window.location.reload();
      })
      .catch((error) => console.error("Unable to add item.", error));
  });
  tasks.forEach((task) => {
    let [isDoneInput, td2, editBtn, deleteBtn] = drawSingleTask(
      dom.tableBody,
      task
    );
    editBtn.addEventListener("click", () => {
      dom.editFormDiv.style.display = "block";
      dom.id.innerHTML = task.id;
      dom.isDone.checked = task.isDone;
      dom.taskName.value = task.name;
    });
    deleteBtn.addEventListener("click", () => {
      fetch(`/api/todo/${task.id}`, {
        method: "DELETE",
        headers: {
          Authorization: `Bearer ${localStorage.getItem("token")}`,
        },
      })
        .then((response) => {
          if (response.ok) {
            for (const row of dom.tableBody.children) {
              if (row.id == task.id) {
                tasks = tasks.filter((t) => t.id != task.id);
                dom.tableBody.removeChild(row);
                alert("task deletd succcesfuly");
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
  dom.closeBtn.addEventListener("click", () => {
    dom.editFormDiv.style.display = "none";
  });
  dom.editForm.addEventListener("submit", () => {
    const item = {
      id: dom.id.innerHTML,
      isDone: dom.isDone.checked,
      name: dom.taskName.value,
    };
    fetch(`/api/todo/${dom.id.innerHTML}`, {
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
        for (const row of dom.tableBody.children) {
          if (row.id == newTask.id) {
            const checkbox = row.querySelector('#td1 input[type="checkbox"]');
            const td2 = row.querySelector("#td2");
            checkbox.checked = newTask.isDone;
            td2.innerHTML = newTask.name;
          }
        }
        const taskIndex = tasks.findIndex((task) => task.id == newTask.id);
        tasks[taskIndex].name = newTask.name;
        tasks[taskIndex].isDone = newTask.isDone;
        console.log(tasks);
        dom.editFormDiv.style.display = "none";
      })
      .catch((error) => console.error("Unable to update item.", error));
  });
  drawAdminBtn();
}

function drawAdminBtn() {
  fetch("/api/type", {
    method: "GET",
    headers: {
      Accept: "application/json",
      Authorization: `Bearer ${localStorage.getItem("token")}`,
    },
  })
    .then((response) => response.json())
    .then((data) => {
      if (data.type == "Admin") {
        let a = document.createElement("a");
        a.href = "../html/users.html";
        let btn = document.createElement("button");
        btn.innerHTML = "watch all users";
        a.appendChild(btn);
        dom.adminDiv.appendChild(a);
      }
    })
    .catch((error) => console.error("unable to get user type", error));
}
