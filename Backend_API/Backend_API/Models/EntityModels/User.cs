using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend_API.Models
{
    public class User : IdentityUser<Guid>
    {
        public string DisplayName { get; set; }
    }
}
