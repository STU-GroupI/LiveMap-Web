namespace LiveMapDashboard.Web.Extensions.Mappers;

public static class ImageHelpers
{
    public static string ToImage(this IFormFile imageFile)
    {
        using var memoryStream = new MemoryStream();
        imageFile.CopyTo(memoryStream);
        byte[] imageBytes = memoryStream.ToArray();

        // Get content type (e.g., image/png, image/jpeg)
        string contentType = imageFile.ContentType;

        // Return data URI
        return $"data:{contentType};base64,{Convert.ToBase64String(imageBytes)}";
    }
}
