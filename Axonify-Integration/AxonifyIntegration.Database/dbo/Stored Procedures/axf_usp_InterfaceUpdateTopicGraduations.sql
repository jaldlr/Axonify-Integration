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