CREATE TABLE [dbo].[API_UserTimeSpent]
(
	[userTimeSpentId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [employeeId] NVARCHAR(255) NULL, 
    [totalTime] INT NULL, 
    [dailyTrainingTime] INT NULL, 
    [introductoryTrainingTime] INT NULL, 
    [certificationTrainingTime] INT NULL, 
    [refresherTrainingTime] INT NULL, 
    [extraQuestionTrainingTime] INT NULL, 
    [extraTrainingModueTime] INT NULL, 
    [gamePlayTime] INT NULL
)
