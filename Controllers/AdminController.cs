using Microsoft.AspNetCore.Mvc;
using Task = MyTasks.Models.Task;
using MyTasks.Interfaces;
using MyTasks.Services;
using Microsoft.AspNetCore.Authorization;
using MyTasks.Models;
using System.Security.Claims;
using Microsoft.VisualBasic;
using Microsoft.AspNetCore.Identity;
using System;

namespace MyTasks.Controllers;

[ApiController]
[Route("api/user/")]
[Authorize(Policy="Admin")]
public class AdminController : ControllerBase
{
    private ITaskManagementService _taskManagementService;
    private IPasswordHasher<User> PasswordHasher;
    
    public AdminController(ITaskManagementService taskManagementService, IPasswordHasher<User> passwordHasher)
    {
        _taskManagementService = taskManagementService;
        PasswordHasher = passwordHasher;
    }


    [HttpGet]
    public ActionResult GetAllUsers(){
         
        return Ok(_taskManagementService.GetAllUsers().Select(u => new { id = u.Id, name = u.Name, password = u.Password }).ToList());
    }

    [HttpGet]
    [Route("me")]
    public ActionResult GetMyUser()
    {
        return Ok(_taskManagementService.GetAllUsers().Find(u => u.Name == "Malky" && PasswordHasher.VerifyHashedPassword( u.Password ?? "", "12345") == PasswordVerificationResult.Success));
    }
    [HttpPost]
    public IActionResult AddNewUser([FromBody] Person person)
    {
        
        if (person == null ||string.IsNullOrEmpty(person.Name) || string.IsNullOrEmpty(person.Password))
        {
            return BadRequest("Missing username or password");
        }
        
        var hashedPassword = PasswordHasher.HashPassword(person.Password);
        person.Password= hashedPassword;
  
        User? newUser = _taskManagementService.AddNewUser(person);
        if(newUser==null)
        {
            return Conflict("User with the same name already exists");
        }
        else
        {
            return Ok(newUser);
        }
             
    }

    [HttpDelete]
    [Route("{id}")]
    public IActionResult DeleteUser(int id)
    {
        var user = _taskManagementService.GetUserById(id);
        if(user == null)
        {
            return NotFound();
        }
        if(user.Name=="Malky" && PasswordHasher.VerifyHashedPassword( user.Password!, "12345" ) == PasswordVerificationResult.Success) 
            return BadRequest("The administrator cannot be deleted");
        if(!_taskManagementService.DeleteUser(user))
        {
             return BadRequest(new { message = "An error occurred while deleting the user." });
        }

        return Ok(new { message = "The user was deleted successfully." });
    }
    [HttpGet]
    [Route("todo")]
    public IActionResult GetToDoItems()
    {
        var users = _taskManagementService.GetAllUsers();
        List<Task> tasks = users.SelectMany(u => u.Tasks).ToList();
        return Ok(tasks);
    }

    [HttpGet]
    [Route("{userId}/todo{taskId}")]
    public IActionResult GetToDoById(int userId,int taskId)
    {
        var user = _taskManagementService.GetUserById(userId);    
        if(user == null)
        {
            return NotFound("user not found");
        }
        var task = user.Tasks.Find(t => t.Id == taskId);
        if(task == null)
        {
            return NotFound("task not found");
        }
        return Ok(task);
    }
    [HttpPost]
    [Route("{userId}/todo")]
    public IActionResult AddNewTask([FromBody] Task task,int userId)
    {

        if (_taskManagementService.GetUserById(userId) == null)
            return NotFound("user not found!");
        if (!_taskManagementService.AddNewTask(task, userId))
            return BadRequest("can not add this task!");
        return Ok("task added succesfully!");
    }
    [HttpPut]
    [Route("{userId}/todo/{taskId}")]
    public IActionResult UpdateUsersToDoItem([FromBody] Task task, int userId,int taskId) 
    {
        if (taskId != task.Id)
        {
            return BadRequest("id and taskId must be the same!");
        }
        Task? ApdatedTask = _taskManagementService.UpdateTask(task, userId,taskId);
        if (ApdatedTask == null)
            return NotFound("user or task id not found!");
        return Ok(ApdatedTask);


    }
    [HttpDelete]
    [Route("{userId}/todo/{taskId}")]
    public IActionResult DeleteTask(int userId, int taskId) 
    {
        try
        {
            if (!_taskManagementService.DeleteTask(userId, taskId))
                return BadRequest("cannot delete this task");
            else
                return Ok("task deleted succesfully");
        }
        catch(Exception ex)
        {
            return BadRequest(ex.Message);
        }
        
    }
}