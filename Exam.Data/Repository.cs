using Exam.Data.Db;
using Exam.Utility.Exceptions;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam.Data
{

    public interface IRepository<out T>
    {
        MongoClient GetClient();
        IMongoDatabase GetDb();


        T Find(string id, string collectionName = null);

        T Find(ObjectId id, string collectionName = null);
    }

    public abstract class BaseRepository
    {
        public static IRepository<IEntity> CreateRepository(Type type)
        {
            var repositoryType = typeof(Repository<>).MakeGenericType(type);

            var r = Activator.CreateInstance(repositoryType);
            return r as IRepository<IEntity>;
        }

        public static IRepository<IEntity> CreateRepository(string typeFullName)
        {
            var type = Type.GetType(typeFullName);
            return CreateRepository(type);
        }


    }

    public class Repository<T> : BaseRepository, IRepository<T> where T : IEntity
    {

        public virtual MongoClient GetClient()
        {
            return Db.DbManager.Current.GetClient();
        }

        public virtual IMongoDatabase GetDb()
        {
            return Db.DbManager.Current.GetGlobalDb();
        }

        //public virtual IMongoDatabase GetAccountDb(Account account)
        //{
        //    return Db.DbManager.Current.GetAccountDb(account);
        //}

        protected virtual DbAttribute GetDbAttribute()
        {
            var dbAttribute = typeof(T).GetCustomAttributes(typeof(DbAttribute), true).FirstOrDefault() as DbAttribute;
            if (dbAttribute == null)
                throw new NotSupportedException($"{typeof(T).FullName} sınıfı için tanımlı DbAttribute bulunamadı.");

            return dbAttribute;
        }

        public IMongoCollection<T> GetCollection(string collectionName = null)
        {
            var dbAttr = GetDbAttribute();
            if (string.IsNullOrWhiteSpace(collectionName))
                collectionName = dbAttr.CollectionName;

            return GetDb().GetCollection<T>(collectionName);
        }

        public IQueryable<T> AsQueryable(string collectionName = null)
        {
            return GetCollection(collectionName).AsQueryable();
        }
        public T Find(ObjectId id, string collectionName = null)
        {
            return GetCollection(collectionName).AsQueryable().Where(x => x.Id == id).FirstOrDefault();
        }

        public T Find(string id, string collectionName = null)
        {
            var oid = ObjectId.Parse(id);
            return GetCollection(collectionName).AsQueryable().Where(x => x.Id == oid).FirstOrDefault();
        }

        /// <summary>
        /// Dokümanın sadece belirli alanlarını update eder
        /// </summary>
        /// <param name="id">Güncelllenecek kayıtId numarası</param>
        /// <param name="data">Güncellenecek alanlar ve değerleri</param>
        /// <param name="collectionName"></param>
        internal virtual void UpdatePartial(ObjectId id, object data, string collectionName = null)
        {
            var dbAttr = GetDbAttribute();
            if (string.IsNullOrWhiteSpace(collectionName))
                collectionName = dbAttr.CollectionName;

            var collection = GetDb().GetCollection<BsonDocument>(collectionName);

            var changesDocument = data.ToBsonDocument();

            var filter = Builders<BsonDocument>.Filter.Eq("_id", id);

            UpdateDefinition<BsonDocument> update = null;
            foreach (var change in changesDocument)
            {
                if (update == null)
                {
                    var builder = Builders<BsonDocument>.Update;
                    update = builder.Set(change.Name, change.Value);
                }
                else
                {
                    update = update.Set(change.Name, change.Value);
                }
            }

            //following 3 lines are for debugging purposes only
            //var registry = BsonSerializer.SerializerRegistry;
            //var serializer = registry.GetSerializer<BsonDocument>();
            //var rendered = update.Render(serializer, registry).ToJson();

            //you can also use the simpler form below if you're OK with bypassing the UpdateDefinitionBuilder (and trust the JSON string to be fully correct)
            update = new BsonDocumentUpdateDefinition<BsonDocument>(new BsonDocument("$set", changesDocument));

            var result = collection.UpdateOne(filter, update);

            //GetCollection(collectionName).UpdateOne(x => x.Id == id, new UpdateDefinition() {  })
        }

        internal virtual T Save(T obj, IClientSessionHandle session = null, string collectionName = null)
        {
            var replaceOptions = new ReplaceOptions() { IsUpsert = true };
            try
            {
                if (session == null)
                {
                    GetCollection(collectionName).ReplaceOne(x => x.Id == obj.Id, obj, replaceOptions);
                }
                else
                {
                    session.WithTransaction(
                         (s, ct) =>
                             GetCollection(collectionName).ReplaceOne(s, x => x.Id == obj.Id, obj, replaceOptions, cancellationToken: ct)
                     );
                }
            }
            catch (MongoDB.Driver.MongoWriteException ex)
            {
                //A bulk write operation resulted in one or more errors.
                // E11000 duplicate key error index: bastapp.User.$Email dup key: {  }
                if (ex.Message.Contains("E11000"))
                {
                    throw new UserException("Index çakışması hatası!", ex);
                }
                else
                {
                    throw;
                }
            }
            return obj;
        }

        /// <summary>
        /// Kayıt siler
        /// </summary>
        /// <param name="obj">Silinecek kayıt.</param>
        /// <param name="collectionName">Kaydın bulunduğu collection. null geçilir ise tipin varsayılan collection'undan silinir</param>
        /// <exception cref="Bastapp.Global.Exceptions.CanNotDeleteException">Kayıt silinemez ise bu hatayı fırlatır.</exception>
        /// <returns></returns>
        internal virtual DeleteResult Remove(T obj, string collectionName = null)
        {
            var validateDeleteObj = obj as IValidateDelete;
            if (validateDeleteObj != null)
                validateDeleteObj.ValidateDelete();

            return GetCollection(collectionName).DeleteOne(x => x.Id == obj.Id);
        }
    }
}
