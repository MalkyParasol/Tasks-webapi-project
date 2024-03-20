using MyTasks.Models;
using Admin = MyTasks.Models.Admin;
namespace MyTasks.Interfaces;

public interface IAdminService{
    List<User> GetAllUsers(); 

    User? GetUserById(int id);

    User? AddNewUser(Person person);

    bool DeleteUser(User user);
}