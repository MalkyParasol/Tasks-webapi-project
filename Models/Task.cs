namespace MyTasks.Models;

public class Task
{
    public int Id { get; set;}
    public string Name { get; set;}
    public bool IsDone {get; set;}
    
    public Task(int id,string name, bool isDone)
    {
        Id=id;
        Name = name;
        IsDone = isDone;
    }
}
