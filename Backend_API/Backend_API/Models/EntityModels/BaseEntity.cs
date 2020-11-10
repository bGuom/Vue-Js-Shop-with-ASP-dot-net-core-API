using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend_API.Models.EntityModels
{
    public class BaseEntity
    {
        protected BaseEntity()
        {
            DateCreated = DateTime.UtcNow;
            DateModified = DateTime.UtcNow;
        }

        public bool IsDeleted { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }

    }
}
