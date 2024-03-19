using System.Text.Json;
using MyTasks.Interfaces;
using MyTasks.Models;

namespace MyTasks.Services;

public class AdminService : IAdminService
{
    List<User> users{get;}

     

    private string fileName = "usersList.json";
    public AdminService(){
        fileName = Path.Combine("Data","usersList.json");
        using var jsonFile = File.OpenText(fileName);
        users = JsonSerializer.Deserialize<List<Models.User>>(jsonFile.ReadToEnd(),
        new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive=true
        })?? new List<User>();
    }
    public List<User> GetAllUsers()
    {
        return users;
    }

    
}

public static class AdminUtils
{
    public static void AddAdmin(this IServiceCollection services)
    {
        services.AddSingleton<IAdminService, AdminService>();
    }
}