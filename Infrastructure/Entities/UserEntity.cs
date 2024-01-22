using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Entities;

public class UserEntity
{

    [Key]
    public Guid Id { get; set; }

    [Required]
    [Column(TypeName = "datetime2")]
    public DateTime Created { get; set; }



    [Required]
    [Column(TypeName = "datetime2")]
    public DateTime Modified { get; set; }

    

    [Required]
    [ForeignKey(nameof(RoleEntity))]
    public int RoleId { get; set; }

    public virtual RoleEntity Role { get; set; } = null!;
    public virtual ProfileEntity Profile { get; set; } = null!;
    public virtual UserAddressEntity UserAddress { get; set; } = null!;
    public virtual UserAuthEntity UserAuth { get; set; } = null!;
}
