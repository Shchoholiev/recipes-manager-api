﻿namespace RecipesManagerApi.Application.Models.Identity;
public class RegisterModel
{
    public string Name { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public string Password { get; set; }
}