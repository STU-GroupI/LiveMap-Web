namespace LiveMap.ImageServer.Models.Image;

public sealed record CreateSingleImageWebRequest(
    IFormFile imageFile
    );
