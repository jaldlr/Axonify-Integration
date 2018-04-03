namespace AxonifyIntegration.Data.DataModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class API_UserPoint
    {
        [Key]
        [StringLength(255)]
        public string employeeId { get; set; }

        public int? Points { get; set; }
    }
}
