using System;
using System.Collections.Generic;

namespace WebAPI_JWT.DataDB
{
    public partial class SoftwarePermission
    {
        public SoftwarePermission()
        {
            RoleClaims = new HashSet<RoleClaim>();
            Users = new HashSet<User>();
        }

        public string Id { get; set; } = null!;
        public string? Name { get; set; }
        public string? NormalizedName { get; set; }
        public string? ConcurrencyStamp { get; set; }

        public virtual ICollection<RoleClaim> RoleClaims { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
