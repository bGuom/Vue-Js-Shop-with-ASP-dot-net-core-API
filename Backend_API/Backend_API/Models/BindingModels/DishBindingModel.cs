using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Backend_API.Models.BindingModels
{
    public class DishBindingModel
    {
        [Required]
        public string DishName { get; set; }
        [Required]
        public string DishDescription { get; set; }
        [Required]
        public decimal DishPrice { get; set; }
        public List<Guid> Images { set; get; }


    }
}
