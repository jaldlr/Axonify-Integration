
USE [master]
GO

/****** Object:  Database [AxonifyIntegration]    Script Date: 24/08/2018 06:30:45 p. m. ******/
CREATE DATABASE [AxonifyIntegration] ON  PRIMARY 
( NAME = N'AxonifyIntegration', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL10_50.MSSQLSERVER\MSSQL\DATA\AxonifyIntegration.mdf' , SIZE = 3072KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'AxonifyIntegration_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL10_50.MSSQLSERVER\MSSQL\DATA\AxonifyIntegration_log.ldf' , SIZE = 1280KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO

IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [AxonifyIntegration].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO

ALTER DATABASE [AxonifyIntegration] SET ANSI_NULL_DEFAULT OFF
GO

ALTER DATABASE [AxonifyIntegration] SET ANSI_NULLS OFF
GO

ALTER DATABASE [AxonifyIntegration] SET ANSI_PADDING OFF
GO

ALTER DATABASE [AxonifyIntegration] SET ANSI_WARNINGS OFF
GO

ALTER DATABASE [AxonifyIntegration] SET ARITHABORT OFF
GO

ALTER DATABASE [AxonifyIntegration] SET AUTO_CLOSE OFF
GO

ALTER DATABASE [AxonifyIntegration] SET AUTO_CREATE_STATISTICS ON
GO

ALTER DATABASE [AxonifyIntegration] SET AUTO_SHRINK OFF
GO

ALTER DATABASE [AxonifyIntegration] SET AUTO_UPDATE_STATISTICS ON
GO

ALTER DATABASE [AxonifyIntegration] SET CURSOR_CLOSE_ON_COMMIT OFF
GO

ALTER DATABASE [AxonifyIntegration] SET CURSOR_DEFAULT  GLOBAL
GO

ALTER DATABASE [AxonifyIntegration] SET CONCAT_NULL_YIELDS_NULL OFF
GO

ALTER DATABASE [AxonifyIntegration] SET NUMERIC_ROUNDABORT OFF
GO

ALTER DATABASE [AxonifyIntegration] SET QUOTED_IDENTIFIER OFF
GO

ALTER DATABASE [AxonifyIntegration] SET RECURSIVE_TRIGGERS OFF
GO

ALTER DATABASE [AxonifyIntegration] SET  DISABLE_BROKER
GO

ALTER DATABASE [AxonifyIntegration] SET AUTO_UPDATE_STATISTICS_ASYNC OFF
GO

ALTER DATABASE [AxonifyIntegration] SET DATE_CORRELATION_OPTIMIZATION OFF
GO

ALTER DATABASE [AxonifyIntegration] SET TRUSTWORTHY OFF
GO

ALTER DATABASE [AxonifyIntegration] SET ALLOW_SNAPSHOT_ISOLATION OFF
GO

ALTER DATABASE [AxonifyIntegration] SET PARAMETERIZATION SIMPLE
GO

ALTER DATABASE [AxonifyIntegration] SET READ_COMMITTED_SNAPSHOT OFF
GO

ALTER DATABASE [AxonifyIntegration] SET RECOVERY FULL
GO

ALTER DATABASE [AxonifyIntegration] SET  MULTI_USER
GO

ALTER DATABASE [AxonifyIntegration] SET PAGE_VERIFY CHECKSUM
GO

ALTER DATABASE [AxonifyIntegration] SET DB_CHAINING OFF
GO

EXEC sys.sp_db_vardecimal_storage_format N'AxonifyIntegration', N'ON'
GO

USE [AxonifyIntegration]
GO

/****** Object:  StoredProcedure [dbo].[axf_usp_InterfaceGetPendingUsers]    Script Date: 24/08/2018 06:30:51 p. m. ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/****** Object:  StoredProcedure [dbo].[axf_usp_InterfaceHistoryUpdate]    Script Date: 24/08/2018 06:30:51 p. m. ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/****** Object:  StoredProcedure [dbo].[axf_usp_InterfaceUpdateTopicGraduations]    Script Date: 24/08/2018 06:30:51 p. m. ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/****** Object:  Table [dbo].[axf_Interfaces]    Script Date: 24/08/2018 06:30:51 p. m. ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

SET ANSI_PADDING OFF
GO

/****** Object:  Table [dbo].[axf_Interfaces_HistoryCalls]    Script Date: 24/08/2018 06:30:51 p. m. ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

SET ANSI_PADDING OFF
GO

USE [master]
GO

ALTER DATABASE [AxonifyIntegration] SET  READ_WRITE
GO

ALTER DATABASE [AxonifyIntegration] SET COMPATIBILITY_LEVEL = 100

GO

--Syntax Error: Incorrect syntax near COMPATIBILITY_LEVEL.
--ALTER DATABASE [AxonifyIntegration] SET COMPATIBILITY_LEVEL = 100

GO

ALTER DATABASE [AxonifyIntegration] SET HONOR_BROKER_PRIORITY OFF 

GO

--Syntax Error: Incorrect syntax near HONOR_BROKER_PRIORITY.
--ALTER DATABASE [AxonifyIntegration] SET HONOR_BROKER_PRIORITY OFF 

GO

-- =============================================
-- Create date: 2018 April 06
-- Description:	Get all users that must to be sent to axonify to be created or updated
-- Example:	execute axf_usp_InterfaceGetPendingUsers
-- =============================================
CREATE PROCEDURE [dbo].[axf_usp_InterfaceGetPendingUsers]
AS
BEGIN
	DECLARE @LasModifiedDate DATETIME = (SELECT TOP 1 LastExecutionDate FROM axf_Interfaces WHERE InterfaceId = 'AxonifyUsers')

	--Table0: Users
	select distinct
		employeeId = t.talentid,
		active = CAST(
			case t.[status]
				when 'Active' then 1
				else 0
			end as bit
		),
		fullName = u.firstname + ' ' + u.lastname,
		--nickName = t.displayname,
		nickName = '',
		username = bos.email,
		--md5Password = convert(varchar(32),hashbytes('md5',bos.password)),
		md5Password = convert(varchar(32),hashbytes('MD5',bos.password),2),
		--userType = (
		--	select top 1 rolename 
		--	from [connect].dbo.vwuserrole 
		--	where u.userid = ur.userid
		--),--for now just the top 1 until we get an answer
		userType = 'Admin',
		email = m.email,
		--[language] = upper(
		--	case
		--		when isnull(t.languagesspoken,'') like '%,%' then left(isnull(t.languagesspoken,''), charindex(',', isnull(t.languagesspoken,'')) - 1)
		--		else isnull(t.languagesspoken,'')
		--	end
		--),
		[lenguaje] = '',
		hireDate = convert(varchar(10), t.dateofhire, 112), --Format 'YYYYMMDD'
		jobTitle = (
			select TOP 1 ssk.skillname 
			from wrsbos.dbo.skill ssk
			join wrsbos.dbo.talentskill stsk
				on ssk.skillid = stsk.skillid and stsk.talentid=t.talentid
		),--for now sending the first skill until we get an answer
		department = '',
		team = vt.territory,
		--lineOfBusiness = s.businessunitcode,
		lineOfBusiness = '',		
		suspended = case 
			when t.[status] in ('Terminated', 'Inactive') then 1
			else 0
		end
	from [wrsbos].dbo.talent t 
	join [connect].dbo.aspnet_users u on t.userid = u.userid
	join [connect].dbo.aspnet_membership m on m.userid = u.userid
	join [wrsbos].dbo.talentskill ts on ts.talentid = t.talentid
	join [wrsbos].dbo.skill s on s.skillid = ts.skillid
	join [wrsbos].dbo.talentterm tt on tt.talentid = t.talentid
	join [connect].dbo.vwuserrole ur on u.userid = ur.userid
	join [connect].dbo.aspnet_roles apr on apr.roleid = ur.roleid
	join [connect].[dbo].[vBOSUSER] bos on bos.mcid = t.talentid	
	join wrsbos.dbo.vtalent vt on vt.talentid=t.talentid
	where (@LasModifiedDate is null or t.modifieddate >= @LasModifiedDate)
	or t.talentid = 107

	--area of intereset now are the classes talents take
	----Table1: Areas Of Interest
	--SELECT distinct employeeId = t.talentid, 
	--		areaOfInterest = LEFT (qs.name, 60)
	--FROM  [winstontraining].[dbo].[QUIZRESULT] qr 
	--join [winstontraining].[dbo].[QUESTIONSET] qs ON qr.questionsetid=qs.questionsetid
	--join [wrsbos].dbo.talent t ON t.talentid=qr.mcid
	--WHERE @LasModifiedDate is null or t.modifieddate >= @LasModifiedDate and qr.status='Assigned'


	-- in this script the area of interests are the brands the talnets have assigned
	----Table1: Areas Of Interest
	select 
		employeeId = t.talentid, 
		areaOfInterest = b.brandname
	FROM [wrsbos].[dbo].[vTalentBrand] b
	join [wrsbos].dbo.talent t
		on b.talentid = t.talentid and b.IsBrandActive = 1 and b.IsActive = 1
	where (@LasModifiedDate is null or t.modifieddate >= @LasModifiedDate)
	or t.talentid = 107
END


GO

--Syntax Error: Incorrect syntax near =.
---- =============================================
---- Create date: 2018 April 06
---- Description:	Get all users that must to be sent to axonify to be created or updated
---- Example:	execute axf_usp_InterfaceGetPendingUsers
---- =============================================
--CREATE PROCEDURE [dbo].[axf_usp_InterfaceGetPendingUsers]
--AS
--BEGIN
--	DECLARE @LasModifiedDate DATETIME = (SELECT TOP 1 LastExecutionDate FROM axf_Interfaces WHERE InterfaceId = 'AxonifyUsers')
--
--	--Table0: Users
--	select distinct
--		employeeId = t.talentid,
--		active = CAST(
--			case t.[status]
--				when 'Active' then 1
--				else 0
--			end as bit
--		),
--		fullName = u.firstname + ' ' + u.lastname,
--		--nickName = t.displayname,
--		nickName = '',
--		username = bos.email,
--		--md5Password = convert(varchar(32),hashbytes('md5',bos.password)),
--		md5Password = convert(varchar(32),hashbytes('MD5',bos.password),2),
--		--userType = (
--		--	select top 1 rolename 
--		--	from [connect].dbo.vwuserrole 
--		--	where u.userid = ur.userid
--		--),--for now just the top 1 until we get an answer
--		userType = 'Admin',
--		email = m.email,
--		--[language] = upper(
--		--	case
--		--		when isnull(t.languagesspoken,'') like '%,%' then left(isnull(t.languagesspoken,''), charindex(',', isnull(t.languagesspoken,'')) - 1)
--		--		else isnull(t.languagesspoken,'')
--		--	end
--		--),
--		[lenguaje] = '',
--		hireDate = convert(varchar(10), t.dateofhire, 112), --Format 'YYYYMMDD'
--		jobTitle = (
--			select TOP 1 ssk.skillname 
--			from wrsbos.dbo.skill ssk
--			join wrsbos.dbo.talentskill stsk
--				on ssk.skillid = stsk.skillid and stsk.talentid=t.talentid
--		),--for now sending the first skill until we get an answer
--		department = '',
--		team = vt.territory,
--		--lineOfBusiness = s.businessunitcode,
--		lineOfBusiness = '',		
--		suspended = case 
--			when t.[status] in ('Terminated', 'Inactive') then 1
--			else 0
--		end
--	from [wrsbos].dbo.talent t 
--	join [connect].dbo.aspnet_users u on t.userid = u.userid
--	join [connect].dbo.aspnet_membership m on m.userid = u.userid
--	join [wrsbos].dbo.talentskill ts on ts.talentid = t.talentid
--	join [wrsbos].dbo.skill s on s.skillid = ts.skillid
--	join [wrsbos].dbo.talentterm tt on tt.talentid = t.talentid
--	join [connect].dbo.vwuserrole ur on u.userid = ur.userid
--	join [connect].dbo.aspnet_roles apr on apr.roleid = ur.roleid
--	join [connect].[dbo].[vBOSUSER] bos on bos.mcid = t.talentid	
--	join wrsbos.dbo.vtalent vt on vt.talentid=t.talentid
--	where (@LasModifiedDate is null or t.modifieddate >= @LasModifiedDate)
--	or t.talentid = 107
--
--	--area of intereset now are the classes talents take
--	----Table1: Areas Of Interest
--	--SELECT distinct employeeId = t.talentid, 
--	--		areaOfInterest = LEFT (qs.name, 60)
--	--FROM  [winstontraining].[dbo].[QUIZRESULT] qr 
--	--join [winstontraining].[dbo].[QUESTIONSET] qs ON qr.questionsetid=qs.questionsetid
--	--join [wrsbos].dbo.talent t ON t.talentid=qr.mcid
--	--WHERE @LasModifiedDate is null or t.modifieddate >= @LasModifiedDate and qr.status='Assigned'
--
--
--	-- in this script the area of interests are the brands the talnets have assigned
--	----Table1: Areas Of Interest
--	select 
--		employeeId = t.talentid, 
--		areaOfInterest = b.brandname
--	FROM [wrsbos].[dbo].[vTalentBrand] b
--	join [wrsbos].dbo.talent t
--		on b.talentid = t.talentid and b.IsBrandActive = 1 and b.IsActive = 1
--	where (@LasModifiedDate is null or t.modifieddate >= @LasModifiedDate)
--	or t.talentid = 107
--END
--

GO

-- =============================================
-- Create date: 2018-04-12
-- Description:	Update all topic graduations obtained by axonify into BOS system
/* Example:	EXECUTE axf_usp_InterfaceUpdateTopicGraduations @TopicGraduationsXML = '<ArrayOfTopicGraduation xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
	  <TopicGraduation>
		<employeeId>4</employeeId>
		<graduationTimestamp>2018-05-26 11:00:53.610</graduationTimestamp>
		<baselineAverage>60</baselineAverage>
		<baselineCorrectAnswerCount>12</baselineCorrectAnswerCount>
		<baselineAnswerCount>20</baselineAnswerCount>
		<currentAverage>80</currentAverage>
		<currentCorrectAnswerCount>32</currentCorrectAnswerCount>
		<currentAnswerCount>40</currentAnswerCount>
		<overallAverage>75</overallAverage>
		<overallCorrectAnswerCount>60</overallCorrectAnswerCount>
		<overallAnswerCount>80</overallAnswerCount>
		<timeSpent>240</timeSpent>
		<topicDetails>
		  <categoryName>brand01</categoryName>
		  <categoryExternalId>brand01</categoryExternalId>
		  <subjectName>Levi''s Premium test</subjectName>
		  <subjectExternalId>524</subjectExternalId>
		  <topicName>Levi''s Premium - Nordstrom Tips and Best Practices test</topicName>
		  <topicExternalId>3365</topicExternalId>
		  <level>1</level>
		</topicDetails>
	  </TopicGraduation>
	</ArrayOfTopicGraduation>
	'
*/
-- =============================================
CREATE PROCEDURE [dbo].[axf_usp_InterfaceUpdateTopicGraduations]
	@TopicGraduationsXML XML --XML of TopicGraduation class
AS
BEGIN
	IF @TopicGraduationsXML IS NOT NULL
	BEGIN
		DECLARE @TblTopicGraduations TABLE(
			employeeId VARCHAR(255),
			graduationTimestamp VARCHAR(100), --ISO 8601 Format YYYYMMDDTHH:mm ±hh:mm
			baselineAverage FLOAT,
			baselineCorrectAnswerCount INT,
			baselineAnswerCount INT,
			currentAverage FLOAT,
			currentCorrectAnswerCount INT,
			currentAnswerCount INT,
			overallAverage FLOAT,
			overallCorrectAnswerCount INT,
			timeSpent INT,

			categoryName VARCHAR(60),
			categoryExternalId VARCHAR(60),
			subjectName VARCHAR(60),
			subjectExternalId VARCHAR(60),
			topicName VARCHAR(60),
			topicExternalId VARCHAR(60),
			[level] INT
		)

		INSERT INTO @TblTopicGraduations
		SELECT  
			employeeId = Tbl.Col.value('(employeeId/text())[1]', 'varchar(60)'),
			graduationTimestamp = Tbl.Col.value('(graduationTimestamp/text())[1]', 'varchar(100)'),
			baselineAverage = Tbl.Col.value('(baselineAverage/text())[1]', 'float'),
			baselineCorrectAnswerCount = Tbl.Col.value('(baselineCorrectAnswerCount/text())[1]', 'int'),
			baselineAnswerCount = Tbl.Col.value('(baselineAnswerCount/text())[1]', 'int'),
			currentAverage = Tbl.Col.value('(currentAverage/text())[1]', 'float'),
			currentCorrectAnswerCount = Tbl.Col.value('(currentCorrectAnswerCount/text())[1]', 'int'),
			currentAnswerCount = Tbl.Col.value('(currentAnswerCount/text())[1]', 'int'),
			overallAverage = Tbl.Col.value('(overallAverage/text())[1]', 'float'),
			overallCorrectAnswerCount = Tbl.Col.value('(overallCorrectAnswerCount/text())[1]', 'int'),
			timeSpent = Tbl.Col.value('(timeSpent/text())[1]', 'int'),
			
			categoryName = Tbl.Col.value('(topicDetails/categoryName/text())[1]', 'varchar(60)'),
			categoryExternalId = Tbl.Col.value('(topicDetails/categoryExternalId/text())[1]', 'varchar(60)'),
			subjectName = Tbl.Col.value('(topicDetails/subjectName/text())[1]', 'varchar(60)'),
			subjectExternalId = Tbl.Col.value('(topicDetails/subjectExternalId/text())[1]', 'varchar(60)'),
			topicName = Tbl.Col.value('(topicDetails/topicName/text())[1]', 'varchar(60)'),
			topicExternalId = Tbl.Col.value('(topicDetails/topicExternalId/text())[1]', 'varchar(60)'),
			[level] = Tbl.Col.value('(topicDetails/level/text())[1]', 'int')
		FROM @TopicGraduationsXML.nodes('//ArrayOfTopicGraduation/TopicGraduation') Tbl(Col)
		OPTION (OPTIMIZE FOR ( @TopicGraduationsXML = NULL ))
		
		MERGE INTO [winstontraining].[dbo].[QUIZRESULT] WITH (HOLDLOCK) AS quizresult
		USING @TblTopicGraduations AS axonifytopic
		ON quizresult.MCID = axonifytopic.employeeId AND quizresult.questionsetid = axonifytopic.topicExternalId
		WHEN MATCHED THEN --update
		UPDATE SET 
		 quizresult.status = (case when axonifytopic.graduationTimestamp is NULL then 'Assigned' else 'Completed' end),
		 quizresult.grade = (case when axonifytopic.graduationTimestamp is NULL then 'Pass' else 'Pass' end),
		 quizresult.datetaken = axonifytopic.graduationTimestamp 
		WHEN NOT MATCHED BY TARGET THEN --insert
		INSERT VALUES((select top 1 questionsetid from  [winstontraining].[dbo].[QUESTIONSET] where questionsetid = axonifytopic.topicExternalId),getdate(),axonifytopic.graduationTimestamp,'Assigned',NULL,NULL,NULL,NULL,0,NULL,NULL,getdate(),(getdate() + 5),axonifytopic.employeeid,NULL);

		
		--MERGE INTO [winstontraining].[dbo].[QUIZRESULT] WITH (HOLDLOCK) AS quizresult
		--USING @TblTopicGraduations AS axonifytopic
		--ON quizresult.MCID = axonifytopic.employeeId AND quizresult.QuestionSetId = axonifytopic.subjectExternalId
		--WHEN MATCHED THEN --update
		--UPDATE SET 
		-- quizresult.status = (case axonifytopic.graduationTimestamp when 'NULL' then 'Assigned' else 'NULL' end),
		-- quizresult.grade = (case axonifytopic.graduationTimestamp when 'NULL' then 'Pass' else 'NULL' end),
		-- quizresult.datetaken = axonifytopic.graduationTimestamp 
		--WHEN NOT MATCHED BY TARGET THEN --insert
		--INSERT VALUES(8,getdate(),axonifytopic.graduationTimestamp,'Assigned',NULL,NULL,NULL,NULL,0,NULL,NULL,getdate(),(getdate() + 5),axonifytopic.employeeid,NULL);
	END
END 

GO

--Syntax Error: Incorrect syntax near MERGE.
---- =============================================
---- Create date: 2018-04-12
---- Description:	Update all topic graduations obtained by axonify into BOS system
--/* Example:	EXECUTE axf_usp_InterfaceUpdateTopicGraduations @TopicGraduationsXML = '<ArrayOfTopicGraduation xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
--	  <TopicGraduation>
--		<employeeId>4</employeeId>
--		<graduationTimestamp>2018-05-26 11:00:53.610</graduationTimestamp>
--		<baselineAverage>60</baselineAverage>
--		<baselineCorrectAnswerCount>12</baselineCorrectAnswerCount>
--		<baselineAnswerCount>20</baselineAnswerCount>
--		<currentAverage>80</currentAverage>
--		<currentCorrectAnswerCount>32</currentCorrectAnswerCount>
--		<currentAnswerCount>40</currentAnswerCount>
--		<overallAverage>75</overallAverage>
--		<overallCorrectAnswerCount>60</overallCorrectAnswerCount>
--		<overallAnswerCount>80</overallAnswerCount>
--		<timeSpent>240</timeSpent>
--		<topicDetails>
--		  <categoryName>brand01</categoryName>
--		  <categoryExternalId>brand01</categoryExternalId>
--		  <subjectName>Levi''s Premium test</subjectName>
--		  <subjectExternalId>524</subjectExternalId>
--		  <topicName>Levi''s Premium - Nordstrom Tips and Best Practices test</topicName>
--		  <topicExternalId>3365</topicExternalId>
--		  <level>1</level>
--		</topicDetails>
--	  </TopicGraduation>
--	</ArrayOfTopicGraduation>
--	'
--*/
---- =============================================
--CREATE PROCEDURE [dbo].[axf_usp_InterfaceUpdateTopicGraduations]
--	@TopicGraduationsXML XML --XML of TopicGraduation class
--AS
--BEGIN
--	IF @TopicGraduationsXML IS NOT NULL
--	BEGIN
--		DECLARE @TblTopicGraduations TABLE(
--			employeeId VARCHAR(255),
--			graduationTimestamp VARCHAR(100), --ISO 8601 Format YYYYMMDDTHH:mm ±hh:mm
--			baselineAverage FLOAT,
--			baselineCorrectAnswerCount INT,
--			baselineAnswerCount INT,
--			currentAverage FLOAT,
--			currentCorrectAnswerCount INT,
--			currentAnswerCount INT,
--			overallAverage FLOAT,
--			overallCorrectAnswerCount INT,
--			timeSpent INT,
--
--			categoryName VARCHAR(60),
--			categoryExternalId VARCHAR(60),
--			subjectName VARCHAR(60),
--			subjectExternalId VARCHAR(60),
--			topicName VARCHAR(60),
--			topicExternalId VARCHAR(60),
--			[level] INT
--		)
--
--		INSERT INTO @TblTopicGraduations
--		SELECT  
--			employeeId = Tbl.Col.value('(employeeId/text())[1]', 'varchar(60)'),
--			graduationTimestamp = Tbl.Col.value('(graduationTimestamp/text())[1]', 'varchar(100)'),
--			baselineAverage = Tbl.Col.value('(baselineAverage/text())[1]', 'float'),
--			baselineCorrectAnswerCount = Tbl.Col.value('(baselineCorrectAnswerCount/text())[1]', 'int'),
--			baselineAnswerCount = Tbl.Col.value('(baselineAnswerCount/text())[1]', 'int'),
--			currentAverage = Tbl.Col.value('(currentAverage/text())[1]', 'float'),
--			currentCorrectAnswerCount = Tbl.Col.value('(currentCorrectAnswerCount/text())[1]', 'int'),
--			currentAnswerCount = Tbl.Col.value('(currentAnswerCount/text())[1]', 'int'),
--			overallAverage = Tbl.Col.value('(overallAverage/text())[1]', 'float'),
--			overallCorrectAnswerCount = Tbl.Col.value('(overallCorrectAnswerCount/text())[1]', 'int'),
--			timeSpent = Tbl.Col.value('(timeSpent/text())[1]', 'int'),
--			
--			categoryName = Tbl.Col.value('(topicDetails/categoryName/text())[1]', 'varchar(60)'),
--			categoryExternalId = Tbl.Col.value('(topicDetails/categoryExternalId/text())[1]', 'varchar(60)'),
--			subjectName = Tbl.Col.value('(topicDetails/subjectName/text())[1]', 'varchar(60)'),
--			subjectExternalId = Tbl.Col.value('(topicDetails/subjectExternalId/text())[1]', 'varchar(60)'),
--			topicName = Tbl.Col.value('(topicDetails/topicName/text())[1]', 'varchar(60)'),
--			topicExternalId = Tbl.Col.value('(topicDetails/topicExternalId/text())[1]', 'varchar(60)'),
--			[level] = Tbl.Col.value('(topicDetails/level/text())[1]', 'int')
--		FROM @TopicGraduationsXML.nodes('//ArrayOfTopicGraduation/TopicGraduation') Tbl(Col)
--		OPTION (OPTIMIZE FOR ( @TopicGraduationsXML = NULL ))
--		
--		MERGE INTO [winstontraining].[dbo].[QUIZRESULT] WITH (HOLDLOCK) AS quizresult
--		USING @TblTopicGraduations AS axonifytopic
--		ON quizresult.MCID = axonifytopic.employeeId AND quizresult.questionsetid = axonifytopic.topicExternalId
--		WHEN MATCHED THEN --update
--		UPDATE SET 
--		 quizresult.status = (case when axonifytopic.graduationTimestamp is NULL then 'Assigned' else 'Completed' end),
--		 quizresult.grade = (case when axonifytopic.graduationTimestamp is NULL then 'Pass' else 'Pass' end),
--		 quizresult.datetaken = axonifytopic.graduationTimestamp 
--		WHEN NOT MATCHED BY TARGET THEN --insert
--		INSERT VALUES((select top 1 questionsetid from  [winstontraining].[dbo].[QUESTIONSET] where questionsetid = axonifytopic.topicExternalId),getdate(),axonifytopic.graduationTimestamp,'Assigned',NULL,NULL,NULL,NULL,0,NULL,NULL,getdate(),(getdate() + 5),axonifytopic.employeeid,NULL);
--
--		
--		--MERGE INTO [winstontraining].[dbo].[QUIZRESULT] WITH (HOLDLOCK) AS quizresult
--		--USING @TblTopicGraduations AS axonifytopic
--		--ON quizresult.MCID = axonifytopic.employeeId AND quizresult.QuestionSetId = axonifytopic.subjectExternalId
--		--WHEN MATCHED THEN --update
--		--UPDATE SET 
--		-- quizresult.status = (case axonifytopic.graduationTimestamp when 'NULL' then 'Assigned' else 'NULL' end),
--		-- quizresult.grade = (case axonifytopic.graduationTimestamp when 'NULL' then 'Pass' else 'NULL' end),
--		-- quizresult.datetaken = axonifytopic.graduationTimestamp 
--		--WHEN NOT MATCHED BY TARGET THEN --insert
--		--INSERT VALUES(8,getdate(),axonifytopic.graduationTimestamp,'Assigned',NULL,NULL,NULL,NULL,0,NULL,NULL,getdate(),(getdate() + 5),axonifytopic.employeeid,NULL);
--	END
--END 



GO

USE [master]
GO

/****** Object:  Database [AxonifyIntegration]    Script Date: 24/08/2018 06:30:45 p. m. ******/
CREATE DATABASE [AxonifyIntegration] ON  PRIMARY 
( NAME = N'AxonifyIntegration', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL10_50.MSSQLSERVER\MSSQL\DATA\AxonifyIntegration.mdf' , SIZE = 3072KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'AxonifyIntegration_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL10_50.MSSQLSERVER\MSSQL\DATA\AxonifyIntegration_log.ldf' , SIZE = 1280KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO

IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [AxonifyIntegration].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO

ALTER DATABASE [AxonifyIntegration] SET ANSI_NULL_DEFAULT OFF
GO

ALTER DATABASE [AxonifyIntegration] SET ANSI_NULLS OFF
GO

ALTER DATABASE [AxonifyIntegration] SET ANSI_PADDING OFF
GO

ALTER DATABASE [AxonifyIntegration] SET ANSI_WARNINGS OFF
GO

ALTER DATABASE [AxonifyIntegration] SET ARITHABORT OFF
GO

ALTER DATABASE [AxonifyIntegration] SET AUTO_CLOSE OFF
GO

ALTER DATABASE [AxonifyIntegration] SET AUTO_CREATE_STATISTICS ON
GO

ALTER DATABASE [AxonifyIntegration] SET AUTO_SHRINK OFF
GO

ALTER DATABASE [AxonifyIntegration] SET AUTO_UPDATE_STATISTICS ON
GO

ALTER DATABASE [AxonifyIntegration] SET CURSOR_CLOSE_ON_COMMIT OFF
GO

ALTER DATABASE [AxonifyIntegration] SET CURSOR_DEFAULT  GLOBAL
GO

ALTER DATABASE [AxonifyIntegration] SET CONCAT_NULL_YIELDS_NULL OFF
GO

ALTER DATABASE [AxonifyIntegration] SET NUMERIC_ROUNDABORT OFF
GO

ALTER DATABASE [AxonifyIntegration] SET QUOTED_IDENTIFIER OFF
GO

ALTER DATABASE [AxonifyIntegration] SET RECURSIVE_TRIGGERS OFF
GO

ALTER DATABASE [AxonifyIntegration] SET  DISABLE_BROKER
GO

ALTER DATABASE [AxonifyIntegration] SET AUTO_UPDATE_STATISTICS_ASYNC OFF
GO

ALTER DATABASE [AxonifyIntegration] SET DATE_CORRELATION_OPTIMIZATION OFF
GO

ALTER DATABASE [AxonifyIntegration] SET TRUSTWORTHY OFF
GO

ALTER DATABASE [AxonifyIntegration] SET ALLOW_SNAPSHOT_ISOLATION OFF
GO

ALTER DATABASE [AxonifyIntegration] SET PARAMETERIZATION SIMPLE
GO

ALTER DATABASE [AxonifyIntegration] SET READ_COMMITTED_SNAPSHOT OFF
GO

ALTER DATABASE [AxonifyIntegration] SET RECOVERY FULL
GO

ALTER DATABASE [AxonifyIntegration] SET  MULTI_USER
GO

ALTER DATABASE [AxonifyIntegration] SET PAGE_VERIFY CHECKSUM
GO

ALTER DATABASE [AxonifyIntegration] SET DB_CHAINING OFF
GO

EXEC sys.sp_db_vardecimal_storage_format N'AxonifyIntegration', N'ON'
GO

USE [AxonifyIntegration]
GO

/****** Object:  StoredProcedure [dbo].[axf_usp_InterfaceGetPendingUsers]    Script Date: 24/08/2018 06:30:51 p. m. ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/****** Object:  StoredProcedure [dbo].[axf_usp_InterfaceHistoryUpdate]    Script Date: 24/08/2018 06:30:51 p. m. ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/****** Object:  StoredProcedure [dbo].[axf_usp_InterfaceUpdateTopicGraduations]    Script Date: 24/08/2018 06:30:51 p. m. ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/****** Object:  Table [dbo].[axf_Interfaces]    Script Date: 24/08/2018 06:30:51 p. m. ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

SET ANSI_PADDING OFF
GO

/****** Object:  Table [dbo].[axf_Interfaces_HistoryCalls]    Script Date: 24/08/2018 06:30:51 p. m. ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

SET ANSI_PADDING OFF
GO

USE [master]
GO

ALTER DATABASE [AxonifyIntegration] SET  READ_WRITE
GO

ALTER DATABASE [AxonifyIntegration] SET COMPATIBILITY_LEVEL = 100

GO

--Syntax Error: Incorrect syntax near COMPATIBILITY_LEVEL.
--ALTER DATABASE [AxonifyIntegration] SET COMPATIBILITY_LEVEL = 100

GO

ALTER DATABASE [AxonifyIntegration] SET HONOR_BROKER_PRIORITY OFF 

GO

--Syntax Error: Incorrect syntax near HONOR_BROKER_PRIORITY.
--ALTER DATABASE [AxonifyIntegration] SET HONOR_BROKER_PRIORITY OFF 

GO

-- =============================================
-- Create date: 2018 April 06
-- Description:	Get all users that must to be sent to axonify to be created or updated
-- Example:	execute axf_usp_InterfaceGetPendingUsers
-- =============================================
CREATE PROCEDURE [dbo].[axf_usp_InterfaceGetPendingUsers]
AS
BEGIN
	DECLARE @LasModifiedDate DATETIME = (SELECT TOP 1 LastExecutionDate FROM axf_Interfaces WHERE InterfaceId = 'AxonifyUsers')

	--Table0: Users
	select distinct
		employeeId = t.talentid,
		active = CAST(
			case t.[status]
				when 'Active' then 1
				else 0
			end as bit
		),
		fullName = u.firstname + ' ' + u.lastname,
		--nickName = t.displayname,
		nickName = '',
		username = bos.email,
		--md5Password = convert(varchar(32),hashbytes('md5',bos.password)),
		md5Password = convert(varchar(32),hashbytes('MD5',bos.password),2),
		--userType = (
		--	select top 1 rolename 
		--	from [connect].dbo.vwuserrole 
		--	where u.userid = ur.userid
		--),--for now just the top 1 until we get an answer
		userType = 'Admin',
		email = m.email,
		--[language] = upper(
		--	case
		--		when isnull(t.languagesspoken,'') like '%,%' then left(isnull(t.languagesspoken,''), charindex(',', isnull(t.languagesspoken,'')) - 1)
		--		else isnull(t.languagesspoken,'')
		--	end
		--),
		[lenguaje] = '',
		hireDate = convert(varchar(10), t.dateofhire, 112), --Format 'YYYYMMDD'
		jobTitle = (
			select TOP 1 ssk.skillname 
			from wrsbos.dbo.skill ssk
			join wrsbos.dbo.talentskill stsk
				on ssk.skillid = stsk.skillid and stsk.talentid=t.talentid
		),--for now sending the first skill until we get an answer
		department = '',
		team = vt.territory,
		--lineOfBusiness = s.businessunitcode,
		lineOfBusiness = '',		
		suspended = case 
			when t.[status] in ('Terminated', 'Inactive') then 1
			else 0
		end
	from [wrsbos].dbo.talent t 
	join [connect].dbo.aspnet_users u on t.userid = u.userid
	join [connect].dbo.aspnet_membership m on m.userid = u.userid
	join [wrsbos].dbo.talentskill ts on ts.talentid = t.talentid
	join [wrsbos].dbo.skill s on s.skillid = ts.skillid
	join [wrsbos].dbo.talentterm tt on tt.talentid = t.talentid
	join [connect].dbo.vwuserrole ur on u.userid = ur.userid
	join [connect].dbo.aspnet_roles apr on apr.roleid = ur.roleid
	join [connect].[dbo].[vBOSUSER] bos on bos.mcid = t.talentid	
	join wrsbos.dbo.vtalent vt on vt.talentid=t.talentid
	where (@LasModifiedDate is null or t.modifieddate >= @LasModifiedDate)
	or t.talentid = 107

	--area of intereset now are the classes talents take
	----Table1: Areas Of Interest
	--SELECT distinct employeeId = t.talentid, 
	--		areaOfInterest = LEFT (qs.name, 60)
	--FROM  [winstontraining].[dbo].[QUIZRESULT] qr 
	--join [winstontraining].[dbo].[QUESTIONSET] qs ON qr.questionsetid=qs.questionsetid
	--join [wrsbos].dbo.talent t ON t.talentid=qr.mcid
	--WHERE @LasModifiedDate is null or t.modifieddate >= @LasModifiedDate and qr.status='Assigned'


	-- in this script the area of interests are the brands the talnets have assigned
	----Table1: Areas Of Interest
	select 
		employeeId = t.talentid, 
		areaOfInterest = b.brandname
	FROM [wrsbos].[dbo].[vTalentBrand] b
	join [wrsbos].dbo.talent t
		on b.talentid = t.talentid and b.IsBrandActive = 1 and b.IsActive = 1
	where (@LasModifiedDate is null or t.modifieddate >= @LasModifiedDate)
	or t.talentid = 107
END


GO

--Syntax Error: Incorrect syntax near =.
---- =============================================
---- Create date: 2018 April 06
---- Description:	Get all users that must to be sent to axonify to be created or updated
---- Example:	execute axf_usp_InterfaceGetPendingUsers
---- =============================================
--CREATE PROCEDURE [dbo].[axf_usp_InterfaceGetPendingUsers]
--AS
--BEGIN
--	DECLARE @LasModifiedDate DATETIME = (SELECT TOP 1 LastExecutionDate FROM axf_Interfaces WHERE InterfaceId = 'AxonifyUsers')
--
--	--Table0: Users
--	select distinct
--		employeeId = t.talentid,
--		active = CAST(
--			case t.[status]
--				when 'Active' then 1
--				else 0
--			end as bit
--		),
--		fullName = u.firstname + ' ' + u.lastname,
--		--nickName = t.displayname,
--		nickName = '',
--		username = bos.email,
--		--md5Password = convert(varchar(32),hashbytes('md5',bos.password)),
--		md5Password = convert(varchar(32),hashbytes('MD5',bos.password),2),
--		--userType = (
--		--	select top 1 rolename 
--		--	from [connect].dbo.vwuserrole 
--		--	where u.userid = ur.userid
--		--),--for now just the top 1 until we get an answer
--		userType = 'Admin',
--		email = m.email,
--		--[language] = upper(
--		--	case
--		--		when isnull(t.languagesspoken,'') like '%,%' then left(isnull(t.languagesspoken,''), charindex(',', isnull(t.languagesspoken,'')) - 1)
--		--		else isnull(t.languagesspoken,'')
--		--	end
--		--),
--		[lenguaje] = '',
--		hireDate = convert(varchar(10), t.dateofhire, 112), --Format 'YYYYMMDD'
--		jobTitle = (
--			select TOP 1 ssk.skillname 
--			from wrsbos.dbo.skill ssk
--			join wrsbos.dbo.talentskill stsk
--				on ssk.skillid = stsk.skillid and stsk.talentid=t.talentid
--		),--for now sending the first skill until we get an answer
--		department = '',
--		team = vt.territory,
--		--lineOfBusiness = s.businessunitcode,
--		lineOfBusiness = '',		
--		suspended = case 
--			when t.[status] in ('Terminated', 'Inactive') then 1
--			else 0
--		end
--	from [wrsbos].dbo.talent t 
--	join [connect].dbo.aspnet_users u on t.userid = u.userid
--	join [connect].dbo.aspnet_membership m on m.userid = u.userid
--	join [wrsbos].dbo.talentskill ts on ts.talentid = t.talentid
--	join [wrsbos].dbo.skill s on s.skillid = ts.skillid
--	join [wrsbos].dbo.talentterm tt on tt.talentid = t.talentid
--	join [connect].dbo.vwuserrole ur on u.userid = ur.userid
--	join [connect].dbo.aspnet_roles apr on apr.roleid = ur.roleid
--	join [connect].[dbo].[vBOSUSER] bos on bos.mcid = t.talentid	
--	join wrsbos.dbo.vtalent vt on vt.talentid=t.talentid
--	where (@LasModifiedDate is null or t.modifieddate >= @LasModifiedDate)
--	or t.talentid = 107
--
--	--area of intereset now are the classes talents take
--	----Table1: Areas Of Interest
--	--SELECT distinct employeeId = t.talentid, 
--	--		areaOfInterest = LEFT (qs.name, 60)
--	--FROM  [winstontraining].[dbo].[QUIZRESULT] qr 
--	--join [winstontraining].[dbo].[QUESTIONSET] qs ON qr.questionsetid=qs.questionsetid
--	--join [wrsbos].dbo.talent t ON t.talentid=qr.mcid
--	--WHERE @LasModifiedDate is null or t.modifieddate >= @LasModifiedDate and qr.status='Assigned'
--
--
--	-- in this script the area of interests are the brands the talnets have assigned
--	----Table1: Areas Of Interest
--	select 
--		employeeId = t.talentid, 
--		areaOfInterest = b.brandname
--	FROM [wrsbos].[dbo].[vTalentBrand] b
--	join [wrsbos].dbo.talent t
--		on b.talentid = t.talentid and b.IsBrandActive = 1 and b.IsActive = 1
--	where (@LasModifiedDate is null or t.modifieddate >= @LasModifiedDate)
--	or t.talentid = 107
--END
--

GO

-- =============================================
-- Create date: 2018-04-12
-- Description:	Update all topic graduations obtained by axonify into BOS system
/* Example:	EXECUTE axf_usp_InterfaceUpdateTopicGraduations @TopicGraduationsXML = '<ArrayOfTopicGraduation xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
	  <TopicGraduation>
		<employeeId>4</employeeId>
		<graduationTimestamp>2018-05-26 11:00:53.610</graduationTimestamp>
		<baselineAverage>60</baselineAverage>
		<baselineCorrectAnswerCount>12</baselineCorrectAnswerCount>
		<baselineAnswerCount>20</baselineAnswerCount>
		<currentAverage>80</currentAverage>
		<currentCorrectAnswerCount>32</currentCorrectAnswerCount>
		<currentAnswerCount>40</currentAnswerCount>
		<overallAverage>75</overallAverage>
		<overallCorrectAnswerCount>60</overallCorrectAnswerCount>
		<overallAnswerCount>80</overallAnswerCount>
		<timeSpent>240</timeSpent>
		<topicDetails>
		  <categoryName>brand01</categoryName>
		  <categoryExternalId>brand01</categoryExternalId>
		  <subjectName>Levi''s Premium test</subjectName>
		  <subjectExternalId>524</subjectExternalId>
		  <topicName>Levi''s Premium - Nordstrom Tips and Best Practices test</topicName>
		  <topicExternalId>3365</topicExternalId>
		  <level>1</level>
		</topicDetails>
	  </TopicGraduation>
	</ArrayOfTopicGraduation>
	'
*/
-- =============================================
CREATE PROCEDURE [dbo].[axf_usp_InterfaceUpdateTopicGraduations]
	@TopicGraduationsXML XML --XML of TopicGraduation class
AS
BEGIN
	IF @TopicGraduationsXML IS NOT NULL
	BEGIN
		DECLARE @TblTopicGraduations TABLE(
			employeeId VARCHAR(255),
			graduationTimestamp VARCHAR(100), --ISO 8601 Format YYYYMMDDTHH:mm ±hh:mm
			baselineAverage FLOAT,
			baselineCorrectAnswerCount INT,
			baselineAnswerCount INT,
			currentAverage FLOAT,
			currentCorrectAnswerCount INT,
			currentAnswerCount INT,
			overallAverage FLOAT,
			overallCorrectAnswerCount INT,
			timeSpent INT,

			categoryName VARCHAR(60),
			categoryExternalId VARCHAR(60),
			subjectName VARCHAR(60),
			subjectExternalId VARCHAR(60),
			topicName VARCHAR(60),
			topicExternalId VARCHAR(60),
			[level] INT
		)

		INSERT INTO @TblTopicGraduations
		SELECT  
			employeeId = Tbl.Col.value('(employeeId/text())[1]', 'varchar(60)'),
			graduationTimestamp = Tbl.Col.value('(graduationTimestamp/text())[1]', 'varchar(100)'),
			baselineAverage = Tbl.Col.value('(baselineAverage/text())[1]', 'float'),
			baselineCorrectAnswerCount = Tbl.Col.value('(baselineCorrectAnswerCount/text())[1]', 'int'),
			baselineAnswerCount = Tbl.Col.value('(baselineAnswerCount/text())[1]', 'int'),
			currentAverage = Tbl.Col.value('(currentAverage/text())[1]', 'float'),
			currentCorrectAnswerCount = Tbl.Col.value('(currentCorrectAnswerCount/text())[1]', 'int'),
			currentAnswerCount = Tbl.Col.value('(currentAnswerCount/text())[1]', 'int'),
			overallAverage = Tbl.Col.value('(overallAverage/text())[1]', 'float'),
			overallCorrectAnswerCount = Tbl.Col.value('(overallCorrectAnswerCount/text())[1]', 'int'),
			timeSpent = Tbl.Col.value('(timeSpent/text())[1]', 'int'),
			
			categoryName = Tbl.Col.value('(topicDetails/categoryName/text())[1]', 'varchar(60)'),
			categoryExternalId = Tbl.Col.value('(topicDetails/categoryExternalId/text())[1]', 'varchar(60)'),
			subjectName = Tbl.Col.value('(topicDetails/subjectName/text())[1]', 'varchar(60)'),
			subjectExternalId = Tbl.Col.value('(topicDetails/subjectExternalId/text())[1]', 'varchar(60)'),
			topicName = Tbl.Col.value('(topicDetails/topicName/text())[1]', 'varchar(60)'),
			topicExternalId = Tbl.Col.value('(topicDetails/topicExternalId/text())[1]', 'varchar(60)'),
			[level] = Tbl.Col.value('(topicDetails/level/text())[1]', 'int')
		FROM @TopicGraduationsXML.nodes('//ArrayOfTopicGraduation/TopicGraduation') Tbl(Col)
		OPTION (OPTIMIZE FOR ( @TopicGraduationsXML = NULL ))
		
		MERGE INTO [winstontraining].[dbo].[QUIZRESULT] WITH (HOLDLOCK) AS quizresult
		USING @TblTopicGraduations AS axonifytopic
		ON quizresult.MCID = axonifytopic.employeeId AND quizresult.questionsetid = axonifytopic.topicExternalId
		WHEN MATCHED THEN --update
		UPDATE SET 
		 quizresult.status = (case when axonifytopic.graduationTimestamp is NULL then 'Assigned' else 'Completed' end),
		 quizresult.grade = (case when axonifytopic.graduationTimestamp is NULL then 'Pass' else 'Pass' end),
		 quizresult.datetaken = axonifytopic.graduationTimestamp 
		WHEN NOT MATCHED BY TARGET THEN --insert
		INSERT VALUES((select top 1 questionsetid from  [winstontraining].[dbo].[QUESTIONSET] where questionsetid = axonifytopic.topicExternalId),getdate(),axonifytopic.graduationTimestamp,'Assigned',NULL,NULL,NULL,NULL,0,NULL,NULL,getdate(),(getdate() + 5),axonifytopic.employeeid,NULL);

		
		--MERGE INTO [winstontraining].[dbo].[QUIZRESULT] WITH (HOLDLOCK) AS quizresult
		--USING @TblTopicGraduations AS axonifytopic
		--ON quizresult.MCID = axonifytopic.employeeId AND quizresult.QuestionSetId = axonifytopic.subjectExternalId
		--WHEN MATCHED THEN --update
		--UPDATE SET 
		-- quizresult.status = (case axonifytopic.graduationTimestamp when 'NULL' then 'Assigned' else 'NULL' end),
		-- quizresult.grade = (case axonifytopic.graduationTimestamp when 'NULL' then 'Pass' else 'NULL' end),
		-- quizresult.datetaken = axonifytopic.graduationTimestamp 
		--WHEN NOT MATCHED BY TARGET THEN --insert
		--INSERT VALUES(8,getdate(),axonifytopic.graduationTimestamp,'Assigned',NULL,NULL,NULL,NULL,0,NULL,NULL,getdate(),(getdate() + 5),axonifytopic.employeeid,NULL);
	END
END 

GO

--Syntax Error: Incorrect syntax near MERGE.
---- =============================================
---- Create date: 2018-04-12
---- Description:	Update all topic graduations obtained by axonify into BOS system
--/* Example:	EXECUTE axf_usp_InterfaceUpdateTopicGraduations @TopicGraduationsXML = '<ArrayOfTopicGraduation xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
--	  <TopicGraduation>
--		<employeeId>4</employeeId>
--		<graduationTimestamp>2018-05-26 11:00:53.610</graduationTimestamp>
--		<baselineAverage>60</baselineAverage>
--		<baselineCorrectAnswerCount>12</baselineCorrectAnswerCount>
--		<baselineAnswerCount>20</baselineAnswerCount>
--		<currentAverage>80</currentAverage>
--		<currentCorrectAnswerCount>32</currentCorrectAnswerCount>
--		<currentAnswerCount>40</currentAnswerCount>
--		<overallAverage>75</overallAverage>
--		<overallCorrectAnswerCount>60</overallCorrectAnswerCount>
--		<overallAnswerCount>80</overallAnswerCount>
--		<timeSpent>240</timeSpent>
--		<topicDetails>
--		  <categoryName>brand01</categoryName>
--		  <categoryExternalId>brand01</categoryExternalId>
--		  <subjectName>Levi''s Premium test</subjectName>
--		  <subjectExternalId>524</subjectExternalId>
--		  <topicName>Levi''s Premium - Nordstrom Tips and Best Practices test</topicName>
--		  <topicExternalId>3365</topicExternalId>
--		  <level>1</level>
--		</topicDetails>
--	  </TopicGraduation>
--	</ArrayOfTopicGraduation>
--	'
--*/
---- =============================================
--CREATE PROCEDURE [dbo].[axf_usp_InterfaceUpdateTopicGraduations]
--	@TopicGraduationsXML XML --XML of TopicGraduation class
--AS
--BEGIN
--	IF @TopicGraduationsXML IS NOT NULL
--	BEGIN
--		DECLARE @TblTopicGraduations TABLE(
--			employeeId VARCHAR(255),
--			graduationTimestamp VARCHAR(100), --ISO 8601 Format YYYYMMDDTHH:mm ±hh:mm
--			baselineAverage FLOAT,
--			baselineCorrectAnswerCount INT,
--			baselineAnswerCount INT,
--			currentAverage FLOAT,
--			currentCorrectAnswerCount INT,
--			currentAnswerCount INT,
--			overallAverage FLOAT,
--			overallCorrectAnswerCount INT,
--			timeSpent INT,
--
--			categoryName VARCHAR(60),
--			categoryExternalId VARCHAR(60),
--			subjectName VARCHAR(60),
--			subjectExternalId VARCHAR(60),
--			topicName VARCHAR(60),
--			topicExternalId VARCHAR(60),
--			[level] INT
--		)
--
--		INSERT INTO @TblTopicGraduations
--		SELECT  
--			employeeId = Tbl.Col.value('(employeeId/text())[1]', 'varchar(60)'),
--			graduationTimestamp = Tbl.Col.value('(graduationTimestamp/text())[1]', 'varchar(100)'),
--			baselineAverage = Tbl.Col.value('(baselineAverage/text())[1]', 'float'),
--			baselineCorrectAnswerCount = Tbl.Col.value('(baselineCorrectAnswerCount/text())[1]', 'int'),
--			baselineAnswerCount = Tbl.Col.value('(baselineAnswerCount/text())[1]', 'int'),
--			currentAverage = Tbl.Col.value('(currentAverage/text())[1]', 'float'),
--			currentCorrectAnswerCount = Tbl.Col.value('(currentCorrectAnswerCount/text())[1]', 'int'),
--			currentAnswerCount = Tbl.Col.value('(currentAnswerCount/text())[1]', 'int'),
--			overallAverage = Tbl.Col.value('(overallAverage/text())[1]', 'float'),
--			overallCorrectAnswerCount = Tbl.Col.value('(overallCorrectAnswerCount/text())[1]', 'int'),
--			timeSpent = Tbl.Col.value('(timeSpent/text())[1]', 'int'),
--			
--			categoryName = Tbl.Col.value('(topicDetails/categoryName/text())[1]', 'varchar(60)'),
--			categoryExternalId = Tbl.Col.value('(topicDetails/categoryExternalId/text())[1]', 'varchar(60)'),
--			subjectName = Tbl.Col.value('(topicDetails/subjectName/text())[1]', 'varchar(60)'),
--			subjectExternalId = Tbl.Col.value('(topicDetails/subjectExternalId/text())[1]', 'varchar(60)'),
--			topicName = Tbl.Col.value('(topicDetails/topicName/text())[1]', 'varchar(60)'),
--			topicExternalId = Tbl.Col.value('(topicDetails/topicExternalId/text())[1]', 'varchar(60)'),
--			[level] = Tbl.Col.value('(topicDetails/level/text())[1]', 'int')
--		FROM @TopicGraduationsXML.nodes('//ArrayOfTopicGraduation/TopicGraduation') Tbl(Col)
--		OPTION (OPTIMIZE FOR ( @TopicGraduationsXML = NULL ))
--		
--		MERGE INTO [winstontraining].[dbo].[QUIZRESULT] WITH (HOLDLOCK) AS quizresult
--		USING @TblTopicGraduations AS axonifytopic
--		ON quizresult.MCID = axonifytopic.employeeId AND quizresult.questionsetid = axonifytopic.topicExternalId
--		WHEN MATCHED THEN --update
--		UPDATE SET 
--		 quizresult.status = (case when axonifytopic.graduationTimestamp is NULL then 'Assigned' else 'Completed' end),
--		 quizresult.grade = (case when axonifytopic.graduationTimestamp is NULL then 'Pass' else 'Pass' end),
--		 quizresult.datetaken = axonifytopic.graduationTimestamp 
--		WHEN NOT MATCHED BY TARGET THEN --insert
--		INSERT VALUES((select top 1 questionsetid from  [winstontraining].[dbo].[QUESTIONSET] where questionsetid = axonifytopic.topicExternalId),getdate(),axonifytopic.graduationTimestamp,'Assigned',NULL,NULL,NULL,NULL,0,NULL,NULL,getdate(),(getdate() + 5),axonifytopic.employeeid,NULL);
--
--		
--		--MERGE INTO [winstontraining].[dbo].[QUIZRESULT] WITH (HOLDLOCK) AS quizresult
--		--USING @TblTopicGraduations AS axonifytopic
--		--ON quizresult.MCID = axonifytopic.employeeId AND quizresult.QuestionSetId = axonifytopic.subjectExternalId
--		--WHEN MATCHED THEN --update
--		--UPDATE SET 
--		-- quizresult.status = (case axonifytopic.graduationTimestamp when 'NULL' then 'Assigned' else 'NULL' end),
--		-- quizresult.grade = (case axonifytopic.graduationTimestamp when 'NULL' then 'Pass' else 'NULL' end),
--		-- quizresult.datetaken = axonifytopic.graduationTimestamp 
--		--WHEN NOT MATCHED BY TARGET THEN --insert
--		--INSERT VALUES(8,getdate(),axonifytopic.graduationTimestamp,'Assigned',NULL,NULL,NULL,NULL,0,NULL,NULL,getdate(),(getdate() + 5),axonifytopic.employeeid,NULL);
--	END
--END 



GO
