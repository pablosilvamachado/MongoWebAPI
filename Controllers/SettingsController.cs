using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using WebAPI.Domain.Entities;
using WebAPI.Infra.Repository;
using WebAPI.Util;
using MongoDB.Driver;

namespace WebAPI.Controllers;
using MongoDatabaseSettings = WebAPI.Infra.MongoDatabaseSettings;

[ApiController] [Route("/settings")]
public class SettingsController : ControllerBase
{
    private readonly MongoRepository<SettingsEntity> _mongoRepositorySettings;
    private readonly MongoRepository<UserEntity> _mongoRepositoryUser;
    
    private readonly IMemoryCache _cache;

    public SettingsController(IOptions<MongoDatabaseSettings> mongoDatabaseSettings, IMemoryCache cache)
    {
        _cache = cache;
        var client = new MongoClient(mongoDatabaseSettings.Value.ConnectionString);
        var database = client.GetDatabase(mongoDatabaseSettings.Value.DatabaseName);
        var dbSettings = database.GetCollection<SettingsEntity>(mongoDatabaseSettings.Value.SettingsCollectionName);
        _mongoRepositorySettings = new MongoRepository<SettingsEntity>(dbSettings);
        
        var dbUser = database.GetCollection<UserEntity>(mongoDatabaseSettings.Value.UserCollectionName);
        _mongoRepositoryUser = new MongoRepository<UserEntity>(dbUser);
        
        try
        {
            var result = _mongoRepositorySettings.GetInit();
            var isEnumerableEmpty = (int)(result.GetType()
                .GetMethod("get_Count", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance)
                ?.Invoke(result, null) ?? "nullable") == 0;
        
            if (isEnumerableEmpty)
            {
                SettingsEntity settingsEntity = new();
                _mongoRepositorySettings.InitSetting(settingsEntity);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    
    [HttpPatch]
    public async Task<IActionResult> PatchSettings(JsonPatchDocument<SettingsEntity> settings)
    {
        var request = HttpContext.Request.Headers["Authorization"];
        try
        {
            var isAuth = await ValidatorUser.BasicAuthorization(_mongoRepositoryUser,request);
            if (!isAuth) return new UnauthorizedObjectResult(new{message="Voce não tem autorização para realizar essa solicitação!"});
            
            var oldSettings = (await _mongoRepositorySettings.GetAllAsync()).FirstOrDefault();
            settings.ApplyTo(oldSettings,ModelState);
            var result = await _mongoRepositorySettings.ReplaceOneAsync(oldSettings?._id.ToString(), oldSettings);
            return new OkObjectResult(new{result});
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new BadRequestResult();
        }
    }


    [HttpGet][ResponseCache(Duration = 300, Location = ResponseCacheLocation.Any)]
    public async Task<IActionResult> GetSettings()
    {
        try
        {
            if (_cache.TryGetValue("SettingsKey", out var settings)) return new OkObjectResult(settings);
            
            settings = (await _mongoRepositorySettings.GetAllAsync()).FirstOrDefault();

            var cacheEntryOptions = new MemoryCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5) };
            _cache.Set("SettingsKey", settings, cacheEntryOptions);

            return new OkObjectResult(settings);
        }
        catch (Exception e)
        {
            return new EmptyResult();
        }
    }
}