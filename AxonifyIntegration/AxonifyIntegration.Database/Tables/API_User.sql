CREATE TABLE [dbo].[API_User]
(
	[employeeId] NVARCHAR(255) NOT NULL PRIMARY KEY, 
    [active] BIT NULL, 
    [fullName] NVARCHAR(100) NULL, 
    [nickName] NVARCHAR(45) NULL, 
    [Username] NVARCHAR(60) NULL, 
    [md5Password] NVARCHAR(MAX) NULL, 
    [userTypeId] INT NULL, 
    [Email] NVARCHAR(75) NULL, 
    [languageId] INT NULL, 
    [hireDate] DATETIME NULL, 
    [jobTitle] NVARCHAR(255) NULL, 
    [department] NVARCHAR(60) NULL, 
    [team] NVARCHAR(60) NULL, 
    [lineOfBusiness] NVARCHAR(60) NULL, 
    [areasOfInterestId] INT NULL, 
    [suspended] BIT NULL
)
