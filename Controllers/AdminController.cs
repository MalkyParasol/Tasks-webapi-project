using Microsoft.AspNetCore.Mvc;
using Task = MyTasks.Models.Task;
using MyTasks.Interfaces;
using MyTasks.Services;
using Microsoft.AspNetCore.Authorization;
using MyTasks.Models;
using System.Security.Claims;
using Microsoft.VisualBasic;
using Microsoft.AspNetCore.Identity;

namespace MyTasks.Controllers;

[ApiController]
[Route("api/")]
//[Authorize(Policy="Admin")]
public class AdminController : ControllerBase
{
    private IAdminService AdminService;
    //---------
    private IPasswordHasher<User> PasswordHasher;
    //---------
    public AdminController(IAdminService AdminService, IPasswordHasher<User> passwordHasher)
    {
        //,IPasswordHasher<User> PasswordHasher
        this.AdminService = AdminService;
        PasswordHasher = passwordHasher;
        //-----------
        // this.PasswordHasher = PasswordHasher;
        //-----------
    }

    [HttpPost]
    [Route("[action]")]

    public ActionResult<string> Login([FromBody] Person person)
    {
        
        var claims = new List<Claim>();

       

       // List<User> users = AdminService.GetAllUsers();

           // new Claim("type","Admin");

        if(person.Name=="Malky" && person.Password=="12345")
        {
            claims.Add(new Claim("type","Admin"));
            claims.Add(new Claim("id","1"));
        }
        else{
            User? user = AdminService.GetAllUsers().FirstOrDefault(u => u.Name == person.Name && PasswordHasher.VerifyHashedPassword(u, u.Password!, person.Password??"") == PasswordVerificationResult.Success);
            //User? user = users.Find(u=>u.Name==person.Name&& u.Password==person.Password);
            if(user==null)
                return Unauthorized();
            claims.Add(new Claim("type","User"));
            claims.Add(new Claim("id", user.Id.ToString()));
        }
        claims.Add(new Claim("userName", person.Name!));
            
                  //  new Claim("password",person.Password!)};

        
        var token = TasksTokenService.GetToken(claims);

        return new OkObjectResult(TasksTokenService.WriteToken(token));
    }

    [HttpGet]
    [Route("user")]
    [Authorize(Policy ="Admin")]

    public ActionResult GetAllUsers(){
         
        return Ok(AdminService.GetAllUsers().Select(u => new { id = u.Id, name = u.Name, password = u.Password }).ToList());
    }
    [HttpPost]
    [Route("user")]
    [Authorize(Policy = "Admin")]
    public IActionResult AddNewUser([FromBody] Person person)
    {
        
        if (person == null ||string.IsNullOrEmpty(person.Name) || string.IsNullOrEmpty(person.Password))
        {
            return BadRequest("Missing username or password");
        }
        //------
       var hashedPassword = PasswordHasher.HashPassword(person.Password);
        person.Password= hashedPassword;
        //----
        User? newUser = AdminService.AddNewUser(person);
        if(newUser==null)
        {
            //----
            return Conflict("User with the same name already exists");
            //---
            // return Conflict("Username and password already exist in the system");
        }
        else
        {
            
            return Ok(newUser);
        }
             
    }

    [HttpDelete]
    [Route("user/{id}")]
    [Authorize(Policy = "Admin")]

    public IActionResult DeleteUser(int id)
    {
        var user = AdminService.GetUserById(id);
        if(user == null)
        {
            return NotFound();
        }
        if(user.Name=="Malky" && user.Password=="12345")
            return BadRequest("The administrator cannot be deleted");
        if(!AdminService.DeleteUser(user))
        {
             return BadRequest(new { message = "An error occurred while deleting the user." });
        }

        return Ok(new { message = "The user was deleted successfully." });
    }
    [HttpGet]
    [Route("user/todo")]
    [Authorize(Policy = "Admin")]
    public IActionResult GetToDoItems()
    {
        var users = AdminService.GetAllUsers();
        List<Task> tasks = users.SelectMany(u => u.Tasks).ToList();
        return Ok(tasks);
    }

    [HttpGet]
    [Route("user{userId}/todo{taskId}")]
    [Authorize(Policy = "Admin")]
    public IActionResult GetToDoById(int userId,int taskId)
    {
        var user = AdminService.GetUserById(userId);    
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
    [Route("user/{userId}/todo")]
    [Authorize(Policy = "Admin")]
    public IActionResult AddNewTask([FromBody] Task task,int userId)
    {

        if (AdminService.GetUserById(userId) == null)
            return NotFound("user not found!");
        if (!AdminService.AddNewTask(task, userId))
            return BadRequest("can not add this task!");
        return Ok("task added succesfully!");
    }
}