//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AxonifyIntegration.Data.DataModels
{
    using System;
    using System.Collections.Generic;
    
    public partial class API_EmployeeOveralMetric
    {
        public int overallMetricId { get; set; }
        public string employeeId { get; set; }
        public Nullable<int> totalAnswerCount { get; set; }
        public Nullable<int> correctAnswerCount { get; set; }
        public Nullable<decimal> percentCorrect { get; set; }
        public Nullable<int> totalConfidence { get; set; }
        public Nullable<decimal> averageConfidence { get; set; }
    }
}
