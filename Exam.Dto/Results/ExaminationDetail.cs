using Exam.Models;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam.Dto.Results
{
    public class ExaminationDetail
    {
        public ExaminationDetail(Examination exam)
        {
            this.Id = exam.Id.ToString();
            this.Content = exam.Content;
            this.WiredUrl = exam.WiredUrl;
            this.Title = exam.Title;
            this.Questions = exam.Questions.Select(x => new QuestionDetail(x)).ToArray();
        }

        public string Id { get; private set; }
        public string Content { get; private set; }
        public string WiredUrl { get; private set; }
        public string Title { get; private set; }
        public IEnumerable<QuestionDetail> Questions { get; private set; }

        public class QuestionDetail
        {
            public QuestionDetail(Question question)
            {
                this.Content = question.Content;
                this.Id = question.Id.ToString();
                this.Choices = question.Choices.Select(x => new ChoiceDetail(x)).ToArray();
            }

            public string Content { get; private set; }
            public string Id { get; private set; }
            public IEnumerable<ChoiceDetail> Choices { get; private set; }
        }

        public class ChoiceDetail
        {
            public ChoiceDetail(Question.Choice choice)
            {
                this.Id = choice.Id.ToString();
                this.Text = choice.Text;
            }

            public string Id { get; private set; }
            public string Text { get; private set; }
        }
    }
}
