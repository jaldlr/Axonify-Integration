CREATE TABLE [dbo].[API_UserAreasOfInterest]
(
	[employeeId] NVARCHAR(255) NOT NULL , 
    [areaOfInterest] NVARCHAR(100) NOT NULL, 
    PRIMARY KEY ([areaOfInterest], [employeeId]), 
    CONSTRAINT [FK_API_UserAreasOfInterest_API_User] FOREIGN KEY ([employeeId]) REFERENCES [API_User]([employeeId])
)
