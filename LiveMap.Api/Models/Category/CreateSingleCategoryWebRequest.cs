using LiveMap.Domain.Models;

namespace LiveMap.Api.Models.Category;

public record CreateSingleCategoryWebRequest(
    string CategoryName
);