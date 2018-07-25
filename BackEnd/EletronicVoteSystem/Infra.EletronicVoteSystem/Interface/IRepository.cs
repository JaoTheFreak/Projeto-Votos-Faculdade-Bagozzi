using System;
using System.Linq;

namespace Infra.EletronicVoteSystem.Interface
{
    public interface IRepository<T> where T : class 
    {
        bool Create(T entity);
        
        T GetById(int id);

        IQueryable<T> GetAll();

        T Get(Func<T, bool> predicate);
        
        bool Update(T entity);
        
        bool Delete(T entity);

        bool DeleteById(int id);
    }
}
