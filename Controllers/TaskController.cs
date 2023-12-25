using Microsoft.AspNetCore.Mvc;
using MyTasks.Models;
using MyTasks.Services;
using Task = MyTasks.Models.Task;


namespace MyTasks.Controllers;

[ApiController]
[Route("[controller]")]
public class TaskController : ControllerBase
{
    [HttpGet]
    public ActionResult<List<Task>> Get()
    {
        return TasksService.GetAll();
    }

    [HttpGet("{id}")]
    public ActionResult<Task> Get(int id)
    {
        var task = TasksService.GetById(id);
        if (task == null)
            return NotFound();
        return task;
    }

    [HttpPost]
    public ActionResult Post(Task newTask)
    {
        var newId = TasksService.Add(newTask);

        return CreatedAtAction("Post", 
            new {id = newId}, TasksService.GetById(newId));
    }
    [HttpPut("{id}")]
    public ActionResult Put(int id,Task newTask)
    {
        var result = TasksService.Update(id, newTask);
        if (!result)
        {
            return BadRequest();
        }
        return NoContent();
    }

    
}
