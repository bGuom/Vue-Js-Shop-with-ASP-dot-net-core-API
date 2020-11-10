using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Backend_API.Models.EntityModels
{
    public class Image : BaseEntity
    {
        public Image()
        {
            ImageId = Guid.NewGuid();
        }

        [Key]
        public Guid ImageId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Path { get; set; }
    }
}
