using MyTasks.Interfaces;
using MyTasks.Models;
using Task = MyTasks.Models.Task;
// using System.Collections.Generic;
// using System.Linq;
// using System.IO;
// using System;
using System.Text.Json;



namespace MyTasks.Services;
public class TasksService : ITasksService
{
    List<Task> tasks{get;}
    private string fileName = "TasksList.json";
    public TasksService()
    {
        fileName = Path.Combine("Data","TasksList.json");
        using var jsonFile = File.OpenText(fileName);
        tasks = JsonSerializer.Deserialize<List<Models.Task>>(jsonFile.ReadToEnd(),
        new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        }
        ) ?? new List<Task>();

    }

    private void updateJson()
    {
        File.WriteAllText(fileName, JsonSerializer.Serialize(tasks));
    }


    public List<Task> GetAll() => tasks;

    
    public Task? GetById(int id)
    {
        return tasks.FirstOrDefault(p => p.Id == id);

    }
    public int Add(Task newTask)
    {
        if (tasks.Count == 0)
        {
            newTask.Id = 1;
        }
        else
        {
            newTask.Id = tasks.Max(p => p.Id) + 1;
        }
        tasks.Add(newTask);

        updateJson();

        return newTask.Id;
    }

    public bool Update(int id, Task newTask)
    {
        if (id != newTask.Id)
            return false;
        var existingTask = GetById(id);
        if (existingTask == null)
            return false;
        var index = tasks.IndexOf(existingTask);
        if (index == -1)
            return false;
        tasks[index] = newTask;
        updateJson();
        return true;
    }

    public bool Delete(int id)
    {
        var existingTask = GetById(id);
        if (existingTask == null)
            return false;
        var index = tasks.IndexOf(existingTask);
        if (index == -1)
            return false;
        tasks.RemoveAt(index);
        updateJson();
        return true;
    }

    public int Count => tasks.Count();
}

public static class TaskUtils
{
    public static void AddTask(this IServiceCollection services)
    {
        services.AddSingleton<ITasksService, TasksService>();
    }
}