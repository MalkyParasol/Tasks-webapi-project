﻿using MyTasks.Interfaces;
using MyTasks.Models;
using System.Text.Json;
using Task = MyTasks.Models.Task;

namespace MyTasks.Services;
public class TaskManagementService:ITaskManagementService
{
    private string fileName;

    private List<User> users;
    public TaskManagementService() 
    {
        users = new List<User>();
        fileName = Path.Combine("Data", "usersList.json");
        AccessUsers();
    }
    private void AccessUsers()
    {
        using var jsonFile = File.OpenText(fileName);
        users = JsonSerializer.Deserialize<List<User>>(jsonFile.ReadToEnd(),
        new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        }) ?? new List<User>();
        jsonFile.Close();
    }

    private void updateJson()
    {
        File.WriteAllText(fileName, JsonSerializer.Serialize(users));
        AccessUsers();
    }

    public List<User> GetAllUsers()
    {
        return users;
    }

    public User? GetUserById(int id)
    {
        return users.Find(u => u.Id == id);
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

    public bool AddNewTask(Task task, int userId)
    {
        User? currentUser = users.Find(u => u.Id == userId);
        if (currentUser == null)
            return false;
        int id=0;
        if (currentUser.Tasks.Count != 0)
        {
            id = currentUser.Tasks.Max(t => t.Id);
        }
            
        
        task.Id = id + 1;
        currentUser.Tasks.Add(task);
        foreach (var user in users)
        {
            if (user.Id == currentUser.Id)
            {
                user.Tasks = currentUser.Tasks;
            }
        }

        updateJson();
        return true;
    }

    public Task? UpdateTask(Task task, int userId,int taskId)
    {
        User? user = GetUserById(userId);
        if (user == null)
            return null;
        foreach (var t in user.Tasks)
        {
            if (t.Id == taskId)
            {
                t.Name = task.Name;
                t.IsDone = task.IsDone;
                updateJson();
                return t;
            }
        }
        return null;


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
}

public static class TaskManagementUtils
{
    public static void AddTaskManagement(this IServiceCollection services)
    {
        services.AddSingleton<ITaskManagementService, TaskManagementService>();
    }
}

