using Backend_API.Models.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend_API.Models.ResourceModels
{
    public class RatingResourceModel : BaseResourceModel
    {
        public Guid RatingId { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public int Rate { get; set; }
        public string Review { get; set; }
    }
}
