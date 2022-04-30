using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam.Data
{
    public interface IEntity
    {
        ObjectId Id { get; set; }
        void Save();
        void Remove();
        bool IsNew();

        IDeleteInfo Deleted { get; set; }
    }
}
