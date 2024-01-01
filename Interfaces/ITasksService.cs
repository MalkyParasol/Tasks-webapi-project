using MyTasks.Models;
using Task = MyTasks.Models.Task;
namespace MyTasks.Interfaces;


public interface ITasksService
{
    List<Task> GetAll(); 
    Task? GetById(int id);
    int Add(Task newTask);
    bool Update(int id,Task newTask);
    bool Delete(int id);

    int Count {get;}
}