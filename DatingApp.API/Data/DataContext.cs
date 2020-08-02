
using DatingApp.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{
    public class DataContext : IdentityDbContext<User, Role, int,
      IdentityUserClaim<int>, UserRole, IdentityUserLogin<int>,
      IdentityRoleClaim<int>, IdentityUserToken<int>>// dodadeni se so cel da bide polesno ponatamu
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        //DbContextOptions method will be called to configure the database (and other options) to be used for this context.

        public DbSet<Value> Values { get; set; } // DbSet used when the type of entity is not known at build time.
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Like> Likes { get; set; } // 153
        public DbSet<Message> Messages { get; set; } // 153
        public DbSet<Room> Rooms { get; set; }
        public DbSet<RoomPhoto> RoomPhotos { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<Notifications> Notifications { get; set; }
        public DbSet<NotificationUser> NotificationUser { get; set; } //s






        //Likes
        // Pri sekoe kreiranje na Tabela Model od DB, ke se prati sledniov metod
        protected override void OnModelCreating(ModelBuilder builder)
        { //153

            base.OnModelCreating(builder); // za da nema Error


             builder.Entity<NotificationUser>(notificationUser =>
            {
                notificationUser.HasKey(ur => new { ur.NotificationId, ur.ReaderId });

                notificationUser.HasOne(u => u.Notification).WithMany(r => r.notificationUsers).HasForeignKey(ur => ur.NotificationId).IsRequired();
                notificationUser.HasOne(u => u.Reader).WithMany(r => r.notificationUsers).HasForeignKey(ur => ur.ReaderId).IsRequired();
            });

            builder.Entity<UserRole>(userRole =>
            {
                userRole.HasKey(ur => new { ur.UserId, ur.RoleId });

                userRole.HasOne(u => u.Role).WithMany(r => r.UserRoles).HasForeignKey(ur => ur.RoleId).IsRequired();
                userRole.HasOne(u => u.User).WithMany(r => r.UserRoles).HasForeignKey(ur => ur.UserId).IsRequired();
            });


            builder.Entity<Like>() //Entity, vrakja objekt koj moze da se dodade vo tabela modelot(bezrazlika dali postoel prethodno vo modelot)
               .HasKey(k => new { k.LikerId, k.LikeeId }); //  gi zimam SITE klucevi (id) od Likes (bidejki e nzs, edno Id ednas go ima i ne moze ista licnost povekje pati da te lajkne)

            builder.Entity<Like>() // one to many + one to many = many to many // vo Core momentalno e Edinstven nacin 
                .HasOne(u => u.Likee)
                .WithMany(u => u.Likers)
                .HasForeignKey(u => u.LikeeId)
                .OnDelete(DeleteBehavior.Restrict); // za da ne go brise i Userot

            builder.Entity<Like>()
                .HasOne(u => u.Liker)
                .WithMany(u => u.Likees)
                .HasForeignKey(u => u.LikerId)
                .OnDelete(DeleteBehavior.Restrict);


            // MESSAGES
            builder.Entity<Message>() // one to many + one to many = many to many // vo Core momentalno e Edinstven nacin 
                .HasOne(u => u.Sender) // Message.Sender   
                .WithMany(m => m.MessagesSent) // .withMany (Message.Sender.MessagesSent[])
                .OnDelete(DeleteBehavior.Restrict); // za da ne go brise i Userot

            builder.Entity<Message>() // one to many + one to many = many to many // vo Core momentalno e Edinstven nacin 
               .HasOne(u => u.Recipient) // Message.Recipient 
               .WithMany(m => m.MessagesReceived) // .withMany (Message.Sender.MessagesReceived[])
               .OnDelete(DeleteBehavior.Restrict); // za da ne go brise i Userot


        } // migriraj! za da se pojavi tabelata



    }
}