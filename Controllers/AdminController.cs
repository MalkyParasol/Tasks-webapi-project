using Microsoft.AspNetCore.Mvc;
using Task = MyTasks.Models.Task;
using MyTasks.Interfaces;
using MyTasks.Services;
using Microsoft.AspNetCore.Authorization;
using MyTasks.Models;
using System.Security.Claims;

namespace MyTasks.Controllers;

[ApiController]
[Route("api/")]
//[Authorize(Policy="Admin")]
public class AdminController : ControllerBase
{
    private IAdminService AdminService;
    public AdminController(IAdminService AdminService){
        this.AdminService = AdminService;
    }

    [HttpPost]
    [Route("[action]")]

    public ActionResult<String> Login([FromBody] Person person)
    {
        
        var claims = new List<Claim>();

        List<User> users = AdminService.GetAllUsers();

           // new Claim("type","Admin");

        if(person.Name=="Malky" && person.Password=="12345")
        {
            claims.Add(new Claim("type","Admin"));
        }
        else{
            User? user = users.Find(u=>u.Name==person.Name&&u.Password==person.Password);
            if(user==null)
                return Unauthorized();
            claims.Add(new Claim("type","User"));
        }

        
        var token = TasksTokenService.GetToken(claims);

        return new OkObjectResult(TasksTokenService.WriteToken(token));
    }
}