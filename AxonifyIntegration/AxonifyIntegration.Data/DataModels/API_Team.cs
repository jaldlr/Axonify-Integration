namespace AxonifyIntegration.Data.DataModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class API_Team
    {
        [Key]
        public int teamId { get; set; }

        [StringLength(60)]
        public string parentName { get; set; }

        [StringLength(60)]
        public string teamName { get; set; }
    }
}
