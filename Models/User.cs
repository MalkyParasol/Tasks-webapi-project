namespace MyTasks.Models;

public class User:Person{

    public List<Task> Tasks{get;set;}

    public User(string name, string password,List<Task> tasks):base(name,password){
        Tasks = tasks;
    }
}