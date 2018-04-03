namespace AxonifyIntegration.Data.DataModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class API_Course
    {
        [Key]
        [StringLength(53)]
        public string courseCode { get; set; }

        [Required]
        [StringLength(177)]
        public string courseName { get; set; }
    }
}
