﻿namespace HanimeliApp.Domain.Dtos.User;

public class CreateUserRequest
{
    public string Name { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Password { get; set; }
    public string Role { get; set; } = "User";
}