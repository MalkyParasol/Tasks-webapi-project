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
    
    
}