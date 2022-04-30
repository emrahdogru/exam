using Exam.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam.Models
{
    public class Examination : Entity<Examination>
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string WiredUrl { get; set; }

        public IEnumerable<Question> Questions { get; set; }

    }
}
