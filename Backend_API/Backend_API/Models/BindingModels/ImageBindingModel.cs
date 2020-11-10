using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Backend_API.Models.BindingModels
{
    public class ImageBindingModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public IFormFile Image { get; set; }
    }
}
