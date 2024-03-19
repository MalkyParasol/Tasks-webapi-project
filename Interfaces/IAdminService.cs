using MyTasks.Models;
using Admin = MyTasks.Models.Admin;
namespace MyTasks.Interfaces;

public interface IAdminService{
    List<User> GetAllUsers(); 
}