using MyTasks.Models;
using Admin = MyTasks.Models.Admin;
using Task = MyTasks.Models.Task;
namespace MyTasks.Interfaces;

public interface IAdminService{
    List<User> GetAllUsers(); 

    User? GetUserById(int id);

    User? AddNewUser(Person person);

    bool DeleteUser(User user);

    bool AddNewTask(Task task, int userID);
}