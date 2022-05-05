using Exam.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam.Dto.Forms
{
    public class ExamineForm : IValidatableObject
    {
        public ExamineForm()
        {

        }

        private Examination examination = null;
        private IEnumerable<AnswerForm> answers = null;

        public string Id { get; set; }

        public Dictionary<string, string> Answers { get; set; }

        public Dictionary<string, bool> Results
        {
            get
            {
                return this.GetAnswers().ToDictionary(x => x.QuestionId, y => y.IsCorrect);
            }
        }

        internal IEnumerable<AnswerForm> GetAnswers()
        {
            foreach(var a in Answers)
            {
                yield return new AnswerForm()
                {
                    Examination = this.Examination,
                    QuestionId = a.Key,
                    ChoiceId = a.Value
                };
            }
        }

        protected Examination Examination
        {
            get
            {
                if (examination == null || examination.Id.ToString() != this.Id)
                    examination = Models.Examination.Find(this.Id);

                return examination;
            }
        }

        public class AnswerForm
        {
            private Question question = null;

            public string QuestionId { get; set; }

            public string ChoiceId { get; set; }

            public bool IsCorrect
            {
                get
                {
                    return !string.IsNullOrEmpty(this.ChoiceId) && this.ChoiceId == this.Question.CorrectChoice?.Id.ToString();
                }
            }

            internal Examination Examination { get; set; }

            internal Question Question
            {
                get
                {
                    if (question == null || question.Id.ToString() != this.QuestionId)
                        question = this.Examination.Questions.FirstOrDefault(x => x.Id.ToString() == this.QuestionId);
                    return question;
                }
            }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (this.Examination == null)
                yield return new ValidationResult("Geçersiz sınav.");
        }
    }
}
