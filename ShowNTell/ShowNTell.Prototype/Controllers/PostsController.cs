using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShowNTell.Prototype.Models;
using System;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ShowNTell.Prototype.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private const string BASE_IMAGE_PATH = "uploads";

        private readonly IWebHostEnvironment _environment;
        private readonly ShowNTellDbContext _context;

        public PostsController(IWebHostEnvironment environment, ShowNTellDbContext context)
        {
            _environment = environment;
            _context = context;

            //Create upload directory
            string uploadsPath = Path.Combine(_environment.WebRootPath, BASE_IMAGE_PATH);
            if(!Directory.Exists(uploadsPath))
            {
                Directory.CreateDirectory(uploadsPath);
            }
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetPost(int id)
        {
            string baseUrl = HttpContext.Request.Scheme + "://" + HttpContext.Request.Host;

            // Get the post from the database.
            ImagePost post = await _context.ImagePosts.FindAsync(id);

            // Setup the uri location to show the image.
            post.ImageUri = Path.Combine(baseUrl, post.ImageUri);

            return Ok(post);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> CreatePost()
        {
            IFormFileCollection files = HttpContext.Request.Form.Files;

            //Must upload only 1 file.
            if (files == null || files.Count != 1)
            {
                return BadRequest();
            }

            //Get the uploaded file.
            IFormFile uploadedFile = files[0];

            //Create the path for the uploaded file.
            string fileExtension = Path.GetExtension(uploadedFile.FileName);
            string relativeImageUri = Path.Combine(BASE_IMAGE_PATH, Guid.NewGuid().ToString() + fileExtension);
            string targetFilePath = Path.Combine(_environment.WebRootPath, relativeImageUri);

            //Write the file to the path.
            using (FileStream output = new FileStream(targetFilePath, FileMode.Create))
            {
                await uploadedFile.CopyToAsync(output);
            }

            //Create new post.
            ImagePost newPost = new ImagePost
            {
                UserEmail = HttpContext.User.FindFirst(ClaimTypes.Email).Value,
                Description = "This is a post",
                ImageUri = relativeImageUri
            };

            //Add post to database.
            _context.ImagePosts.Add(newPost);
            await _context.SaveChangesAsync();

            return Ok(newPost);
        }
    }
}