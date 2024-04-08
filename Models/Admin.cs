namespace MyTasks.Models;

public class Admin : Person
{
    public int Id { get; set; }
    public Admin(string name, string password, int id) : base(name, password)
    {
        Id = id;
    }
}