using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace MovieTheaterWS_v2.Models;

public partial class User : IdentityUser
{
    //public int Id { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    //public string Email { get; set; } = null!;

    //public string PasswordHash { get; set; } = null!;

    //public int IdRole { get; set; }

    //public virtual Role IdRoleNavigation { get; set; } = null!;
}
