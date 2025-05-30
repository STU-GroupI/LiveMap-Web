using Azure.Core;
using LiveMap.Application;
using LiveMap.Application.Images.Requests;
using LiveMap.Application.Images.Responses;
using LiveMap.ImageServer.Models.Image;
using Microsoft.AspNetCore.Mvc;

namespace LiveMap.ImageServer.Controllers
{
    [Route("apiuwu/[controller]")]
    [ApiController]
    public class ImageController : Controller
    {
        private readonly IWebHostEnvironment _env;

        public ImageController(IWebHostEnvironment env)
        {
            _env = env;
        }

        //[HttpPost("upload")]
        //public async Task<IActionResult> Upload(IFormFile file)
        //{
        //    if (file == null || file.Length == 0)
        //        return BadRequest("No file uploaded.");

        //    var uploadsFolder = Path.Combine(_env.WebRootPath, "images");

        //    if (!Directory.Exists(uploadsFolder))
        //        Directory.CreateDirectory(uploadsFolder);

        //    var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
        //    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

        //    using (var stream = new FileStream(filePath, FileMode.Create))
        //    {
        //        await file.CopyToAsync(stream);
        //    }

        //    var url = $"{Request.Scheme}://{Request.Host}/images/{uniqueFileName}";
        //    return Ok(new { imageUrl = url });
        //}

        [HttpPost("upload")]
        public async Task<IActionResult> Upload(
            [FromBody] CreateSingleImageWebRequest webRequest,
            [FromServices] IRequestHandler<CreateSingleRequest, CreateSingleResponse> handler)
        {
            if (webRequest.imageFile == null || webRequest.imageFile.Length == 0)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "No file uploaded.");
            }

            try
            {
                var uploadsFolder = Path.Combine(_env.WebRootPath, "images");

                // For first time use
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(webRequest.imageFile.FileName);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await webRequest.imageFile.CopyToAsync(stream);
                }

                var url = $"{Request.Scheme}://{Request.Host}/images/{uniqueFileName}";

                var request = new CreateSingleRequest(url);

                CreateSingleResponse response = await handler.Handle(request);
                return Created("", response.ImageUrl);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong...");
            }
        }
    }
}
