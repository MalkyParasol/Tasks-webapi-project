using System.Runtime.Serialization.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using MyTasks.Interfaces;
using MyTasks.Models;
using Microsoft.Extensions.Options;
using MyTasks.PasswordHashers;
using NetDevPack.Security.PasswordHasher.Core;


namespace MyTasks.Services;

public class AdminService : IAdminService
{


    //List<Models.Task> tasks{get;}



    private readonly string fileName = "usersList.json";

    private List<User> users;
    public AdminService()
    {
        fileName = Path.Combine("Data", "usersList.json");
        using var jsonFile = File.OpenText(fileName);
        users = JsonSerializer.Deserialize<List<Models.User>>(jsonFile.ReadToEnd(),
        new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        }) ?? new List<User>();
    }

    private void updateJson()
    {
        File.WriteAllText(fileName, JsonSerializer.Serialize(users));
        //set users anew
    }
    public List<User> GetAllUsers()
    {
        return users;
    }

    public User? GetUserById(int id)
    {
        return users.Find(u => u.Id == id);
    }

    public bool DeleteUser(User user)
    {
        try
        {
            users.Remove(user);
            updateJson();
            return true;
        }
        catch (Exception ex)
        {
            System.Console.WriteLine($"Error deleting user: {ex.Message}");
            return false;
        }


    }

    public User? AddNewUser(Person person)
    {

        User? existingUser = users.Find(u => u.Password == person.Password && u.Name == person.Name);
        if (existingUser == null)
        {
            int id = users.Max(u => u.Id);
            User newUser = new User(person.Name!, person.Password!, new List<Models.Task>(), id + 1);
            users.Add(newUser);
            updateJson();
            return newUser;
        }
        return null;

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