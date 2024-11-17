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
    public class ProjectsController : ControllerBase
    {
        private readonly TaskMgtDBContext _context;
        private readonly ILogger<ProjectsController> _logger;

        public ProjectsController(TaskMgtDBContext context, ILogger<ProjectsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/Projects
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectTb>>> GetProject()
        {
            _logger.LogInformation("Received a Projects Get request");
            return await _context.ProjectTb.ToListAsync();
        }

        // GET: api/Projects/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectTb>> GetProject(int id)
        {
            var project = await _context.ProjectTb.FindAsync(id);
            _logger.LogInformation($"Received a Projects Get request by id: {id}");
            if (project == null)
            {
                return NotFound();
            }

            return project;
        }

        // PUT: api/Projects/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProject(int id, ProjectDTO projectDTO)
        {
            ProjectTb projectTb= new ProjectTb();
            projectTb.ProjectId=projectDTO.ProjectId;
            projectTb.ProjectName=projectDTO.ProjectName;
            projectTb.StartDate=projectDTO.StartDate;
            projectTb.EndDate=projectDTO.EndDate;
            projectTb.Description=projectDTO.Description;

            if (id != projectTb.ProjectId)
            {
                return BadRequest();
            }

            _context.Entry(projectTb).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Updated a projects Put request by id: {id}");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectExists(id))
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
        public async Task<ActionResult<ProjectTb>> PostProject(ProjectDTO projectDTO)
        {
            ProjectTb projectTb = new ProjectTb();
            projectTb.ProjectId = projectDTO.ProjectId;
            projectTb.ProjectName = projectDTO.ProjectName;
            projectTb.StartDate = projectDTO.StartDate;
            projectTb.EndDate = projectDTO.EndDate;
            projectTb.Description = projectDTO.Description;

            _context.ProjectTb.Add(projectTb);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Updated a project Post request by id: {projectTb.ProjectId}");
            return CreatedAtAction("Getproject", new { id = projectTb.ProjectId }, projectTb);
        }

        // DELETE: api/projects/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            var project = await _context.ProjectTb.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }

            _context.ProjectTb.Remove(project);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Deleted a Projects data belongs to id: {id}");

            return NoContent();
        }

        private bool ProjectExists(int id)
        {
            return _context.ProjectTb.Any(e => e.ProjectId == id);
        }
    }
}