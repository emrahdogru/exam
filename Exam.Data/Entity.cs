using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam.Data
{

    public abstract class Entity : IEntity
    {
        private ObjectId _id = ObjectId.Empty;

        [BsonId(IdGenerator = typeof(MongoDB.Bson.Serialization.IdGenerators.ObjectIdGenerator))]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("_id")]
        public ObjectId Id
        {
            get
            {
                if (_id == ObjectId.Empty)
                    _id = ObjectId.GenerateNewId();
                return _id;
            }
            set
            {
                _id = value;
            }
        }

        [BsonElement]
        public DeleteInfo Deleted { get; internal set; }

        IDeleteInfo IEntity.Deleted { get => this.Deleted; set => this.Deleted = (DeleteInfo)value; }

        [BsonExtraElements]
        internal BsonDocument ExtraElements { get; set; }

        [BsonElement]
        public DateTime CreateDate { get; internal set; } = DateTime.Now;

        public abstract void Remove();
        public abstract bool IsNew();
        public abstract void Save();
        /// <summary>
        /// Kaydı silindi olarak işaretler
        /// </summary>
        /// <param name="user">Silme işlemini yapan kullanıcı</param>
        public virtual void Delete(IUser user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var validateDeleteObj = this as IValidateDelete;
            if (validateDeleteObj != null)
                validateDeleteObj.ValidateDelete();

            this.Deleted = new DeleteInfo()
            {
                Date = DateTime.Now,
                IsDeleted = true,
                UserId = user.Id
            };
            this.Save();
        }

    }

    public abstract class Entity<T> : Entity where T : Entity
    {
        private static Repository<T> _repository = null;

        private static Repository<T> Repository
        {
            get
            {
                if (_repository == null)
                    _repository = new Repository<T>();
                return _repository;
            }
        }


        public static T Find(ObjectId id)
        {
            return Repository.AsQueryable().FirstOrDefault(x => x.Id == id);
        }

        public static T Find(string id)
        {
            return Find(ObjectId.Parse(id));
        }

        /// <summary>
        /// Kaydı siler.
        /// </summary>
        /// <exception cref="Bastapp.Global.Exceptions.CanNotDeleteException">Kayıt silinemez ise...</exception>
        public override void Remove()
        {
            Repository.Remove(this as T);
        }


        public override bool IsNew()
        {
            return !All().Any(x => x.Id == this.Id);
        }

        public override void Save()
        {
            Repository.Save(this as T);
        }

        /// <summary>
        /// Tüm kayıtlar
        /// </summary>
        /// <param name="includeDeleted">Silinmiş olanlar da dahil mi?</param>
        /// <returns></returns>
        public static IQueryable<T> All(bool includeDeleted = false)
        {
            var result = Repository.AsQueryable();

            if (!includeDeleted)
                result = result.Where(x => x.Deleted == null || x.Deleted.IsDeleted == false);

            return result;
        }

        public static IQueryable<T> DeletedEntities()
        {
            return Repository.AsQueryable().Where(x => x.Deleted.IsDeleted == true);
        }
    }
}
