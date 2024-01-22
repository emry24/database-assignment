using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Entities;

[Index(nameof(Email), IsUnique = true)]
public class UserAuthEntity
{
    [Key]
    [ForeignKey(nameof(UserEntity))]
    public Guid UserId { get; set; }

    [Required]
    [Column(TypeName = "varchar(100)")]
    public string Email { get; set; } = null!;

    [Required]
    [Column(TypeName = "varchar(200)")]
    public string Password { get; set; } = null!;

    public virtual UserEntity User { get; set; } = null!;
}
