using Microsoft.AspNetCore.Mvc;
using Task = MyTasks.Models.Task;
using MyTasks.Interfaces;
using MyTasks.Services;
using Microsoft.AspNetCore.Authorization;

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
    
    [HttpGet]
    [Route("todo")]
    public ActionResult GetToDoList(){
         var userName = User?.FindFirst("UserName")?.Value;
           // System.Console.WriteLine(User);
            return new OkObjectResult($"Public Files Accessed by {userName}");
    }
    
}