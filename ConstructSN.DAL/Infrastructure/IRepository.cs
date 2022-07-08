using ConstructSN.Shared.InfrastructureModel;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ConstructSN.DAL.Infrastructure
{
    public interface IRepository<T> where T : EntityBase
    {
        Task<T> Delete(Guid id);
        Task<ReplaceOneResult> Upsert(T item);
        Task<IEnumerable<T>> Get(int pageIndex, int pageSize, FilterDefinition<T> filterDefinition);
        Task<long> CountDocuments(FilterDefinition<T> filterDefinition);
        FilterDefinition<T> Filter(FilterDefinition<T> filterDefinition);
    }

    public abstract class ARepositoryMRC<T> : IRepository<T> where T : EntityBase
    {
        string nameCollection;

        private readonly IMongoDatabase database;

        public ARepositoryMRC(string connectionString, string databaseName, string nameCollection)
        {
            var client = new MongoClient(connectionString);

            this.database = client.GetDatabase(databaseName);

            this.nameCollection = nameCollection;
        }

        public IMongoCollection<T> collectionInstance
        {
            get
            {
                return database.GetCollection<T>(this.nameCollection);
            }
        }

        public async Task<long> CountDocuments(FilterDefinition<T> filterDefinition)
        {
            try
            {
                return await collectionInstance.CountDocumentsAsync(Filter(filterDefinition));
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IEnumerable<T>> Get(int pageIndex, int pageSize, FilterDefinition<T> filterDefinition)
        {
            try
            {
                return await collectionInstance.Find(Filter(filterDefinition))
                    .Skip(pageIndex)
                    .Limit(pageSize).ToListAsync();

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public FilterDefinition<T> Filter(FilterDefinition<T> filterDefinition)
        {
            var builder = Builders<T>.Filter;

            var filter = builder.And(filterDefinition);

            return filter;
        }

        public async Task<ReplaceOneResult> Upsert(T item)
        {
            try
            {
                return await collectionInstance.ReplaceOneAsync(n => n._id.Equals(item._id)
                                            , item
                                            , new ReplaceOptions { IsUpsert = true });
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<T> Delete(Guid id)
        {
            try
            {
                var update = new BsonDocument("$set", new BsonDocument("bAnulado", true));
                return await collectionInstance.FindOneAndUpdateAsync(_ => _._id == id, update);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }

}
