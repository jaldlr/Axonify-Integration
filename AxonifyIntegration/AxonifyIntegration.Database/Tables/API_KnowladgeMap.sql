CREATE TABLE [dbo].[API_KnowledgeMap]
(
	[knowledgeMapId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [employeeId] NVARCHAR(255) NULL, 
    [categoryExternalId] NVARCHAR(60) NULL, 
    [subjectExternalId] NVARCHAR(60) NULL, 
    [topicExternalId] NVARCHAR(60) NULL, 
    [learnerStatusId] INT NULL, 
    [certificationStatusId] INT NULL, 
    [introductoryStatusId] INT NULL, 
    [trainingModuleCompletedId] INT NULL, 
    [trainingModuleLastCompletionTimestamp] DATETIME NULL, 
    [topicGraduationTimestamp] DATETIME NULL 
)
