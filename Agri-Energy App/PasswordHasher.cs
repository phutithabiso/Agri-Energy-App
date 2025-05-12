using System;
using BCrypt.Net;

namespace Agri_Energy_App
{
    public static class PasswordHasher
    {
        private const int WorkFactor = 12;

        public static string HashPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password cannot be empty");

            return BCrypt.Net.BCrypt.EnhancedHashPassword(password, WorkFactor, HashType.SHA384);
        }

        public static bool VerifyPassword(string password, string hash)
        {
            if (string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(hash))
                return false;

            return BCrypt.Net.BCrypt.EnhancedVerify(password, hash, HashType.SHA384);
        }

        public static bool NeedsRehash(string hash)
        {
            return BCrypt.Net.BCrypt.PasswordNeedsRehash(hash, WorkFactor);
        }
    }
}