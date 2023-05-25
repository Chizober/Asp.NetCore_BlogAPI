using Blog.API.Data;
using Blog.API.Models.DTO;
using Blog.API.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly BlogDbContext dbContext;

        public PostController(BlogDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
    [HttpGet]
        public async Task<IActionResult> GetAllPosts()
        {
            var posts = await dbContext.Posts.ToListAsync();
            return Ok(posts);
        }
    [HttpGet]
    [Route("{id:guid}")]
    [ActionName("GetPostById")]
     public async Task<IActionResult> GetPostById(Guid id)
    {
      var post = await dbContext.Posts.FirstOrDefaultAsync(x => x.Id == id);
      if (post != null)
      {
        return Ok(post);
      }
      return NotFound();

    }

    [HttpPost]
     public async Task<IActionResult> AddPost(AddPostRequest addPostRequest)
    {
      var post = new Post()
      {
        Title = addPostRequest.Title,
        Content = addPostRequest.Content,
        Author = addPostRequest.Author,
        FeaturedImageUrl = addPostRequest.FeaturedImageUrl,
        PublishedDate = addPostRequest.PublishedDate,
        UpdatedDate = addPostRequest.UpdatedDate,
        Summary = addPostRequest.Summary,
        UrlHandle = addPostRequest.UrlHandle,
        Visible = addPostRequest.Visible
  
      };
      post.Id = Guid.NewGuid();
      await dbContext.Posts.AddAsync(post);
      await dbContext.SaveChangesAsync();
      return CreatedAtAction(nameof(GetPostById), new {id = post.Id}, post);
    }
    [HttpPut]
    [Route("{id:guid}")]
    public async Task<IActionResult> UpdatePost([FromRoute]Guid id,UpdatePostRequest updatePostRequest)
    {
     
      var existingPost = await dbContext.Posts.FindAsync(id);
      if(existingPost != null)
      {
        existingPost.Title = updatePostRequest.Title;
        existingPost.Content = updatePostRequest.Content;
        existingPost.Author = updatePostRequest.Author;
        existingPost.FeaturedImageUrl = updatePostRequest.FeaturedImageUrl;
        existingPost.PublishedDate = updatePostRequest.PublishedDate;
        existingPost.UpdatedDate = updatePostRequest.UpdatedDate;
        existingPost.Summary = updatePostRequest.Summary;
        existingPost.UrlHandle = updatePostRequest.UrlHandle;
        existingPost.Visible = updatePostRequest.Visible;
         await dbContext.SaveChangesAsync();
        return Ok(existingPost);

      }
      return NotFound();

    }

    [HttpDelete]
    [Route("{id:guid}")]
    public async Task<IActionResult> DeletePost(Guid id)
    {
      var existingPost = await dbContext.Posts.FindAsync(id);
      if (existingPost != null)
      {
        dbContext.Remove(existingPost);
        await dbContext.SaveChangesAsync();
        return Ok(existingPost);

      }
      return NotFound();


    }

  }

}
