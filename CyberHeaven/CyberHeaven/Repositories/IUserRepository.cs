
using CyberHeaven.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace CyberHeaven.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        bool IsUsernameExists(string username);
        User GetByUsername(string username);
    }

    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(DbContext context) : base(context)
        {
        }

        public bool IsUsernameExists(string username)
        {
            return _dbSet.Any(u => u.Username == username);
        }

        public User GetByUsername(string username)
        {
            return _dbSet.FirstOrDefault(u => u.Username == username);
        }
    }
}