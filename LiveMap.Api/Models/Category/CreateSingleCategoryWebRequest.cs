namespace LiveMap.Api.Models.Category;

public record CreateSingleCategoryWebRequest(
    string CategoryName,
    string IconName
);