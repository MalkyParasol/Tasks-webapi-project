using MyTasks.Interfaces;
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
    // private void WriteErrorLog(string message)
    // {
    //     if (!string.IsNullOrEmpty(message))
    //     {
    //         using (StreamWriter writer = new StreamWriter("Loggers/errors.txt",true))
    //         {
    //             writer.WriteLine(message);
    //         }
    //     }
    // }
    private void AccessUsers()
    {
        try
        {
            using var jsonFile = File.OpenText(fileName);
            users = JsonSerializer.Deserialize<List<User>>(jsonFile.ReadToEnd(),
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new List<User>();
            jsonFile.Close();
        } 
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            //WriteErrorLog($"Error loading user data: {ex.Message}");
            users = new List<User>();
        }
       
        
    }

    private void updateJson()
    {
        try
        {
            File.WriteAllText(fileName, JsonSerializer.Serialize(users));
            AccessUsers();
        }
        catch(Exception ex)
        {
            System.Console.WriteLine(ex);
            //WriteErrorLog($"Error saving user data: {ex.Message}");
        }  
        
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

    public Task? AddNewTask(Task task, int userId)
    {
       
        User? currentUser = users.Find(u => u.Id == userId);
        if (currentUser == null)
            return null;

        int id = 0;
        if (currentUser.Tasks.Count != 0)
        {
        //    var tasksCopy = currentUser.Tasks.ToList(); // Create a copy to avoid modification issues
        //    id = tasksCopy.Max(t => t.Id); // Use the copy for enumeration
           id = currentUser.Tasks.Max(t => t.Id);
        }


        task.Id = id + 1;
        //task.Id =int.Parse( Guid.NewGuid().ToString());
        //task.Id = Guid.NewGuid().ToString()
        currentUser.Tasks.Add(task);
        foreach (var user in users)
        {
            if (user.Id == currentUser.Id)
            {
                user.Tasks = currentUser.Tasks;
            }
        }

        updateJson();
        return task;
        
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
            System.Console.WriteLine(ex);
           // WriteErrorLog($"Error deleting user: {ex.Message}");
            return false;
        }
    }
    public bool DeleteTask(int userId,int TaskId)
    {
        User? user = users.Find(u => u.Id == userId);
        if(user == null)
        {
            throw new Exception("user not found");
        }
        foreach (var task in user.Tasks)
        {
            if (task.Id == TaskId)
            {
                user.Tasks.Remove(task);
                updateJson();
                return true;
            }
        }
        return false;
        }
}

public static class TaskManagementUtils
{
    public static void AddTaskManagement(this IServiceCollection services)
    {
        services.AddSingleton<ITaskManagementService, TaskManagementService>();
    }
}

