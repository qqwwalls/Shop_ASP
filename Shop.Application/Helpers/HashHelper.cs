using Shop.Application.Interfaces.Helpers;

namespace Shop.Application.Helpers
{
    public class HashHelper : IHashHelper
    {
        public string Hash(string password)
        {
            return BCrypt.Net.BCrypt.EnhancedHashPassword(password);
        }

        public bool IsValidPassword(string password, string hash)
        {
            return BCrypt.Net.BCrypt.EnhancedVerify(password, hash);
        }
    }
}
