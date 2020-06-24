using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore; // mi treba za DbContext

namespace DatingApp.API.Data
{
    public class DataContext : DbContext // crven underline poradi missing ENTITY.FRAMEWOTK.cORE/ go instalirav preku ctrl+P
    { //DbContext can be used to query and save instances of your entities
        public DataContext(DbContextOptions<DataContext> options) : base(options) {} 
        //DbContextOptions method will be called to configure the database (and other options) to be used for this context.
        
        public DbSet<Value> Values {get; set;} // DbSet used when the type of entity is not known at build time.
        public DbSet<User>  User { get; set; } // posle ova moras Migrations da ne zaboravis, vazi za site
        public DbSet<Photo> Photos {get; set;}
        public DbSet<Like> Likes {get; set;} // 153





        //Likes
        // Pri sekoe kreiranje na Tabela Model od DB, ke se prati sledniov metod
         protected override void OnModelCreating(ModelBuilder builder){ //153

             builder.Entity<Like> () //Entity, vrakja objekt koj moze da se dodade vo tabela modelot(bezrazlika dali postoel prethodno vo modelot)
                .HasKey(k => new { k.LikerId, k.LikeeId}); //  gi zimam SITE klucevi (id) od Likes (bidejki e nzs, edno Id ednas go ima i ne moze ista licnost povekje pati da te lajkne)
            
            builder.Entity<Like> () // one to many + one to many = many to many // vo Core momentalno e Edinstven nacin 
                .HasOne(u => u.Likee)
                .WithMany(u => u.Likers)
                .HasForeignKey(u => u.LikeeId)
                .OnDelete(DeleteBehavior.Restrict); // za da ne go brise i Userot
                
            builder.Entity<Like> ()
                .HasOne(u => u.Liker)
                .WithMany(u => u.Likees)
                .HasForeignKey(u => u.LikerId)
                .OnDelete(DeleteBehavior.Restrict); 
         } // migriraj! za da se pojavi tabelata

        

    }  
}