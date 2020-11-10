using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend_API.Models.ResourceModels
{
    public class ResponseModel
    {
        public string Status { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public Object Data { get; set; }
    }
        
} 
