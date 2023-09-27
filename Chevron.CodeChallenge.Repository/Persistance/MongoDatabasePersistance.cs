using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;

namespace Chevron.CodeChallenge.Persistance
{
    public class MongoDatabasePersistance
    {
        public static void Configure()
        {
            BoxerMap.Configure();
            UserMap.Configure();

            BsonDefaults.GuidRepresentation = GuidRepresentation.CSharpLegacy;

            var pack = new ConventionPack
            {
                new IgnoreExtraElementsConvention(true),
                new IgnoreIfDefaultConvention(true)
            };

            ConventionRegistry.Register("My Solution Conventions", pack, t => true);
        }
    }
}
