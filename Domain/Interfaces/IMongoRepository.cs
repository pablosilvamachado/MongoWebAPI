using MongoDB.Driver;

namespace WebAPI.Domain.Interfaces;

public interface IMongoRepository<T>
{
    Task<IEnumerable<T>> GetAllAsync(string? filterObj, string? filterValue);
    Task InsertAsync(T document);
    Task<UpdateResult> UpdateOneAsync(string id, T newValue);

    Task<ReplaceOneResult> ReplaceOneAsync(string? id, T document);
}