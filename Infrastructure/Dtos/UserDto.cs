namespace Infrastructure.Dtos;

public class UserDto
{
    //public string Id { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? StreetName { get; set; }

    public string? PostalCode { get; set; }

    public string? City { get; set; }

    public string RoleName { get; set; } = null!;
}
