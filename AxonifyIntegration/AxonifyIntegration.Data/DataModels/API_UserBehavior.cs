namespace AxonifyIntegration.Data.DataModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class API_UserBehavior
    {
        [Key]
        public int userBehaviorId { get; set; }

        [StringLength(255)]
        public string observerEmployeeId { get; set; }

        [StringLength(255)]
        public string observeeEmployeeId { get; set; }

        public DateTime? observationDateTime { get; set; }

        [StringLength(60)]
        public string behaviorCode { get; set; }

        public int? positiveObservations { get; set; }

        public int? negativeObservations { get; set; }

        public string comment { get; set; }
    }
}
