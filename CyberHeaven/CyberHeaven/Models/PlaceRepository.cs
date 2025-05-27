using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberHeaven.Models
{
    public class PlaceRepository
    {
        private readonly AppDbContext _context;

        public PlaceRepository()
        {
            _context = new AppDbContext();
            _context.Database.EnsureCreated(); // Создает БД, если ее нет (для SQL Server лучше миграции)
        }

        public List<Place> GetAllUsers() => _context.Places.ToList();

        public void AddUser(Place place)
        {
            _context.Places.Add(place);
            _context.SaveChanges();
        }

        // Другие методы (Update, Delete, FindByEmail и т.д.)
    }
}
