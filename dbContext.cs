using System.Security.Cryptography.X509Certificates;
using listip;
using Microsoft.EntityFrameworkCore;
using MySql.EntityFrameworkCore;
namespace listip.data{

    public class listipDbContext : DbContext
    {
        public listipDbContext(DbContextOptions<listipDbContext> context) : base(context){}
            public DbSet<IP> IP{get; set;}
    }

}