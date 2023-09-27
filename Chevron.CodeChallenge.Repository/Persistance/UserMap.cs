using AutoMapper.Configuration.Annotations;
using MongoDB.Bson.Serialization;
using Chevron.CodeChallenge.Models;

namespace Chevron.CodeChallenge.Persistance
{
    public class UserMap
    {
        public static void Configure()
        {
            BsonClassMap.RegisterClassMap<User>(map =>
            {
                map.AutoMap();
                map.SetIgnoreExtraElements(true);
                map.MapIdMember(x => x.Id);
            });
        }
    }
}
