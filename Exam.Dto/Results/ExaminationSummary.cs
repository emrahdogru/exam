using Exam.Models;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam.Dto.Results
{
    public class ExaminationSummary
    {
        private Examination examination = null;

        public ExaminationSummary(Examination examination)
        {
            this.examination = examination;
        }

        public ObjectId Id
        {
            get
            {
                return examination.Id;
            }
        }

        public string Title
        {
            get
            {
                return examination.Title;
            }
        }
    }
}
