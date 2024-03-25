using MyTasks.Models;
using Task = MyTasks.Models.Task;
using User = MyTasks.Models.User;
namespace MyTasks.Interfaces;

public interface IUserService{
    User? GetUserById(int id);

    bool AddNewTask(Task task,int userID);
    Task? UpdateTask(Task task,int userID);
}