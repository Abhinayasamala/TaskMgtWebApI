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
    public class UsersController : ControllerBase
    {
        private readonly TaskMgtDBContext _context;
        private readonly ILogger<UsersController> _logger;

        public UsersController(TaskMgtDBContext context, ILogger<UsersController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserTb>>> GetUser()
        {
            _logger.LogInformation("Received a Users Get request");
            return await _context.UserTb.ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserTb>> GetUsers(int id)
        {
            var User = await _context.UserTb.FindAsync(id);
            _logger.LogInformation($"Received a Users Get request by id: {id}");
            if (User == null)
            {
                return NotFound();
            }

            return User;
        }

        // PUT: api/Projects/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, UserDTO userDTO)
        {
            UserTb userTb=new UserTb();
            userTb.UserId=userDTO.UserId;
            userTb.FirstName=userDTO.FirstName; 
            userTb.LastName=userDTO.LastName;
            userTb.Email=userDTO.Email;
            userTb.Password=userDTO.Password;
            userTb.RoleId=userDTO.RoleId;   

            if (id != userTb.UserId)
            {
                return BadRequest();
            }

            _context.Entry(userTb).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Updated a Users Put request by id: {id}");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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
        public async Task<ActionResult<UserTb>> PostUser(UserDTO userDTO)
        {
            UserTb userTb = new UserTb();
            userTb.UserId = userDTO.UserId;
            userTb.FirstName = userDTO.FirstName;
            userTb.LastName = userDTO.LastName;
            userTb.Email = userDTO.Email;
            userTb.Password = userDTO.Password;
            userTb.RoleId = userDTO.RoleId;

            _context.UserTb.Add(userTb);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Updated a User Post request by id: {userTb.UserId}");
            return CreatedAtAction("GetUser", new { id = userTb.UserId }, userTb);
        }

        // DELETE: api/projects/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.UserTb.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.UserTb.Remove(user);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Deleted a Users data belongs to id: {id}");

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return _context.UserTb.Any(e => e.UserId == id);
        }
    }
}