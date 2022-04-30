using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam.Data
{
    public class DeleteInfo : IDeleteInfo
    {
        public bool IsDeleted { get; internal set; } = false;
        public DateTime? Date { get; internal set; }
        public ObjectId UserId { get; internal set; }
    }
}
