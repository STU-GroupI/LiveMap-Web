using Azure.Core;
using LiveMap.Application;
using LiveMap.Application.Images.Requests;
using LiveMap.Application.Images.Responses;
using LiveMap.ImageServer.Models.Image;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace LiveMap.ImageServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ImageController : Controller
    {
        private readonly IWebHostEnvironment _env;

        public ImageController(IWebHostEnvironment env)
        {
            _env = env;
        }

        [HttpPost("")]
        public async Task<IActionResult> Upload(
            [FromBody] CreateSingleImageWebRequest webRequest,
            [FromServices] IRequestHandler<CreateSingleRequest, CreateSingleResponse> handler)
        {
            if (string.IsNullOrWhiteSpace(webRequest.Image.Base64Image))
            {
                return BadRequest("No image data provided.");
            }

            try
            {
                var uploadsFolder = Path.Combine(_env.WebRootPath, "images");

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // Extract the base64 part (in case it's a data URL)
                var base64Data = GetBase64FromDataUri(webRequest.Image.Base64Image);
                var imageBytes = Convert.FromBase64String(base64Data);

                var uniqueFileName = Guid.NewGuid().ToString() + ".png"; // Or infer format from data URI
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                await System.IO.File.WriteAllBytesAsync(filePath, imageBytes);

                var url = $"{Request.Scheme}://{Request.Host}/images/{uniqueFileName}";
                var request = new CreateSingleRequest(url);
                CreateSingleResponse response = await handler.Handle(request);

                return Created("", response);
            }
            catch (FormatException)
            {
                return BadRequest("Invalid Base64 string.");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong...");
            }
        }

        private static string GetBase64FromDataUri(string dataUri)
        {
            var commaIndex = dataUri.IndexOf(",");
            return commaIndex >= 0 ? dataUri.Substring(commaIndex + 1) : dataUri;
        }
    }
}
