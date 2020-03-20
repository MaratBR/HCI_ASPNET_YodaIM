

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace YodaIM.Models
{
    public class AuthDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public AuthDbContext([NotNullAttribute] DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var user = builder.Entity<User>();
            user.HasIndex(u => u.UserName).IsUnique();
            user.HasIndex(u => u.Alias).IsUnique();
            user.HasIndex(u => u.PhoneNumber).IsUnique();
        }
    }

    public class User
    {
        public int Id { get; set; }
        public string PhoneNumber { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required] [JsonIgnore]
        public string PasswordHash { get; set; }

        public string Alias { get; set; }

        [JsonIgnore]
        public ICollection<UserRole> UserRoles { get; set; }

        [JsonIgnore]
        public ICollection<Role> Roles => UserRoles.Select(ur => ur.Role).ToList();

        public ICollection<string> RoleNames => Roles.Select(r => r.Name).ToList();
    }

    public class Role
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [JsonIgnore]
        public ICollection<UserRole> UserRoles { get; set; }
    }

    public class UserRole
    {
        public int UserId { get; set; }

        public int RoleId { get; set; }

        public User User { get; set; }

        public Role Role { get; set; }
    }
}
