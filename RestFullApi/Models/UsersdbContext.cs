using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace RestFullApi.Models
{
    public class UsersdbContext : IdentityDbContext<ApplicationUser>
    {
        //public UsersdbContext(DbContextOptions<UsersdbContext> options)
        //    : base(options)
        //{


        //}
        public UsersdbContext(DbContextOptions<UsersdbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
        //public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
        public virtual DbSet<Category> Category => Set<Category>();
        public virtual DbSet<News> News => Set<News>();
        public virtual DbSet<CustomPages> CustomPages => Set<CustomPages>();
        public virtual DbSet<Comments> Comments => Set<Comments>();
    }
}
