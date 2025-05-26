using System;

public class UserDbTable
{
    public int Id { get; set; }

    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }

    public bool IsActive { get; set; }

    public DateTime RegistrationDateTime { get; set; }
    public string RegistrationIp { get; set; }

    public DateTime? LastLogin { get; set; }  // ← suportă fallback cu `??`
    public string LastIp { get; set; }

    public string FirstName { get; set; }
    public string LastName { get; set; }

    public string UserRole { get; set; }

    // Pentru compatibilitate cu codul existent (dar acum setabile)
    public string PasswordHash { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool EmailConfirmed { get; set; } = true;
    public DateTime? LastLoginAt { get; set; }
    public int Role { get; set; }
}
