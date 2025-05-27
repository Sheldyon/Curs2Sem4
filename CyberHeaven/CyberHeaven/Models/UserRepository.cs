using System.Collections.Generic;
using System.Linq;

namespace CyberHeaven.Models
{
    public class UserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository()
        {
            _context = new AppDbContext();
            _context.Database.EnsureCreated(); // Создает БД, если ее нет (для SQL Server лучше миграции)
        }

        public List<User> GetAllUsers() => _context.Users.ToList();

        public void AddUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        // Другие методы (Update, Delete, FindByEmail и т.д.)
    }
}