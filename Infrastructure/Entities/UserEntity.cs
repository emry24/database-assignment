using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Entities;

public class UserEntity
{
    public UserEntity()
    {
        Created = DateTime.Now;
        Modified = DateTime.Now;
    }

    [Key]
    public Guid Id { get; set; }

    [Required]
    [Column(TypeName = "datetime2")]
    public DateTime Created { get; set; }


    // public Datetime Created { get; set; } = Datetime.Now();

    [Required]
    [Column(TypeName = "datetime2")]
    public DateTime Modified { get; set; }

    // public Datetime Modified { get; set; } = Datetime.Now();

    [Required]
    [ForeignKey(nameof(UserAddressEntity))]
    public int AddressId { get; set; }

    [Required]
    [ForeignKey(nameof(RoleEntity))]
    public int RoleId { get; set; }

    public virtual RoleEntity Role { get; set; } = null!;
    public virtual ProfileEntity Profile { get; set; } = null!;
    public virtual UserAddressEntity UserAddress { get; set; } = null!;
    public virtual UserAuthEntity UserAuth { get; set; } = null!;
}
