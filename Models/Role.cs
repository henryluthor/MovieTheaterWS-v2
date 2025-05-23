﻿using System;
using System.Collections.Generic;

namespace MovieTheaterWS_v2.Models;

public partial class Role
{
    public int IdRole { get; set; }

    public string Description { get; set; } = null!;

    public virtual ICollection<SystemUser> Systemusers { get; set; } = new List<SystemUser>();
}
