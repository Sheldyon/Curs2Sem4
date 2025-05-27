using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberHeaven.Models
{
    public class PlaceCategoryRepository
    {
        private readonly AppDbContext _context;

        public PlaceCategoryRepository()
        {
            _context = new AppDbContext();
            _context.Database.EnsureCreated(); // Создает БД, если ее нет (для SQL Server лучше миграции)
        }

        public List<PlaceCategory> GetAllUsers() => _context.PlaceCategories.ToList();

        public void AddUser(PlaceCategory placeCategory)
        {
            _context.PlaceCategories.Add(placeCategory);
            _context.SaveChanges();
        }

        // Другие методы (Update, Delete, FindByEmail и т.д.)
    }
}
