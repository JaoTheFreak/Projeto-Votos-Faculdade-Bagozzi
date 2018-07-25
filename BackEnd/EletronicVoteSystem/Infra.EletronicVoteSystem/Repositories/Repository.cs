using Infra.EletronicVoteSystem.Data;
using Infra.EletronicVoteSystem.Interface;
using System;
using System.Linq;

namespace Infra.EletronicVoteSystem.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly EVSContext _dbContext;

        public Repository(EVSContext dbContext)
        {
            _dbContext = dbContext;
        }

        public bool Create(T entity)
        {
            try
            {
                _dbContext.Set<T>().Add(entity);
                Save();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public T GetById(int id)
        {
            try
            {
                return _dbContext.Set<T>().Find(id);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public IQueryable<T> GetAll()
        {
            try
            {
                return _dbContext.Set<T>();
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public T Get(Func<T, bool> predicate)
        {
            try
            {
                return _dbContext.Set<T>().Where(predicate).SingleOrDefault();
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public bool Update(T entity)
        {
            try
            {
                _dbContext.Set<T>().Update(entity);
                Save();

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool Delete(T entity)
        {
            try
            {
                _dbContext.Set<T>().Remove(entity);
                Save();
                
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool DeleteById(int id)
        {
            try
            {
                _dbContext.Set<T>().Remove(GetById(id));
                Save();

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        private void Save()
        {
            _dbContext.SaveChanges();
        }
    }
}
