using System.Runtime.Serialization.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using MyTasks.Interfaces;
using MyTasks.Models;
using Microsoft.Extensions.Options;
using MyTasks.PasswordHashers;
using NetDevPack.Security.PasswordHasher.Core;
using Task = MyTasks.Models.Task;


namespace MyTasks.Services;

public class AdminService : IAdminService
{


    //List<Models.Task> tasks{get;}



    private readonly IDataAccessService _dataAccessService;

    private List<User> users;
    public AdminService(IDataAccessService dataAccessService)
    {
        _dataAccessService = dataAccessService;
    }

    
    public List<User> GetAllUsers()
    {
        return _dataAccessService.GetAllUsers();
    }

    public User? GetUserById(int id)
    {
       return _dataAccessService.GetUserById(id);
    }

    public bool DeleteUser(User user)
    {
        return _dataAccessService.DeleteUser(user);

    }

    public User? AddNewUser(Person person)
    {

       return _dataAccessService.AddNewUser(person);

    }

    public bool AddNewTask(Task task, int userID)
    {
        return _dataAccessService.AddNewTask(task, userID);
    }
}

public static class AdminUtils
{
    public static void AddAdmin(this IServiceCollection services)
    {
        services.AddSingleton<IAdminService, AdminService>();
       // services.AddTransient<IPasswordHasher<User>, Argon2Id<User>>();
    }

    public static void configurService(this IServiceCollection services)
    {
        services.AddTransient<IPasswordHasher<User>, Argon2Id<User>>();
        //services.AddTransient<IPasswordHasher<User>>((serviceProvider)
        //        =>
        //{
        //    var options = serviceProvider.GetRequiredService<IOptions<ImprovedPasswordHasherOptions>>();
        //    return new Argon2Id<User>(options: options);
        //});
    }
}