using Exam.Data;
using Exam.Utility.Exceptions;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam.Models
{
    /// <summary>
    /// Sınav sonuçları
    /// </summary>
    public class ExamResult : Entity<ExamResult>
    {
        private Examination examination = null;
        private User user = null;
        private IEnumerable<Answer> answers = null;

        public ExamResult(Examination examination, User user)
        {
            if (examination == null)
                throw new ArgumentNullException(nameof(examination));

            if (user == null)
                throw new ArgumentNullException(nameof(user));

            this.Examination = examination;
            this.User = user;
        }

        [BsonConstructor]
        internal ExamResult()
        { }

        [BsonElement]
        internal ObjectId ExaminationId { get; private set; }

        /// <summary>
        /// İligli sınav
        /// </summary>
        [BsonIgnore]
        public Examination Examination
        {
            get
            {
                if (examination == null || examination.Id != this.ExaminationId)
                    examination = Examination.Find(this.ExaminationId);

                return examination;
            }
            set
            {
                if (value == null)
                    throw new NullReferenceException(nameof(Examination));

                examination = value;
                ExaminationId = value.Id;
            }
        }

        [BsonElement]
        internal ObjectId UserId { get; private set; }

        /// <summary>
        /// Sınava giren kullanıcı
        /// </summary>
        [BsonIgnore]
        public User User
        {
            get
            {
                if (user == null || user.Id != this.UserId)
                    user = Models.User.Find(this.UserId);

                return user;
            }
            set
            {
                if (value == null)
                    throw new NullReferenceException(nameof(User));

                this.user = value;
                this.UserId = value.Id;
            }
        }

        public void SetAnswer(Question question, Question.Choice choice)
        {
            if (question == null)
                throw new ArgumentNullException(nameof(question));

            if (choice == null)
                throw new ArgumentNullException(nameof(choice));

            if (!this.Examination.Questions.Any(x => x.Id == question.Id))
                throw new UserException("Soru, bu sınava ait değil.");

            if (!question.Choices.Any(x => x.Id == choice.Id))
                throw new UserException("Seçilen şık bu soruya ait değil.");

            var answer = this.Answers.FirstOrDefault(x => x.QuestionId == question.Id);
            if(answer == null)
            {
                answer = new Answer(this.Examination, question);
                answers = answers.Append(answer);
            }

            answer.Choice = choice;
        }

        [BsonElement]
        public IEnumerable<Answer> Answers
        {
            get
            {
                return answers;
            }
            protected set
            {
                answers = value;
                foreach(var a in answers)
                {
                    a.Examination = this.Examination;
                }
            }
        }

        public class Answer
        {
            private Question question = null;
            private Question.Choice choice = null;

            public Answer()
            { }

            internal Answer(Examination examination, Question question)
            {
                if (examination == null)
                    throw new ArgumentNullException(nameof(examination));

                if (question == null)
                    throw new ArgumentNullException(nameof(question));

                this.Examination = examination;
                this.Question = question;
            }

            [BsonIgnore]
            internal Examination Examination { get; set; }

            [BsonElement]
            internal ObjectId QuestionId { get; private set; }

            [BsonIgnore]
            public Question Question
            {
                get
                {
                    if (question == null || question.Id != this.QuestionId)
                        question = this.Examination.Questions.FirstOrDefault(x => x.Id == this.QuestionId);

                    return question;
                }
                set
                {
                    if (value == null)
                        throw new NullReferenceException(nameof(Question));

                    this.Question = value;
                    this.QuestionId = value.Id;
                }
            }

            [BsonElement]
            internal ObjectId ChoiceId { get; private set; }

            [BsonIgnore]
            public Question.Choice Choice
            {
                get
                {
                    if (choice == null || choice.Id != this.ChoiceId)
                        choice = this.Question.Choices.FirstOrDefault(x => x.Id == this.ChoiceId);

                    return choice;
                }
                set
                {
                    if (value == null)
                        throw new NullReferenceException(nameof(choice));

                    choice = value;
                    this.ChoiceId = value.Id;
                }
            }

            /// <summary>
            /// Doğru cevap mı?
            /// </summary>
            [BsonElement]
            public bool IsCorrect
            {
                get
                {
                    return this.Question.CorrectChoiceId == this.ChoiceId;
                }
            }
        }
    }
}
