﻿namespace Infrastructure.Dtos;


public class UserRegistrationDto
{

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? StreetName { get; set; }

    public string? PostalCode { get; set; }

    public string? City { get; set; }

    public string RoleName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    //public DateTime Created { get; set; } = DateTime.Now;



}
