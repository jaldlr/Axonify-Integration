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
    
    public partial class API_UserAreasOfInterest
    {
        public string employeeId { get; set; }
        public string areaOfInterest { get; set; }
    
        public virtual API_User API_User { get; set; }
    }
}
