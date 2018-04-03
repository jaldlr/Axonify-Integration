namespace AxonifyIntegration.Data.DataModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class API_CertificationCompletion
    {
        [Key]
        [StringLength(255)]
        public string employeeId { get; set; }

        [StringLength(60)]
        public string categoryExternalId { get; set; }

        [StringLength(60)]
        public string subjectExternalId { get; set; }

        [StringLength(60)]
        public string topicExternalId { get; set; }

        public DateTime? completionTimestamp { get; set; }

        public decimal? assessmentScore { get; set; }

        public int? timeSpent { get; set; }
    }
}
