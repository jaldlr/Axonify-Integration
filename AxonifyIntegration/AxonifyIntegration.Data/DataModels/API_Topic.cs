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
    using System.ComponentModel.DataAnnotations;

    public partial class API_Topic
    {
        [Required]
        [MaxLength(128)]
        public string topicExternalId { get; set; }

        [Required]
        [MaxLength(128)]
        public string subjectExternalId { get; set; }

        [Required]
        [MaxLength(60)]
        public string topicName { get; set; }

        public string revision { get; set; }
    }
}
