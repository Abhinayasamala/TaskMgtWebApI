using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskMgtWebAPI.Models;
using TaskMgtWebApI.DTO;

namespace TaskMgtWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly TaskMgtDBContext _context;
        private readonly ILogger<TasksController> _logger;

        public TasksController(TaskMgtDBContext context, ILogger<TasksController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/Projects
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskTb>>> GetProject()
        {
            _logger.LogInformation("Received a Tasks Get request");
            return await _context.TaskTb.ToListAsync();
        }

        // GET: api/Projects/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TaskTb>> GetTask(int id)
        {
            var Task = await _context.TaskTb.FindAsync(id);
            _logger.LogInformation($"Received a Tasks Get request by id: {id}");
            if (Task == null)
            {
                return NotFound();
            }

            return Task;
        }

        // PUT: api/Projects/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTask(int id, TaskDTO taskDTO)
        {
            TaskTb taskTb=new TaskTb();
            taskTb.TaskId=taskDTO.TaskId;
            taskTb.Title=taskDTO.Title; 
            taskTb.Description=taskDTO.Description;
            taskTb.Status=taskDTO.Status;
            taskTb.DueDate=taskDTO.DueDate;
            taskTb.Priority=taskDTO.Priority;
            taskTb.UserId=taskDTO.UserId;
            taskTb.ProjectId=taskDTO.ProjectId; 

            if (id != taskTb.ProjectId)
            {
                return BadRequest();
            }

            _context.Entry(taskTb).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Updated a Tasks Put request by id: {id}");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Projects
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TaskTb>> PostTask(TaskDTO taskDTO)
        {
            TaskTb taskTb = new TaskTb();
            taskTb.TaskId = taskDTO.TaskId;
            taskTb.Title = taskDTO.Title;
            taskTb.Description = taskDTO.Description;
            taskTb.Status = taskDTO.Status;
            taskTb.DueDate = taskDTO.DueDate;
            taskTb.Priority = taskDTO.Priority;
            taskTb.UserId = taskDTO.UserId;
            taskTb.ProjectId = taskDTO.ProjectId;

            _context.TaskTb.Add(taskTb);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Updated a task Post request by id: {taskTb.TaskId}");
            return CreatedAtAction("Getproject", new { id = taskTb.TaskId }, taskTb);
        }

        // DELETE: api/projects/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var task = await _context.TaskTb.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }

            _context.TaskTb.Remove(task);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Deleted a Tasks data belongs to id: {id}");

            return NoContent();
        }

        private bool TaskExists(int id)
        {
            return _context.TaskTb.Any(e => e.TaskId == id);
        }
    }
}
