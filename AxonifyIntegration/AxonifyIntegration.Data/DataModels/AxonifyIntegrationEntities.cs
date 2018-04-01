namespace AxonifyIntegration.Data.DataModels
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class AxonifyIntegrationEntities : DbContext
    {
        public AxonifyIntegrationEntities()
            : base("name=AxonifyIntegrationEntities")
        {
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
        public virtual DbSet<API_UserBehavior> API_UserBehavior { get; set; }
        public virtual DbSet<API_userPoint> API_userPoint { get; set; }
        public virtual DbSet<API_UserTimeSpent> API_UserTimeSpent { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<API_CertificationCompletion>()
                .Property(e => e.assessmentScore)
                .HasPrecision(2, 1);

            modelBuilder.Entity<API_EmployeeBaselineMetric>()
                .Property(e => e.percentCorrect)
                .HasPrecision(2, 2);

            modelBuilder.Entity<API_EmployeeBaselineMetric>()
                .Property(e => e.averageConfidence)
                .HasPrecision(2, 2);

            modelBuilder.Entity<API_EmployeeCurrentMetric>()
                .Property(e => e.percentCorrect)
                .HasPrecision(2, 2);

            modelBuilder.Entity<API_EmployeeCurrentMetric>()
                .Property(e => e.averageConfidence)
                .HasPrecision(2, 2);

            modelBuilder.Entity<API_EmployeeOveralMetric>()
                .Property(e => e.percentCorrect)
                .HasPrecision(2, 2);

            modelBuilder.Entity<API_EmployeeOveralMetric>()
                .Property(e => e.averageConfidence)
                .HasPrecision(2, 2);
        }
    }
}
