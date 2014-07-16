using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;
using System;
using System.Linq;
using System.Linq.Expressions;
using ThinkUp.Sdk.Data.Configuration;
using ThinkUp.Sdk.Data.Entities;

namespace ThinkUp.Sdk.Data
{
    public class Repository<T> : IRepository<T>
        where T : DataEntity
    {
        private static readonly string idName = "_id";
        private static readonly string collectionName = typeof(T).Name;

        private MongoDatabase database;

        ///<exception cref="DataException">DataException</exception>
        public Repository(IDataSection configuration)
        {
            this.InitializeDatabase(configuration);
        }

        private void InitializeDatabase(IDataSection configuration)
        {
            try
            {
                var databaseClient = new MongoClient(configuration.ConnectionString);
                var databaseServer = databaseClient.GetServer();

                this.database = databaseServer.GetDatabase(configuration.DatabaseName);
            }
            catch (MongoException mongoEx)
            {
                var errorMessage = string.Concat("An unexpected error occured when initializing DB connection. Details: {0}", mongoEx.Message);

                throw new DataException(errorMessage, mongoEx);
            }
        }

        public IQueryable<T> GetAll(Expression<Func<T, bool>> predicate = null)
        {
            var collection = this.database.GetCollection<T>(collectionName).AsQueryable();

            if (predicate != null)
            {
                collection = collection.Where(predicate);
            }

            return collection;
        }

        public T Get(Expression<Func<T, bool>> predicate = null)
        {
            var filteredCollection = this.GetAll(predicate);

            return filteredCollection.SingleOrDefault();
        }

        public bool Exist(Expression<Func<T, bool>> predicate = null)
        {
            var existingDataObject = this.Get(predicate);

            return existingDataObject != default(T);
        }

        ///<exception cref="DataException">DataException</exception>
        public void Create(T dataEntity)
        {
            var collection = this.database.GetCollection<T>(collectionName);
            var insertResult = collection.Insert(dataEntity);

            if (!insertResult.Ok)
            {
                var errorMessage = string.Concat("Creation of document {0} failed", collectionName);

                throw new DataException(errorMessage);
            }
        }

        ///<exception cref="DataException">DataException</exception>
        public void Update(T dataEntity)
        {
            var collection = this.database.GetCollection<T>(collectionName);
            var saveResult = collection.Save(dataEntity);

            if (!saveResult.Ok)
            {
                var errorMessage = string.Concat("Update of document {0} with Id {1} failed", collectionName, dataEntity.Id);

                throw new DataException(errorMessage);
            }
        }

        ///<exception cref="DataException">DataException</exception>
        public void Delete(T dataEntity)
        {
            var id = dataEntity.Id;
            var collection = this.database.GetCollection<T>(collectionName);
            var removeQuery = Query.EQ(idName, id);
            var deleteResult = collection.Remove(removeQuery);

            if (!deleteResult.Ok)
            {
                var errorMessage = string.Concat("Deletion of document {0} with Id {1} failed", collectionName, id);

                throw new DataException(errorMessage);
            }
        }

        ///<exception cref="DataException">DataException</exception>
        public void DeleteAll()
        {
            var collection = this.database.GetCollection<T>(collectionName);
            var deleteResult = collection.RemoveAll();

            if (!deleteResult.Ok)
            {
                var errorMessage = string.Concat("Deletion of the hole collection {0} failed", collectionName);

                throw new DataException(errorMessage);
            }
        }
    }
}
