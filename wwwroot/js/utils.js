const utilsDom = {
  title: document.getElementById("userTitle"),
}
const expirationTime = localStorage.getItem("expirationTime");

const checkTokenExpiration=()=> {
  const now = Math.floor(Date.now()/1000);
  if(expirationTime-now <= 60){
    window.location.href = "../html/login";
  }
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
