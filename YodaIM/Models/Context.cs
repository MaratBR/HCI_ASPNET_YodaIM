using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YodaIM.Models
{
    public class Context : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<MessageAttachment> MessageAttachments { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<FileModel> FileModels { get; set; }
        public DbSet<BinaryBlob> BinaryBlobs { get; set; }
        public DbSet<UserRoom> UserRooms { get; set; }

        public Context(DbContextOptions<Context> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var user = builder.Entity<User>();
            user
                .HasIndex(u => u.UserName)
                .IsUnique();

            user
                .Property(u => u.Gender);

            builder.Entity<BinaryBlob>()
                .Property(fm => fm.Sha256)
                .HasColumnType("BINARY(32)");

            builder.Entity<MessageAttachment>()
                .HasKey(nameof(MessageAttachment.FileModelId), nameof(MessageAttachment.MessageId));


            builder.Entity<MessageAttachment>()
               .HasOne(a => a.Message)
               .WithMany(m => m.MessageAttachments)
               .HasForeignKey(a => a.MessageId);

            builder.Entity<MessageAttachment>()
                .HasOne(a => a.FileModel)
                .WithMany(fm => fm.MessageAttachments)
                .HasForeignKey(a => a.FileModelId);

            builder.Entity<UserRoom>()
                .HasKey(ur => new { ur.RoomId, ur.UserId });

            builder.Entity<UserRoom>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.Rooms)
                .HasForeignKey(ur => ur.UserId);

            builder.Entity<UserRoom>()
                .HasOne(ur => ur.Room)
                .WithMany(r => r.Users)
                .HasForeignKey(ur => ur.RoomId);
        }
    }
}
