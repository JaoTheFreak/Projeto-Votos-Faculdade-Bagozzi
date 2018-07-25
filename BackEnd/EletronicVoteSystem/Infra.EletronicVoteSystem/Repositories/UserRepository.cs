using Infra.EletronicVoteSystem.Data;
using Infra.EletronicVoteSystem.Entities;
using Infra.EletronicVoteSystem.Interface;
using System;
using System.Linq;

namespace Infra.EletronicVoteSystem.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly EVSContext _dbContext;
        
        public UserRepository(EVSContext dbContext) 
            : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public User ValidateUser(User user)
        {
            try
            {
                return _dbContext.Users.FirstOrDefault(u => u.Email.Equals(user.Email) && u.Password.Equals(user.Password));
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
