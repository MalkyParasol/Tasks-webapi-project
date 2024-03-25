using MyTasks.Models;
using Task = MyTasks.Models.Task;
namespace MyTasks.Interfaces
{
    public interface IDataAccessService
    {
        List<User> GetAllUsers();

        User? GetUserById(int id);

        bool DeleteUser(User user);

        User? AddNewUser(Person person);

        bool AddNewTask(Task task,int userId);

        Task? UpdateTask(Task task, int userId);

    }
}
