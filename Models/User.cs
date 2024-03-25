using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;

namespace MyTasks.Models;

public class User:Person{

    public int Id{get;set;}
   //[JsonConverter(typeof(UserTaskConverter))]
    public List<Task> Tasks{get;set;}

    public User():base(){
       
        Tasks = new List<Task>();
    }
    public User(string name, string password,List<Task> tasks,int id):base(name,password){
        Tasks = tasks;
        Id = id;
    }
}

// public class UserTaskConverter : JsonConverter<List<Task>>
// {
//     public override List<Task> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
//     {
//             var tasks = JsonSerializer.Deserialize<List<Task>>(ref reader, options);
//             return tasks != null ? tasks : new List<Task>();
//     }

//     public override void Write(Utf8JsonWriter writer, List<Task> value, JsonSerializerOptions options)
//     {
        
//         JsonSerializer.Serialize(writer, value, options);
//     }
// }