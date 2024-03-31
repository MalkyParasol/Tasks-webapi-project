using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;

namespace MyTasks.Models;

public class User:Person{

    public int Id{get;set;}
   
    public List<Task> Tasks{get;set;}

    public User():base(){
       
        Tasks = new List<Task>();
    }
    public User(string name, string password,List<Task> tasks,int id):base(name,password){
        Tasks = tasks;
        Id = id;
    }
}
