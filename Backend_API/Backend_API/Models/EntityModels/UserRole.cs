using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend_API.Models
{
    public class UserRole : IdentityRole<Guid>
    {

        public string Description { get; set; }
        public DateTime DateCreated { get; set; }

    }
}
