using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConstructSN.Shared.InfrastructureModel
{

    public abstract class EntityBase
    {
        [BsonId]
        [BsonIgnoreIfNull]
        [BsonIgnoreIfDefault]
        public virtual Guid? _id
        {
            get;
            set;
        } = Guid.NewGuid();
    }

   
}
