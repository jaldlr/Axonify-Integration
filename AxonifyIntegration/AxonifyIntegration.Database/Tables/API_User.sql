CREATE TABLE [dbo].[API_User]
(
	[employeeId] NVARCHAR(255) NOT NULL PRIMARY KEY, 
    [active] BIT NOT NULL DEFAULT 1, 
    [fullName] NVARCHAR(100) NOT NULL, 
    [nickName] NVARCHAR(45) NULL, 
    [userName] NVARCHAR(60) NOT NULL, 
    [md5Password] NVARCHAR(MAX) NULL, 
    [userType] NVARCHAR(50) NOT NULL, 
    [email] NVARCHAR(75) NULL, 
    [language] NVARCHAR(10) NULL, 
    [hireDate] DATETIME NOT NULL, 
    [jobTitle] NVARCHAR(255) NOT NULL, 
    [department] NVARCHAR(60) NULL, 
    [team] NVARCHAR(60) NOT NULL, 
    [lineOfBusiness] NVARCHAR(60) NULL, 
    [suspended] BIT NOT NULL DEFAULT 0
)
