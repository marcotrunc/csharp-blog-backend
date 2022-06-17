using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using csharp_blog_backend.Models;
using System.Web;


namespace csharp_blog_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly BlogContext _context;

        public PostsController(BlogContext context)
        {
            _context = context;
        }

        // GET: api/Posts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Post>>> GetPosts()
        {
          if (_context.Posts == null)
             return NotFound();

            return await _context.Posts.ToListAsync();
        }

        // GET: api/Posts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Post>> GetPost(int id)
        {
          if (_context.Posts == null)
              return NotFound();

            var post = await _context.Posts.FindAsync(id);

            if (post == null)
                return NotFound();

            return post;
        }

        // PUT: api/Posts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPost(int id, Post post)
        {
            if (id != post.Id)
                return BadRequest();

            _context.Entry(post).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PostExists(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // POST: api/Posts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Post>> PostPost([FromForm] Post post)
        {
            FileInfo fileInfo = new FileInfo(post.File.FileName);
            post.Image = $"FileLocal.{fileInfo.Extension}";
            
            _context.Posts.Add(post);
            await _context.SaveChangesAsync();
            //Estrazione File e salvataggio su file system.
            //Agendo su Request ci prendiamo il file e lo salviamo su file system.
            string Image = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files");

            if (!Directory.Exists(Image))
                Directory.CreateDirectory(Image);


            string fileName = $"immagine-{post.Id}" + fileInfo.Extension;

            string fileNameWithPath = Path.Combine(Image, fileName);

            using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
            {
                post.File.CopyTo(stream);
            }

            if (_context.Posts == null)
                return Problem("Entity set 'BlogContext.Posts'  is null.");

            return CreatedAtAction("GetPost", new { id = post.Id }, post);
        }

        // DELETE: api/Posts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(int id)
        {
            if (_context.Posts == null)
                return NotFound();
            
            var post = await _context.Posts.FindAsync(id);
            if (post == null)
                return NotFound();

            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PostExists(int id)
        {
            return (_context.Posts?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
