-- =============================================
-- Create date: 2018-08-24
-- Description:	Update all introductory completions obtained by axonify into BOS system
/* Example:	EXECUTE axf_usp_InterfaceUpdateIntroductoryCompletions @IntroductoryCompletionsXML = '<ArrayOfIntroductoryCompletion xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <IntroductoryCompletion>
    <employeeId>107</employeeId>
    <completionTimestamp>20180824T20:11-05:00</completionTimestamp>
    <assessmentScore>11</assessmentScore>
    <timeSpent>51</timeSpent>
    <topicDetails>
      <categoryName>categoryName1</categoryName>
      <categoryExternalId>categoryExternalId1</categoryExternalId>
      <subjectName>subjectName1</subjectName>
      <subjectExternalId>subjectExternalId1</subjectExternalId>
      <topicName>topicName1</topicName>
      <topicExternalId>583</topicExternalId>
      <level>21</level>
    </topicDetails>
  </IntroductoryCompletion>
</ArrayOfIntroductoryCompletion>
	'
*/
-- =============================================
CREATE PROCEDURE [dbo].[axf_usp_InterfaceUpdateIntroductoryCompletions]
	@IntroductoryCompletionsXML XML --XML of IntroductoryCompletion class
AS
BEGIN
	IF @IntroductoryCompletionsXML IS NOT NULL
	BEGIN
		DECLARE @TblIntroductoryCompletions TABLE(
			employeeId VARCHAR(255),
			completionTimestamp VARCHAR(20), --ISO 8601 Format YYYYMMDDTHH:mm±hh:mm
			assessmentScore FLOAT,
			timeSpent INT,

			categoryName VARCHAR(60),
			categoryExternalId VARCHAR(60),
			subjectName VARCHAR(60),
			subjectExternalId VARCHAR(60),
			topicName VARCHAR(60),
			topicExternalId VARCHAR(60),
			[level] INT
		)

		INSERT INTO @TblIntroductoryCompletions
		SELECT  
			employeeId = Tbl.Col.value('(employeeId/text())[1]', 'varchar(255)'),
			completionTimestamp = Tbl.Col.value('(completionTimestamp/text())[1]', 'varchar(100)'),
			assessmentScore = Tbl.Col.value('(assessmentScore/text())[1]', 'float'),
			timeSpent = Tbl.Col.value('(timeSpent/text())[1]', 'int'),
			
			categoryName = Tbl.Col.value('(topicDetails/categoryName/text())[1]', 'varchar(60)'),
			categoryExternalId = Tbl.Col.value('(topicDetails/categoryExternalId/text())[1]', 'varchar(60)'),
			subjectName = Tbl.Col.value('(topicDetails/subjectName/text())[1]', 'varchar(60)'),
			subjectExternalId = Tbl.Col.value('(topicDetails/subjectExternalId/text())[1]', 'varchar(60)'),
			topicName = Tbl.Col.value('(topicDetails/topicName/text())[1]', 'varchar(60)'),
			topicExternalId = Tbl.Col.value('(topicDetails/topicExternalId/text())[1]', 'varchar(60)'),
			[level] = Tbl.Col.value('(topicDetails/level/text())[1]', 'int')
		FROM @IntroductoryCompletionsXML.nodes('//ArrayOfIntroductoryCompletion/IntroductoryCompletion') Tbl(Col)
		OPTION (OPTIMIZE FOR ( @IntroductoryCompletionsXML = NULL ))

		MERGE INTO [winstontraining].[dbo].[QUIZRESULT] WITH (HOLDLOCK) AS quizresult
		USING (
			SELECT axonify.*
			FROM @TblIntroductoryCompletions axonify
			JOIN [winstontraining].[dbo].[QUESTIONSET] q
				ON axonify.topicExternalId = q.questionsetid
		) AS axonifySource
			ON quizresult.MCID = axonifySource.employeeId AND quizresult.questionsetid = axonifySource.topicExternalId
		WHEN MATCHED THEN --update
		UPDATE SET 
			quizresult.status = (case when axonifySource.completionTimestamp IS NULL then 'Assigned' else 'Completed' end),
			quizresult.grade = (case when axonifySource.completionTimestamp IS NULL then 'Pass' else 'Pass' end),
			quizresult.datetaken = [dbo].[axf_udf_ConvertDatISO8601ToDateTime](axonifySource.completionTimestamp) 
		WHEN NOT MATCHED THEN  
		INSERT ([QuestionSetID],[DateAssigned],[DateTaken],[Status],[Grade],[Attempts],[DateReviewed],[ReviewedBy],[Closed],[ProjectSegmentID],[ProjectEventAssignmentID],[ExecutionDateMin],[ExecutionDateMax],[MCID],[EmailedOn])  
		VALUES(axonifySource.topicExternalId,getdate(),[dbo].[axf_udf_ConvertDatISO8601ToDateTime](axonifySource.completionTimestamp),'Assigned',NULL,NULL,NULL,NULL,0,NULL,NULL,getdate(),(getdate() + 5),axonifySource.employeeid,NULL);

	END
END 