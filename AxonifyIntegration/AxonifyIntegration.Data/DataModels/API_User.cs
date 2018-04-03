namespace AxonifyIntegration.Data.DataModels
{
    using Object.Constants;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Globalization;
    using Newtonsoft.Json;
    using System.Runtime.Serialization;
    using System.Linq;

    [DataContract]
    public partial class API_User
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public API_User()
        {
            API_UserAreasOfInterest = new HashSet<API_UserAreasOfInterest>();
            areasOfInterest = new List<string>();
        }

        [Key]
        [StringLength(255)]
        [DataMember]
        public string employeeId { get; set; }

        public bool active { get; set; }

        [Required]
        [StringLength(100)]
        [DataMember]
        public string fullName { get; set; }

        [StringLength(45)]
        [DataMember]
        public string nickName { get; set; }

        [Required]
        [StringLength(60)]
        [DataMember]
        public string userName { get; set; }

        [DataMember]
        public string md5Password { get; set; }

        [Required]
        [StringLength(50)]
        [DataMember]
        public string userType { get; set; }

        [StringLength(75)]
        [DataMember]
        public string email { get; set; }

        [StringLength(10)]
        [DataMember]
        public string language { get; set; }

        [Required]
        [Column("hireDate", TypeName = "datetime")]
        public DateTime hireDateDt { get; set; }

        [Required]
        [StringLength(255)]
        [DataMember]
        public string jobTitle { get; set; }

        [StringLength(60)]
        [DataMember]
        public string department { get; set; }

        [Required]
        [StringLength(60)]
        [DataMember]
        public string team { get; set; }

        [StringLength(60)]
        [DataMember]
        public string lineOfBusiness { get; set; }

        [DataMember]
        public bool suspended { get; set; }
        
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<API_UserAreasOfInterest> API_UserAreasOfInterest { get; set; }

        #region NotMapped properties

        [NotMapped]
        [DataMember]
        public List<string> areasOfInterest { set; get; }

        [NotMapped]
        [DataMember]
        public string hireDate
        {
            get
            {
                return this.hireDateDt.ToString(DateFormats.generalDateFormat);
            }
            set
            {
                if(value != null)
                {
                    this.hireDateDt = DateTime.ParseExact(value, DateFormats.generalDateFormat, CultureInfo.InvariantCulture);
                }
            } 
        }

        #endregion
    }
}
