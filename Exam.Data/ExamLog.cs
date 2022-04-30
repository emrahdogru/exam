using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam.Data
{
    public class ExamLog
    {
        public static void Add(string description, IUser user, object extra = null, int severity = 1, Exception ex = null)
        {
            // Log işlemleri
        }
    }
}
