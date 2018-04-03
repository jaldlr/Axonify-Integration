namespace AxonifyIntegration.Data.DataModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class API_EmplyeeSubjectCompletion
    {
        [Key]
        [StringLength(255)]
        public string employeeId { get; set; }

        [StringLength(60)]
        public string categoryExternalId { get; set; }

        [StringLength(60)]
        public string subjectExternalId { get; set; }

        public DateTime? graduationTimestamp { get; set; }

        public int? timeSpent { get; set; }

        public int? baselineMetricId { get; set; }

        public int? currentMetricId { get; set; }

        public int? overalMetricId { get; set; }
    }
}
