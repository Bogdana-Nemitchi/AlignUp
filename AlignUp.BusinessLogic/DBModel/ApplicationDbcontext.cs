using AlignUp.Domain.Core;
using System;
using System.Data.Entity;
using System.Linq;
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext() : base("DefaultConnection")
    {
    }

    public DbSet<UserDbTable> Users { get; set; }
    public DbSet<Session> Sessions { get; set; }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<UserDbTable>()
            .ToTable("Users")
            .HasKey(u => u.Id);

        modelBuilder.Entity<Session>()
            .ToTable("Sessions")
            .HasKey(s => s.Id);
    }

    // 👇 Aceasta este metoda corectă
    public void SeedAdmin()
    {
        try
        {
            if (!this.Users.Any(u => u.Username == "admin"))
            {
                var hashedPassword = PasswordHelper.HashPassword("admin");

                this.Users.Add(new UserDbTable
                {
                    Username = "admin",
                    Email = "admin@alignup.com",
                    Password = hashedPassword,
                    RegistrationDateTime = DateTime.Now,
                    RegistrationIp = "127.0.0.1",
                    LastLogin = DateTime.Now,
                    LastIp = "127.0.0.1",
                    UserRole = UserApi.UserRole.Admin
                });

                this.SaveChanges();
                System.Diagnostics.Debug.WriteLine("✔ Utilizatorul admin a fost creat.");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("ℹ Utilizatorul admin există deja.");
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"❌ Eroare la crearea utilizatorului admin: {ex.Message}");
        }
    }
}
