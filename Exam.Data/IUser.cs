using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam.Data
{
    public interface IUser : IEntity
    {
        string Username { get; set; }
        string Password { set; }

    }
}
