using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using practice_ef.Data;
using practice_ef.Models;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public UsersController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/Users
    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetUsers()
    {
        return await _context.Users.ToListAsync();
    }

    // GET: api/Users/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUser(int id)
    {
        User? user = await _context.Users.FindAsync(id);

        if (user == null)
            return NotFound();

        return user;
    }

    // POST: api/Users
    [HttpPost]
    public async Task<ActionResult<User>> CreateUser(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return Created();
    }

    // PUT: api/Users/{id}
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateUser(int id, User updatedUser)
    {

        User? userEntity = await _context.Users.FindAsync(id);

        if (userEntity == null)
            return NotFound();

        userEntity.FirstName = updatedUser.FirstName;
        userEntity.LastName = updatedUser.LastName;
        userEntity.Email = updatedUser.Email;

        _context.Entry(userEntity).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (Exception)
        {
            return BadRequest();
        }

        return NoContent();
    }

    // DELETE: api/Users/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        User? user = await _context.Users.FindAsync(id);

        if (user == null)
            return NotFound();

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
