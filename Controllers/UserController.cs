using Microsoft.AspNetCore.Mvc;
using Task = MyTasks.Models.Task;
using MyTasks.Interfaces;
using MyTasks.Services;
using Microsoft.AspNetCore.Authorization;
using MyTasks.Models;
using System.Reflection.Metadata.Ecma335;

namespace MyTasks.Controllers;

[ApiController]
[Route("api/")]
[Authorize(Policy ="User")]
public class UserController : ControllerBase
{
    private IUserService UserService;

    public UserController(IUserService UserService)
    {
        this.UserService = UserService;
    }


    private User? GetUserFromClaims()
    {
        string id = User?.FindFirst("id")?.Value!;
        if (id == null)
        {
            return null;
        }
        User user = UserService.GetUserById(int.Parse(id))!;
        if (user == null)
        {
            return null;
        }
        return user;
    }


    [HttpGet]
    [Route("todo")]
    public ActionResult GetToDoList(){

        User? user = GetUserFromClaims();
        if(user == null)
        {
            return NotFound("user not found");
        }
        List<Task> tasks = user.Tasks;   
          
        return  Ok(tasks);
    }
    [HttpGet]
    [Route("todo/{taskId}")]
    public IActionResult GetToDoById(int taskId)
    {

        User? user = GetUserFromClaims();
        if (user == null)
        {
            return NotFound("user not found");
        }
        Task task = user.Tasks.Find(t=>t.Id == taskId)!;
        if (task == null)
        {
            return NotFound("task not found");
        }
        return Ok(task);
        
    }
    [HttpPost]
    [Route("todo")]
    public IActionResult AddNewTask([FromBody] Task task)
    {
        string? id = User.FindFirst("id")?.Value;
        if(id==null)
        {
            return NotFound("user not found");
        }
        if (!UserService.AddNewTask(task, int.Parse(id)))
            return BadRequest("can not add this task!");
        return Ok("task added succesfully!");
    }
    [HttpPut]
    [Route("todo/{taskId}")]
    public IActionResult UpdateTask([FromBody] Task task,int taskId)
    {
        if(taskId!=task.Id)
        {
            task.Id = taskId;
        }
        Task updatedTask = UserService.UpdateTask(task, taskId);
        if (updatedTask == null)
            return BadRequest("can not update this task");
        return Ok(updatedTask);
    }
    
}