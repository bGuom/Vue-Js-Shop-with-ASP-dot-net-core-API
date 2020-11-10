using Backend_API.Models.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend_API.Models.ResourceModels
{
    public class ImageResourceModel : BaseResourceModel
    {
        public Guid ImageId { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
    }
}
