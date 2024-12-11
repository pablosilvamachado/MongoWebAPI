using System.Net.Http.Headers;
using System.Text;
using WebAPI.Domain.Entities;
using WebAPI.Infra.Repository;

namespace WebAPI.Util;

public static class ValidatorUser
{
    
    public static async Task<bool> BasicAuthorization(MongoRepository<UserEntity> _mongoRepositoryUser,string authHeader){
        var credentials = AuthenticationHeaderValue.Parse(authHeader).Parameter;
        var encoding = Encoding.GetEncoding("iso-8859-1");
        credentials = encoding.GetString(Convert.FromBase64String(credentials));
        string?[] values = credentials.Split(":");
        string? name = values[0];
        string? password = values[1];
        bool res = false;
        
        
        try
        {
            var user = (await _mongoRepositoryUser.GetAllAsync("userName",name)).FirstOrDefault();
            if (user.password == password)
            {
                res = true;
            }
        }
        catch(Exception e)
        {
            Console.WriteLine(e);
        }

        return res;
    }
}