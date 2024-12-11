using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace WebAPI.Domain.Entities;

public record MongoEntity
{
    [System.Text.Json.Serialization.JsonIgnore, JsonIgnore] public ObjectId _id { get; set; }
}