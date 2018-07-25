using Infra.EletronicVoteSystem.Entities;

namespace Infra.EletronicVoteSystem.Interface
{
    public interface IUserRepository : IRepository<User>
    {
        User ValidateUser(User user);
    }
}
