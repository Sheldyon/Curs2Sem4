using System.Threading.Tasks;
using CyberHeaven.Models;

namespace CyberHeaven.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        IRepository<Place> Places { get; }
        IRepository<PlaceCategory> PlaceCategories { get; }
        IRepository<Booking> Bookings { get; }
        IRepository<PromoCode> PromoCodes { get; }
        IRepository<Review> Reviews { get; }

        int Complete();
    }
}