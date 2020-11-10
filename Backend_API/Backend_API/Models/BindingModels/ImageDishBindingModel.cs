using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Backend_API.Models.BindingModels
{
    public class ImageDishBindingModel
    {
        [Key]
        public Guid ImageId { get; set; }

    }
}
