using ImperfectDatabaseApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ImperfectDatabaseApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ImperfectDataContext context;

        public UsersController(ImperfectDataContext context)
            => this.context = context;

        [HttpGet]
        public async Task<IActionResult> Get()
            => Ok(await context.Users.OrderBy(p => p.Name).ToListAsync());


        [HttpGet("id")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var post = await context.Users.FirstOrDefaultAsync(p => p.Id == id);
            if (post == null)
                return NotFound();

            return Ok(post);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UserDto dto)
        {
            var existing = await context.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (existing == null)
                return NotFound();

            existing.Profile.FirstName = dto.FirstName;
            existing.Profile.LastName = dto.LastName;

            // update author on all related posts,
            // todo: find a better way at scale ;-)
            var allPosts = await context.Posts.Where(p=>p.Author.UserId == id).ToListAsync();
            allPosts.ForEach(p => p.Author = existing.ToAuthor());

            await context.SaveChangesAsync();
            return Ok(existing);
        }
    }

    public class UserDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
