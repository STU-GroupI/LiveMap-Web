using Img = LiveMap.Domain.Models.Image;

namespace LiveMap.ImageServer.Models.Image;

public sealed record CreateSingleImageWebRequest(
    Img Image
    );
