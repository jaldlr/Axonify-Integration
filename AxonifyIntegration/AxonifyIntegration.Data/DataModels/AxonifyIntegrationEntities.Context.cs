﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class AxonifyIntegrationEntities : DbContext
    {
        public AxonifyIntegrationEntities()
            : base("name=AxonifyIntegrationEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<API_Category> API_Category { get; set; }
        public virtual DbSet<API_CertificationCompletion> API_CertificationCompletion { get; set; }
        public virtual DbSet<API_Course> API_Course { get; set; }
        public virtual DbSet<API_EmployeeBaselineMetric> API_EmployeeBaselineMetric { get; set; }
        public virtual DbSet<API_EmployeeCurrentMetric> API_EmployeeCurrentMetric { get; set; }
        public virtual DbSet<API_EmployeeOveralMetric> API_EmployeeOveralMetric { get; set; }
        public virtual DbSet<API_EmployeeTopicGraduation> API_EmployeeTopicGraduation { get; set; }
        public virtual DbSet<API_EmplyeeSubjectCompletion> API_EmplyeeSubjectCompletion { get; set; }
        public virtual DbSet<API_KnowledgeMap> API_KnowledgeMap { get; set; }
        public virtual DbSet<API_Subject> API_Subject { get; set; }
        public virtual DbSet<API_Team> API_Team { get; set; }
        public virtual DbSet<API_Topic> API_Topic { get; set; }
        public virtual DbSet<API_User> API_User { get; set; }
        public virtual DbSet<API_UserAreasOfInterest> API_UserAreasOfInterest { get; set; }
        public virtual DbSet<API_UserBehavior> API_UserBehavior { get; set; }
        public virtual DbSet<API_UserPoint> API_UserPoint { get; set; }
        public virtual DbSet<API_UserTimeSpent> API_UserTimeSpent { get; set; }
    }
}
