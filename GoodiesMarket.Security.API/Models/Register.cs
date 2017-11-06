﻿using GoodiesMarket.Components.Models;
using System.ComponentModel.DataAnnotations;

namespace GoodiesMarket.Security.API.Models
{
    public class Register
    {
        [Required,
        Display(Name = "Email"),
        DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required,
        StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6),
        DataType(DataType.Password),
        Display(Name = "Password")]
        public string Password { get; set; }


        [Required]
        public RoleType RoleType { get; set; }
    }
}