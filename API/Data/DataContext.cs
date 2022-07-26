using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class DataContext : IdentityDbContext<AppUser, AppRole, int,
        IdentityUserClaim<int>, AppUserRole, IdentityUserLogin<int>,
        IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        private readonly IConfiguration Configuration;

        public DataContext(DbContextOptions options, IConfiguration configuration) : base(options)
        {
            Configuration = configuration;
        }

        // protected override void OnConfiguring(DbContextOptionsBuilder options)
        // {
        //     var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        //     if (env == "Development")
        //     {
        //         var connectionString = Configuration.GetConnectionString("DefaultConnection1");
        //         options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        //     }
        // }
        public DbSet<UserLike> Likes { get; set; }
        public DbSet<UserStatus> Status { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Connection> Connections { get; set; }
        public DbSet<FriendRequest> Freinds { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<AppUser>()
               .HasMany(ur => ur.UserRoles)
               .WithOne(u => u.User)
               .HasForeignKey(ur => ur.UserId)
               .IsRequired();

            builder.Entity<AppRole>()
                .HasMany(ur => ur.UserRoles)
                .WithOne(u => u.Role)
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();

            builder.Entity<UserLike>()
            .HasKey(k => new { k.SourceUserId, k.LikedUserId });

            builder.Entity<UserLike>()
            .HasOne(s => s.SourceUser)
            .WithMany(l => l.LikedUsers)
            .HasForeignKey(s => s.SourceUserId)
            .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<UserLike>()
           .HasOne(s => s.LikedUser)
           .WithMany(l => l.LikedByUsers)
           .HasForeignKey(s => s.LikedUserId)
           .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Message>()
            .HasOne(u => u.Sender)
            .WithMany(m => m.MessageSent)
            .OnDelete(DeleteBehavior.Restrict);


            builder.Entity<Message>()
            .HasOne(u => u.Recipient)
            .WithMany(m => m.MessageRecived)
            .OnDelete(DeleteBehavior.Restrict);

             builder.Entity<FriendRequest>()
            .HasOne(r => r.Requester)
            .WithMany(u => u.RequestedRequests)
            .OnDelete(DeleteBehavior.ClientCascade);
                builder.Entity<FriendRequest>()
            .HasOne(r => r.Receiver)
            .WithMany(u => u.RecevedRequests)
            .OnDelete(DeleteBehavior.ClientCascade);

        }
    }
}