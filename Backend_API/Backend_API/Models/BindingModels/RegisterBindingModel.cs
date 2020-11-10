using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Backend_API.Models
{
    public class RegisterBindingModel
    {
            
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public string Role { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }


        
    }
}
