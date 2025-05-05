using Microsoft.EntityFrameworkCore;
using BCrypt.Net;
using PRS_API_AB.Models;

namespace PRS_API_AB.Services
{
    public class AuthService(PrsDbContext context)
    {
        private readonly PrsDbContext _context = context;

        public async Task<User?> AuthenticateAsync(string username, string password)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == username);
            if (user == null || !VerifyPassword(password, user.PasswordHash))
            {
                return null;
            }
            return user;
        }

        private static bool VerifyPassword(string password, string storedHash)
        {
            return BCrypt.Net.BCrypt.Verify(password, storedHash);
        }
    }
}
