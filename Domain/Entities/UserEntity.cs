namespace WebAPI.Domain.Entities;

public record UserEntity: MongoEntity
{
    public string userName { get; set; }
    public string password { get; set; }
}