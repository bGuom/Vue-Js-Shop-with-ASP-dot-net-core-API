using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Backend_API.Models.BindingModels
{
    public class RatingBindingModel
    {
        [Required]
        public Guid DishId { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string UserEmail { get; set; }
        [Required]
        public int Rate { get; set; }
        public string Review { get; set; }
    }
}
