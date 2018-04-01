namespace AxonifyIntegration.Data.DataModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class API_EmployeeOveralMetric
    {
        [Key]
        public int overallMetricId { get; set; }

        [StringLength(255)]
        public string employeeId { get; set; }

        public int? totalAnswerCount { get; set; }

        public int? correctAnswerCount { get; set; }

        public decimal? percentCorrect { get; set; }

        public int? totalConfidence { get; set; }

        public decimal? averageConfidence { get; set; }
    }
}
