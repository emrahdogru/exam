using Exam.Data;
using Exam.Utility.Exceptions;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Linq;

namespace Exam.Models
{
    [Data.Db.Db("User")]
    public class User : Entity<User>
    {
        private string passwordSalt = null;


        public string Username { get; set; }
        
        [BsonElement]
        private string HashedPasword { get; set; }

        [BsonElement]
        private string PasswordSalt
        {
            get
            {
                if (passwordSalt == null)
                    passwordSalt = Guid.NewGuid().ToString("N");
                return passwordSalt;
            }
            set
            {
                passwordSalt = value;
            }
        }

        private string GenereateHashedPassword(string password)
        {
            return Utility.Tools.Sha256(password + this.PasswordSalt);
        }

        public void SetPassword(string password)
        {
            if (password == null)
                throw new ArgumentNullException(nameof(password));

            if (password.Length < 8)
                throw new UserException("Parola en az 8 karakter olmalıdır.");

            this.HashedPasword = GenereateHashedPassword(password);
        }

        public bool IsValidPassword(string password)
        {
            if (password == null)
                throw new ArgumentNullException(nameof(password));

            var hashedPassword = GenereateHashedPassword(password);

            return hashedPassword.Equals(this.HashedPasword);
        }

        public static User FindByUsername(string username)
        {
            if (username == null)
                throw new ArgumentNullException(nameof(username));

            return User.All().AsQueryable().FirstOrDefault(x => x.Username == username);
        }
    }
}
