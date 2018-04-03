namespace AxonifyIntegration.Data.DataModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class API_Topic
    {
        [Key]
        public string topicExternalId { get; set; }

        [Required]
        [StringLength(128)]
        public string subjectExternalId { get; set; }

        [Required]
        [StringLength(60)]
        public string topicName { get; set; }

        [StringLength(128)]
        public string revision { get; set; }
    }
}
