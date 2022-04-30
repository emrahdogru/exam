using Exam.Models;
using Exam.Utility.Exceptions;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Exam.Dto.Forms
{
    public class LoginForm
    {
        [Required(ErrorMessage = "Kullanıcı adı zorunludur.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Parola zorunludur.")]
        public string Password { get; set; }

        public User GetUser()
        {
            var user = User.FindByUsername(this.Username);

            if (user?.IsValidPassword(this.Password) != true)
                throw new UserException("Geçersiz kullanıcı adı yada parola.");

            return user;
        }

        public Results.LoginResult GetResult(string tokenSecret)
        {
            var user = GetUser();

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(tokenSecret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new Results.LoginResult()
            {
                Username = user.Username,
                Token = tokenHandler.WriteToken(token)
            };
        }
    }
}
