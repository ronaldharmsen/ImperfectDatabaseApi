using ImperfectDatabaseApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ImperfectDatabaseApi.Controllers
{
    [Route("api/posts")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly ImperfectDataContext context;

        public PostsController(ImperfectDataContext context)
            => this.context = context;
        
        [HttpGet]
        public async Task<IActionResult> Get()
            => Ok(await context.Posts.OrderBy(p => p.Created).ToListAsync());

        [HttpGet("id")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var post = await context.Posts.FirstOrDefaultAsync(p=>p.Id == id);
            if (post == null)
                return NotFound();

            return Ok(post);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody]PostDto postDto)
        {
            //todo: replace with proper logic/current user
            var firstUser = await context.Users.FirstAsync();
            var post = new Post
            {
                Author = firstUser.ToAuthor(),
                Created = DateTime.UtcNow,
                Text = postDto.Text
            };
            await context.Posts.AddAsync(post);
            await context.SaveChangesAsync();
            return Created($"api/post/{post.Id}", post);
        }
    }

    public class PostDto
    {
        public string Text { get; set; }
    }
}
