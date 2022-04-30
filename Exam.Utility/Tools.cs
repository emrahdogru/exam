using System;
using System.Security.Cryptography;
using System.Text;

namespace Exam.Utility
{
    public static class Tools
    {

        /// <summary>
        /// SHA256 şifreleme
        /// </summary>
        /// <param name="input">Şifrenelecek veri</param>
        /// <returns>Şifrelenmiş veri</returns>
        public static string Sha256(byte[] input)
        {
            if (input == null) return null;
            var sha256 = SHA256CryptoServiceProvider.Create();
            byte[] data = sha256.ComputeHash(input);
            StringBuilder stb = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                stb.Append(data[i].ToString("x2"));
            }
            return stb.ToString();
        }

        /// <summary>
        /// SHA256 şifreleme
        /// </summary>
        /// <param name="input">Şifrenelecek veri</param>
        /// <returns>Şifrelenmiş veri</returns>
        public static string Sha256(string input)
        {
            return Sha256(Encoding.UTF8.GetBytes(input));
        }
    }
}
