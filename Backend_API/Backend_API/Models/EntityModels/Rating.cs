using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Backend_API.Models.EntityModels
{
    public class Rating : BaseEntity
    {

        public Rating()
        {
            RatingId = Guid.NewGuid();
        }


        [Key]
        public Guid RatingId { get; set; }
        [Required]
        public Guid DishId { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        [Required]
        public int Rate { get; set; }
        public string Review { get; set; }

    }
}
