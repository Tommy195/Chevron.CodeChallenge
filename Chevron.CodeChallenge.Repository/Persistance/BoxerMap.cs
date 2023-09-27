using AutoMapper.Configuration.Annotations;
using MongoDB.Bson.Serialization;
using Chevron.CodeChallenge.Models;

namespace Chevron.CodeChallenge.Persistance
{
    public class BoxerMap
    {
        public static void Configure()
        {
            BsonClassMap.RegisterClassMap<Boxer>(map =>
            {
                map.AutoMap();
                map.SetIgnoreExtraElements(true);
                map.MapIdMember(x => x.Id);
                map.MapMember(x => x.Description).SetIsRequired(true);
            });
        }
    }
}
