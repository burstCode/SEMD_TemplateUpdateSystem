using TemplatesWebsite.Data;
using TemplatesWebsite.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;


namespace TemplatesWebsite.Services
{
    public class UserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User> AuthenticateAsync(string email, string password)
        {
            var hashedPassword = HashPassword(password);
            var user = await _context.Users.SingleOrDefaultAsync(u => u.User_Email == email && u.User_Password == hashedPassword);
            return user;
        }

        // Хэширование пароля
        public static string HashPassword(string password)
        {
            using (SHA512 sha512 = SHA512.Create())
            {
                byte[] bytes = sha512.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
