namespace AxonifyIntegration.Data.DataModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class API_EmployeeCurrentMetric
    {
        [Key]
        public int currentMetricId { get; set; }

        [StringLength(255)]
        public string employeeId { get; set; }

        public int? totalAnswerCount { get; set; }

        public int? correctAnswerCount { get; set; }

        public decimal? percentCorrect { get; set; }

        public int? totalConfidence { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? averageConfidence { get; set; }
    }
}
