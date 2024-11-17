using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskMgtWebAPI.Models;
using TaskMgtWebApI.DTO;
using System.Data;

namespace TaskMgtWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly TaskMgtDBContext _context;
        private readonly ILogger<RolesController> _logger;

        public RolesController(TaskMgtDBContext context, ILogger<RolesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/Roles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoleTb>>> GetRole()
        {
            _logger.LogInformation("Received a Roles Get request");
            return await _context.RoleTb.ToListAsync();
        }

        // GET: api/Roles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RoleTb>> GetRole(int id)
        {
            var role = await _context.RoleTb.FindAsync(id);
            _logger.LogInformation($"Received a Roles Get request by id: {id}");
            if (role == null)
            {
                return NotFound();
            }

            return role;
        }

        // PUT: api/Roles/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRole(int id, RoleDTO roleDTO)
        {
            RoleTb roleTb=new RoleTb();
            roleTb.RoleId=roleDTO.RoleId;
            roleTb.RoleName=roleDTO.RoleName;

            if (id != roleTb.RoleId)
            {
                return BadRequest();
            }

            _context.Entry(roleTb).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Updated a Roles Put request by id: {id}");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RoleExists(id))
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

        // POST: api/Roles
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<RoleTb>> PostRole(RoleDTO roleDTO)
        {
            RoleTb roleTb = new RoleTb();
            roleTb.RoleId = roleDTO.RoleId;
            roleTb.RoleName = roleDTO.RoleName;

            _context.RoleTb.Add(roleTb);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Updated a Admin Post request by id: {roleTb.RoleId}");
            return CreatedAtAction("GetRole", new { id = roleTb.RoleId }, roleTb);
        }

        // DELETE: api/Roles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            var role = await _context.RoleTb.FindAsync(id);
            if (role == null)
            {
                return NotFound();
            }

            _context.RoleTb.Remove(role);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Deleted a roles data belongs to id: {id}");

            return NoContent();
        }

        private bool RoleExists(int id)
        {
            return _context.RoleTb.Any(e => e.RoleId == id);
        }
    }
}
