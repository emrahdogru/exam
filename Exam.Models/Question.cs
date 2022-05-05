using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam.Models
{
    public class Question
    {
        private Choice correctChoice = null;

        public ObjectId Id { get; set; }

        public string Content { get; set; }

        public IEnumerable<Choice> Choices { get; set; }

        [BsonElement]
        internal ObjectId CorrectChoiceId { get; set; }

        [BsonIgnore]
        public Choice CorrectChoice
        {
            get
            {
                if (correctChoice == null || correctChoice.Id != this.CorrectChoiceId)
                    correctChoice = this.Choices.FirstOrDefault(x => x.Id == this.CorrectChoiceId);

                return correctChoice;
            }
            set
            {
                if (value == null)
                    throw new NullReferenceException("CorrectChoice null olamaz.");

                correctChoice = value;
                this.CorrectChoiceId = value.Id;
            }
        }

        public class Choice
        {
            public ObjectId Id { get; set; }
            public string Text { get; set; }
        }
    }
}
