using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam.Data.Db
{
    internal class DbManager
    {
        private static DbManager _current = null;

        public static DbManager Current
        {
            get
            {
                if (_current == null)
                {
                    _current = new DbManager();
                }

                return _current;
            }
        }

        private DbManager()
        {

        }

        private string GetConnectionString()
        {
            // Konfigürasyon dosyası veya parola kasası servisinden oluşturulabilir...
            return "mongodb://localhost:27017";
        }

        private MongoClient client = null;
        private object lockClient = new object();
        public MongoClient GetClient()
        {
            lock (lockClient)
            {
                if (client == null)
                {
                    try
                    {
                        MongoDB.Bson.Serialization.BsonSerializer.RegisterSerializer(typeof(DateTime), new MongoDateTimeSerializer());
                    }
                    catch (MongoDB.Bson.BsonSerializationException ex)
                    {
                        ExamLog.Add("MongoDateTimeSerializer, register edilemedi.", null, ex: ex);
                    }

                    string connStr = GetConnectionString();

                    if (string.IsNullOrWhiteSpace(connStr))
                    {
                        throw new ArgumentException("`Exam´ ConnectionString tanımlı değil.");
                    }

                    client = new MongoClient(connStr);
                }
            }
            return client;
        }

        public IMongoDatabase GetGlobalDb()
        {
            return GetClient().GetDatabase("Exam");
        }

    }
}
