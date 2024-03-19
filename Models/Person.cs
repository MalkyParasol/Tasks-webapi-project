namespace MyTasks.Models;
public class Person{
    public string Name{get;set;}

    public string Password{get;set;}

    public Person(string name,string password)
    {
        Name = name;
        Password = password;
    }
}