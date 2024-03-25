

using MyTasks.Interfaces;

namespace MyTasks.Services;
public class UserService : IUserService{

}

public static class UserUtils
{
    public static void AddUser(this IServiceCollection services)
    {
        services.AddSingleton<IUserService, UserService>();
    }
}