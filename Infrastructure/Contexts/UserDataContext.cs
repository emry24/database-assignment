using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Contexts;

public partial class UserDataContext(DbContextOptions<UserDataContext> options) : DbContext(options)
{

    public virtual DbSet<RoleEntity> Roles { get; set; }
    public virtual DbSet<UserEntity> Users { get; set; }
    public virtual DbSet<ProfileEntity> Profiles { get; set; }
    public virtual DbSet<UserAddressEntity> UserAddresses { get; set; }
    public virtual DbSet<UserAuthEntity> UserAuths { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RoleEntity>()
            .HasIndex(x => x.RoleName)
            .IsUnique();

        modelBuilder.Entity<UserAuthEntity>()
            .HasIndex(x => x.Email)
            .IsUnique();

    }
}
