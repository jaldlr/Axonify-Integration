namespace AxonifyIntegration.Data.DataModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class API_UserAreasOfInterest
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(255)]
        public string employeeId { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(100)]
        public string areaOfInterest { get; set; }

        public virtual API_User API_User { get; set; }
    }
}
