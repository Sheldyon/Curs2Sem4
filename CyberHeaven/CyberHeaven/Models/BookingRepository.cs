using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberHeaven.Models
{
    public class BookingRepository
    {
        private readonly AppDbContext _context;

        public BookingRepository()
        {
            _context = new AppDbContext();
            _context.Database.EnsureCreated(); // Создает БД, если ее нет (для SQL Server лучше миграции)
        }

        public List<Booking> GetAllUsers() => _context.Bookings.ToList();

        public void AddUser(Booking booking)
        {
            _context.Bookings.Add(booking);
            _context.SaveChanges();
        }

        // Другие методы (Update, Delete, FindByEmail и т.д.)
    }
}
