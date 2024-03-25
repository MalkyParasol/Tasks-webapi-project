using MyTasks.Interfaces;
using MyTasks.Models;
using Task = MyTasks.Models.Task;
using MyTasks.Services;
using System.Text.Json;

namespace MyTasks.Services
{
    public class DataAccessService : IDataAccessService
    {
        private  string fileName = "usersList.json";

        private List<User> users;

        public DataAccessService()
        {
            fileName = Path.Combine("Data", "usersList.json");
            AccessUsers();
        }

        private void AccessUsers()
        {
            
            
          //  fileName = Path.Combine("Data", "usersList.json");
            using var jsonFile = File.OpenText(fileName);
            users = JsonSerializer.Deserialize<List<Models.User>>(jsonFile.ReadToEnd(),
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new List<User>();
            jsonFile.Close();
            Console.WriteLine(users.Select(u =>( u.Id == 2 )));
        }

        

        List<User> IDataAccessService.GetAllUsers()
        {
           return users;
        }

        private void updateJson()
        {
            
            File.WriteAllText(fileName, JsonSerializer.Serialize(users));
            AccessUsers();
            
           
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
                User newUser = new User(person.Name!, person.Password!, new List<Models.Task>(), (id + 1));
                users.Add(newUser);
                updateJson();
                return newUser;
            }
            return null;

        }

        public User? GetUserById(int id)
        {
            return users.Find(u => u.Id == id);
        }

        public bool AddNewTask(Task task, int userId)
        {
            User? currentUser = users.Find(u => u.Id == userId);
            if (currentUser == null)
                return false;
           
            int id = currentUser.Tasks.Max(t => t.Id);
            task.Id = id+1;
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

        public Task? UpdateTask(Task task, int userId)
        {
            User? updatedUserTask = GetUserById(userId);
            if (updatedUserTask == null)
                return null;
            foreach(var t in  updatedUserTask.Tasks)
            {
                if(t.Id==task.Id)
                {
                    t.Name = task.Name; 
                    t.IsDone = task.IsDone;
                    updateJson();
                    return t;
                }
            }
            return null;
            

        }
    }


}

public static class DataAccessUtils
{
    public static void AddDataAccess(this IServiceCollection services)
    {
        services.AddSingleton<IDataAccessService, DataAccessService>();
    }
}
