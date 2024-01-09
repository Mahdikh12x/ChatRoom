using System.Security.Cryptography;
using System.Text;

namespace _01_framework.Application
{
    public interface ISecurity
    {
        string GetSHA256Hash(string val);
        bool CheckSHA256Hach(string value, string hash);
    }

    public class Security : ISecurity
    {
        public bool CheckSHA256Hach(string value, string hash)
        {
            var hashText = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(value));

            if (value.Equals(hashText)) return true; return false;

        }

        public string GetSHA256Hash(string val)
        {
            if (val == null) return "";

            var hashCode = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(val));

            return Convert.ToBase64String(hashCode);
        }

    }

}
