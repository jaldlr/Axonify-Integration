CREATE TABLE [dbo].[API_SubjectCompletion]
(
	[employeeId] NVARCHAR(255) NOT NULL PRIMARY KEY, 
    [categoryExternalId] NVARCHAR(60) NULL, 
    [subjectExternalId] NVARCHAR(60) NULL, 
    [graduationTimestamp] DATETIME NULL, 
    [baselineAverage] DECIMAL(2, 1) NULL, 
    [baselineCorrectAnswerCount] INT NULL, 
    [baselineAnswerCount] INT NULL, 
    [currentAverage] DECIMAL(2, 1) NULL, 
    [currentCorrectAnswerCount] INT NULL, 
    [currentAnswerCount] INT NULL, 
    [overallAverage] DECIMAL(2, 1) NULL, 
    [overallCorrectAnswerCount] INT NULL, 
    [overallAnswerCount] INT NULL, 
    [timeSpent] INT NULL
)
