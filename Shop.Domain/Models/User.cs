using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Shop.Domain.Enums;

namespace Shop.Domain.Models;

[Table("users")]
public class User : BaseEntity
{
    [Column("id")]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    [EmailAddress]
    [Column("email")]
    public string Email { get; set; } = string.Empty;

    [Required]
    [Column("password_hash")]
    public string PasswordHash { get; set; } = string.Empty;

    [Column("role")]
    public UserRole Role { get; set; } = UserRole.User;

    [Column("is_active")]
    public bool IsActive { get; set; } = true;

    [Column("reset_password_token")]
    public string? ResetPasswordToken { get; set; }

    [Column("reset_password_token_expires")]
    public DateTime? ResetPasswordTokenExpires { get; set; }

    public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    
    public ICollection<Order> Orders { get; set; } = new List<Order>();
}
