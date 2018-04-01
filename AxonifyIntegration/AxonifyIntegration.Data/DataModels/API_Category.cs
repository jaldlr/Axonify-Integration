namespace AxonifyIntegration.Data.DataModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class API_Category
    {
        [Key]
        public string categoryExternalId { get; set; }

        [Required]
        [StringLength(60)]
        public string categoryName { get; set; }

        [StringLength(128)]
        public string revision { get; set; }
    }
}
