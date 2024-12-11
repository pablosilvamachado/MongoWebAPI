using WebAPI.Domain.Interfaces;
using MongoDB.Driver;
using MongoDB.Bson;

namespace WebAPI.Infra.Repository;

public class MongoRepository<T> : IMongoRepository<T>
{
    private readonly IMongoCollection<T> _collection;

    public MongoRepository(IMongoCollection<T> collection)
    {
        _collection = collection;
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="document"></param>
    public void InitSetting(T document)
    {
        _collection.InsertOne(document);
    }

    public IEnumerable<T> GetInit()
    {
        return _collection.Find(_ => true).ToList();
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<T>> GetAllAsync(string? filterObj = null, string? filterValue = null)
    {
        if (filterObj != null && filterValue != null)
        {
            var filter = Builders<T>.Filter.Eq(filterObj, filterValue);
            return await _collection.Find(filter).ToListAsync();
        }
        return await _collection.Find(_=> true).ToListAsync();
    }   
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="document"></param>
    public async Task InsertAsync(T document)
    {
        await _collection.InsertOneAsync(document);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="newValue"></param>
    public async Task<UpdateResult> UpdateOneAsync(string id,T newValue)
    {
        var filter = Builders<T>.Filter.Eq("_id", new ObjectId(id));
        var update = Builders<T>.Update.Set(arg => arg, newValue);
        return await _collection.UpdateOneAsync(filter, update);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="document"></param>
    public async Task<ReplaceOneResult> ReplaceOneAsync(string? id, T document)
    
    {
        var filter = Builders<T>.Filter.Eq("_id", new ObjectId(id));
        return await _collection.ReplaceOneAsync(filter, document);
    }
}