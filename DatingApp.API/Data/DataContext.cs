using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore; // mi treba za DbContext

namespace DatingApp.API.Data
{
    public class DataContext : DbContext // crven underline poradi missing ENTITY.FRAMEWOTK.cORE/ go instalirav preku ctrl+P
    { //DbContext can be used to query and save instances of your entities
        public DataContext(DbContextOptions<DataContext> options) : base(options) {} // DbContextOptions koristi DataContext! 
        //DbContextOptions method will be called to configure the database (and other options) to be used for this context.
        
        public DbSet<Value> Values {get; set;} // DbSet used when the type of entity is not known at build time.
        public DbSet<User>  User { get; set; } // posle ova moras Migrations, odnosno posle sekoj nov model go pravis ova!!! stavas u DataContext i posle migrationss
        public DbSet<Photo> Photos {get; set;}

    }  
}