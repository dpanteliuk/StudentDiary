using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace StudentDiary.UI.Code
{
    static class Encryptor
    {
        public static string GetHash(string text)
        {
            using (MD5 cryptoProvider = new MD5CryptoServiceProvider())
            {
                var result = cryptoProvider.ComputeHash(Encoding.Default.GetBytes(text));
                var stringResult = new StringBuilder();
                foreach (var resultSymbol in result)
                {
                    stringResult.Append(resultSymbol.ToString("x2"));
                }
                return stringResult.ToString();
            }
        }
    }
}
