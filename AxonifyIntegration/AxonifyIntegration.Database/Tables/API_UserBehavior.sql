CREATE TABLE [dbo].[API_UserBehavior]
(
	[userBehaviorId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [observerEmployeeId] NVARCHAR(255) NULL, 
    [observeeEmployeeId] NVARCHAR(255) NULL, 
    [observationDateTime] DATETIME NULL, 
    [behaviorCode] NVARCHAR(60) NULL, 
    [positiveObservations] INT NULL, 
    [negativeObservations] INT NULL, 
    [comment] NVARCHAR(MAX) NULL
)
