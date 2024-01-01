using Microsoft.AspNetCore.Mvc;
using Task = MyTasks.Models.Task;
using MyTasks.Interfaces;
using MyTasks.Services;

namespace MyTasks.Controllers;

[ApiController]
[Route("[controller]")]
public class TaskController : ControllerBase
{
    private ITasksService TasksService;
    public TaskController(ITasksService TasksService)
    {
        this.TasksService = TasksService;
    }
    
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
    [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var task = TasksService.GetById(id);
           
            if (task is null)
                return  NotFound();

            TasksService.Delete(id);

            var tt = TasksService.Count;
            return Content(TasksService.Count.ToString());
        }

    
}
