using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShowNTell.Prototype.Models;
using System.IO;
using System.Threading.Tasks;

namespace ShowNTell.Prototype.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<PostsController> _logger;

        public PostsController(IWebHostEnvironment environment, ILogger<PostsController> logger)
        {
            _environment = environment;
            _logger = logger;
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult GetPost(int id)
        {
            string baseUrl = HttpContext.Request.Scheme + "://" + HttpContext.Request.Host;

            //Simulate getting a post by ID from the database...
            return Ok(new ShowNTellFile
            {
                Id = id,
                Name = "ProfilePicture",
                Url = baseUrl + "/uploads/profile.png"
            });
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
            //Note: Directory is hardcoded. Ideally, we will pass this as a constant parameter to the controller.
            string targetFilePath = Path.Combine(_environment.WebRootPath, "uploads", "profile.png");

            //Write the file to the path.
            using (FileStream output = new FileStream(targetFilePath, FileMode.Create))
            {
                await uploadedFile.CopyToAsync(output);
            }

            //Simluate adding the file data to the database...

            return Ok();
        }
    }
}