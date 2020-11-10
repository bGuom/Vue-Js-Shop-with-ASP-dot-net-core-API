using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Backend_API.Models.EntityModels
{
    public class Dish : BaseEntity
    {
        public Dish() {
            DishId = Guid.NewGuid();
            DishTotalRating = 0;
            Images = new List<Image>();
            Ratings = new List<Rating>();
        }

        [Key]
        public Guid DishId { get; set; }
        [Required]
        public string DishName { get; set; }
        [Required]
        public string DishDescrition { get; set; }
        [Required]
        public decimal DishPrice { get; set; }
        public decimal DishTotalRating { get; set; }
        public List<Image> Images { set; get; }

        public List<Rating> Ratings { set; get; }

    }
}
