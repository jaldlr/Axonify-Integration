CREATE TABLE [dbo].[API_EmployeeBaselineMetric]
(
	[baselineMetricId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [employeeId] NVARCHAR(255) NULL, 
    [totalAnswerCount] INT NULL, 
    [correctAnswerCount] INT NULL, 
    [percentCorrect] DECIMAL(2, 2) NULL, 
    [totalConfidence] INT NULL, 
    [averageConfidence] DECIMAL(2, 2) NULL
)
