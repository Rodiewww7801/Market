using Market.Data.Entities.ProductAggregate;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Market.Mongo
{
    public class RegisterClassMap
    {
        public static void Register()
        {
            BsonClassMap.RegisterClassMap<Product>(cm =>
            {
                //cm.AutoMap();
                //cm.MapIdMember(x => x.Id)
                //.SetSerializer(new StringSerializer(BsonType.ObjectId));
            });
        }
    }
}
