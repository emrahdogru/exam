using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam.Data.Db
{
    [AttributeUsage(AttributeTargets.Class)]
    public class DbAttribute : Attribute
    {
        public DbAttribute(string collectionName)
        {
            this.CollectionName = collectionName;
        }

        public string CollectionName { get; set; }
    }
}
