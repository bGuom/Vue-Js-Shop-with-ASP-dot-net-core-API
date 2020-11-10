
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend_API.Models.ResourceModels
{
    public class UserResourceModel
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
    }
}
