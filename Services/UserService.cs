

using MyTasks.Interfaces;
using MyTasks.Models;
using Task = MyTasks.Models.Task;

namespace MyTasks.Services;

public class UserService : IUserService
{
    private readonly IDataAccessService _dataAccessService;

    private User user;

    public UserService(IDataAccessService dataAccessService)
    {
        _dataAccessService = dataAccessService;
    }

    public bool AddNewTask(Task task,int userId)
    {
        return _dataAccessService.AddNewTask(task, userId);
    }

    public User? GetUserById(int id)
    {
        return _dataAccessService.GetUserById(id);
    }

    public Task? UpdateTask(Task task, int userID)
    {
        return _dataAccessService.UpdateTask(task, userID); 
    }
}

public static class UserUtils
{
    public static void AddUser(this IServiceCollection services)
    {
        services.AddSingleton<IUserService, UserService>();
    }
}