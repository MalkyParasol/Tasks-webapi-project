using MyTasks.Models;
using Task = MyTasks.Models.Task;

namespace MyTasks.Interfaces;
public interface ITaskManagementService
{
    List<User> GetAllUsers();

    User? GetUserById(int id);

    User? AddNewUser(Person person);

    bool AddNewTask(Task task, int userID);

    Task? UpdateTask(Task task, int userId,int taskId);

    bool DeleteUser(User user);

    bool DeleteTask(int userId,int taskId);
}

