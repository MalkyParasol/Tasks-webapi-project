using MyTasks.Models;
using Task = MyTasks.Models.Task;
namespace MyTasks.Services;

public static class TasksService
{
    private static List<Task> tasks;
    static TasksService()
    {
        tasks = new List<Task>
        {
            new Task {Id = 1,Name = "Learning for c# test", IsDone = false},
            new Task {Id = 2,Name = "Learning for Data Struct test", IsDone = false},
            new Task {Id = 3,Name = "Learning for Operating Systems test", IsDone = false},
        };
    }
    public static List<Task> GetAll() => tasks;

    public static Task GetById(int id)
    {
        return tasks.FirstOrDefault(p => p.Id == id);
    }
    public static int Add(Task newTask)
    {
        if(tasks.Count==0)
        {
            newTask.Id  =1;
        }
        else
        {
            newTask.Id = tasks.Max(p => p.Id)+1;
        }
        tasks.Add(newTask);

        return newTask.Id;
    }

    public static bool Update(int id,Task newTask)
    {
        if(id!=newTask.Id)
            return false;
        var existingTask = GetById(id);
        if(existingTask == null)
            return false;
        var index = tasks.IndexOf(existingTask);
        if(index==-1)
            return false;
        tasks[index] = newTask;

        return true;
    }

    public static bool Delete(int id)
    {
        var existingTask = GetById(id);
        if(existingTask==null)
            return false;
        var index = tasks.IndexOf(existingTask);
        if(index ==-1)
            return false;
        tasks.RemoveAt(index);
        return true;
    }
}