using Exam.Models;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam.Dto.Forms
{
    public class ExaminationForm
    {
        public ExaminationForm(Examination examination)
        {
            if (examination == null)
                throw new ArgumentNullException(nameof(examination));

            this.Id = examination.Id.ToString();
            this.Title = examination.Title;
            this.Content = examination.Title;
            this.Questions = examination.Questions.Select(x => new QuestionForm(x));
        }

        public ExaminationForm()
        {

        }

        public string Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string WiredUrl { get; set; }

        public IEnumerable<QuestionForm> Questions { get; set; }

        public Examination GetBinded()
        {
            var examination = Examination.Find(this.Id);
            if (examination == null)
                examination = new Examination() { Id = ObjectId.Parse(this.Id) };

            examination.Title = this.Title;
            examination.Content = this.Content;
            examination.WiredUrl = this.WiredUrl;
            examination.Questions = this.Questions.Select(x => x.GetBinded(examination)).ToArray();

            return examination;
        }
    }
}
