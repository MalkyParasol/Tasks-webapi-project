namespace MyTasks.Models;

public class Admin:Person{
    public List<User> Users{get;set;}
    public Admin(string name,string password,List<User> users):base(name,password)
    {
        Users = users;
    }
}