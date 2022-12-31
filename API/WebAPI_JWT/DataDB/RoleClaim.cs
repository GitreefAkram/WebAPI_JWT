using System;
using System.Collections.Generic;

namespace WebAPI_JWT.DataDB
{
    public partial class RoleClaim
    {
        public int Id { get; set; }
        public string RoleId { get; set; } = null!;
        public string? ClaimType { get; set; }
        public string? ClaimValue { get; set; }

        public virtual SoftwarePermission Role { get; set; } = null!;
    }
}
