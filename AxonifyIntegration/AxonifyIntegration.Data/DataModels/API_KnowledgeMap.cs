namespace AxonifyIntegration.Data.DataModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class API_KnowledgeMap
    {
        [Key]
        public int knowledgeMapId { get; set; }

        [StringLength(255)]
        public string employeeId { get; set; }

        [StringLength(60)]
        public string categoryExternalId { get; set; }

        [StringLength(60)]
        public string subjectExternalId { get; set; }

        [StringLength(60)]
        public string topicExternalId { get; set; }

        public int? learnerStatusId { get; set; }

        public int? certificationStatusId { get; set; }

        public int? introductoryStatusId { get; set; }

        public int? trainingModuleCompletedId { get; set; }

        public DateTime? trainingModuleLastCompletionTimestamp { get; set; }

        public DateTime? topicGraduationTimestamp { get; set; }
    }
}
