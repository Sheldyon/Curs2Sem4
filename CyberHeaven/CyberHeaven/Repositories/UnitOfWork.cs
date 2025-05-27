
using CyberHeaven.Models;
using System.Threading.Tasks;

namespace CyberHeaven.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private IUserRepository _users;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public IRepository<Place> Places { get; }
        public IRepository<PlaceCategory> PlaceCategories { get; }
        public IRepository<Booking> Bookings { get; }
        public IRepository<PromoCode> PromoCodes { get; }
        public IRepository<Review> Reviews { get; }

        public IUserRepository Users => _users ??= new UserRepository(_context);

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}