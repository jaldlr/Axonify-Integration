namespace AxonifyIntegration.Data.DataModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class API_User
    {
        [Key]
        [StringLength(255)]
        public string employeeId { get; set; }

        public bool? active { get; set; }

        [StringLength(100)]
        public string fullName { get; set; }

        [StringLength(45)]
        public string nickName { get; set; }

        [StringLength(60)]
        public string Username { get; set; }

        public string md5Password { get; set; }

        public int? userTypeId { get; set; }

        [StringLength(75)]
        public string Email { get; set; }

        public int? languageId { get; set; }

        public DateTime? hireDate { get; set; }

        [StringLength(255)]
        public string jobTitle { get; set; }

        [StringLength(60)]
        public string department { get; set; }

        [StringLength(60)]
        public string team { get; set; }

        [StringLength(60)]
        public string lineOfBusiness { get; set; }

        public int? areasOfInterestId { get; set; }

        public bool? suspended { get; set; }
    }
}
