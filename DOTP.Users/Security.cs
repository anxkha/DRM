using System;
using System.Security.Cryptography;
using System.Text;

namespace DOTP.Users
{
    public static class Security
    {
        public static string CreateEncryptedPassword(string password)
        {
            return Sha256(Salt(password));
        }

        public static string Sha256(string text)
        {
            var bits = Encoding.UTF8.GetBytes(text);
            var hash = new SHA256CryptoServiceProvider().ComputeHash(bits);

            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }

        public static string Salt(string text)
        {
            return "Ga971n9AVy2w4bnao86" + text + "baN1907ta7513ikjna@mnvAsd21";
        }
    }
}
