using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyTasks.Interfaces;
using MyTasks.Models;
using MyTasks.Services;
using System.Security.Claims;

namespace MyTasks.Controllers;

[ApiController]
[Route("api/")]
public class AuthenticationController:ControllerBase
{
    private ITaskManagementService _taskManagementService;

    private IPasswordHasher<User> _passwordHasher;

    public AuthenticationController(ITaskManagementService taskManagementService, IPasswordHasher<User> passwordHasher)
    {
        _taskManagementService = taskManagementService;
        _passwordHasher = passwordHasher;
    }

    [HttpPost]
    [Route("[action]")]

    public ActionResult<string> Login([FromBody] Person person)
    {

        var claims = new List<Claim>();
        if(person == null || person!.Password==null || person.Name==null)
        {
            return BadRequest($"person or his properties are null");
        }
        Admin? admin = TasksTokenService.GetAdmins().Find(a => a.Name == person.Name && a.Password!=null && _passwordHasher.VerifyHashedPassword(a.Password,person.Password)== PasswordVerificationResult.Success);   
        if(admin!=null)
        {
            claims.Add(new Claim("type", "Admin"));
            claims.Add(new Claim("id", "1"));
        }
        else
        {
            User? user = _taskManagementService.GetAllUsers().FirstOrDefault(u => u.Name == person.Name && _passwordHasher.VerifyHashedPassword( u.Password!, person.Password ?? "") == PasswordVerificationResult.Success);
            if (user == null)
                return Unauthorized();
            claims.Add(new Claim("type", "User"));
            claims.Add(new Claim("id", user.Id.ToString()));
        }
        
        claims.Add(new Claim("userName", person.Name!));

        var token = TasksTokenService.GetToken(claims);

        return new OkObjectResult(TasksTokenService.WriteToken(token));
    }
}

