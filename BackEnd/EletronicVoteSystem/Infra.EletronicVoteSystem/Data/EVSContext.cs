using Infra.EletronicVoteSystem.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infra.EletronicVoteSystem.Data
{
    public class EVSContext : DbContext
    {
        public EVSContext(DbContextOptions<EVSContext> options)
        : base(options)
        {
            
        }

        public DbSet<User> Users { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
            
        //}

    }
}
