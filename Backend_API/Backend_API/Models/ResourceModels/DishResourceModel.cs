using Backend_API.Models.EntityModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Backend_API.Models.ResourceModels
{
    public class DishResourceModel : BaseResourceModel
    {

        public Guid DishId { get; set; }
        public string DishName { get; set; }
        public string DishDescription { get; set; }
        [DisplayFormat(DataFormatString = "{0:[format]}", ApplyFormatInEditMode = true)]
        public decimal DishPrice { get; set; }
        [DisplayFormat(DataFormatString = "{0:[format]}", ApplyFormatInEditMode = true)]
        public decimal DishTotalRating { get; set; }
        public List<ImageResourceModel> Images { set; get; }
        public List<RatingResourceModel> Ratings { set; get; }

    }
}
