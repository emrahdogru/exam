using Exam.Models;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Exam.Models.Question;

namespace Exam.Dto.Forms
{
    public class QuestionForm
    {
        private Question question = null;

        public QuestionForm()
        {

        }
        public QuestionForm(Question question)
        {
            if (question == null)
                throw new ArgumentNullException(nameof(question));

            this.Id = question.Id.ToString();
            this.Content = question.Content;
            this.Choices = question.Choices.Select(x => new ChoiceForm(x));
        }

        public string Id { get; set; }
        public string Content { get; set; }

        public IEnumerable<ChoiceForm> Choices { get; set; }

        [Required(ErrorMessage = "Doğru şık belirtilmedi.")]
        public string CorrectChoiceId { get; set; }

        public Question GetBinded(Examination examination)
        {
            if (examination == null)
                throw new ArgumentNullException(nameof(examination));

            var q = examination.Questions?.FirstOrDefault(x => x.Id == ObjectId.Parse(this.Id));

            if (q == null)
                q = new Question() { Id = ObjectId.Parse(this.Id) };

            q.Content = this.Content;
            q.Choices = this.Choices.Select(x => x.GetBinded(q)).ToArray();
            q.CorrectChoice = q.Choices.FirstOrDefault(x => x.Id == ObjectId.Parse(this.CorrectChoiceId));

             return q;
        }

        public class ChoiceForm
        {
            public ChoiceForm() { }

            public ChoiceForm(Choice choice)
            {
                if (choice == null)
                    throw new ArgumentNullException(nameof(choice));

                this.Id = choice.Id.ToString();
                this.Text = choice.Text;
            }

            public string Id { get; set; }
            public string Text { get; set; }

            public Choice GetBinded(Question question)
            {
                var c = question.Choices?.FirstOrDefault(x => x.Id == ObjectId.Parse(this.Id));
                if (c == null)
                    c = new Choice() { Id = ObjectId.Parse(this.Id) };

                c.Text = this.Text;

                return c;
            }
        }
    }
}
