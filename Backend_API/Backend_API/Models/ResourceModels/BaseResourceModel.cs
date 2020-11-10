using System;


namespace Backend_API.Models.EntityModels
{
    public class BaseResourceModel
    {

        public bool IsDeleted { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }

    }
}
