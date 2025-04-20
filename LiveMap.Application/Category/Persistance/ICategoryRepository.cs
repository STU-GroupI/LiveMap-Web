using Models = LiveMap.Domain.Models;

public interface ICategoryRepository
{
    public Task<Models.Category> Create(Models.Category category);
    public Task<Models.Category?> GetSingle(string name);

    public Task<ICollection<Models.Category>> GetMultiple(string name, int? skip, int? take);
    public Task<bool> Update(string oldName, string newName);
    public Task<bool> Delete(string name);
}
