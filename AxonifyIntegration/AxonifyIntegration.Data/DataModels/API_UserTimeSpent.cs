namespace AxonifyIntegration.Data.DataModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class API_UserTimeSpent
    {
        [Key]
        public int userTimeSpentId { get; set; }

        [StringLength(255)]
        public string employeeId { get; set; }

        public int? totalTime { get; set; }

        public int? dailyTrainingTime { get; set; }

        public int? introductoryTrainingTime { get; set; }

        public int? certificationTrainingTime { get; set; }

        public int? refresherTrainingTime { get; set; }

        public int? extraQuestionTrainingTime { get; set; }

        public int? extraTrainingModueTime { get; set; }

        public int? gamePlayTime { get; set; }
    }
}
