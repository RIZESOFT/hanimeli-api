﻿namespace HanimeliApp.Domain.Dtos.User;

public class UserLoginRequest
{
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
}