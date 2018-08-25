-- =============================================
-- Author:		<Jorge Arturo Luna de la Rosa>
-- Create date: <2018-08-24>
-- Description:	<Convert a datetime ISO 8601 from format "YYYYMMDDTHH:mm±hh:mm">
-- Example: SELECT [dbo].[axf_udf_ConvertDatISO8601ToDateTime]('20151119T00:00-05:00')
-- =============================================
CREATE FUNCTION [dbo].[axf_udf_ConvertDatISO8601ToDateTime](@ISO8601Date VARCHAR(20))
RETURNS DATETIME
AS
BEGIN
	DECLARE @result DATETIME = NULL

	IF @ISO8601Date IS NOT NULL
	BEGIN
		SET @result = CAST((LEFT(@ISO8601Date, 4) + '-' + RIGHT(LEFT(@ISO8601Date, 6),2) + '-' + RIGHT(LEFT(@ISO8601Date, 8),2) + ' ' + RIGHT(LEFT(@ISO8601Date, 11),2) + ':' + RIGHT(LEFT(@ISO8601Date, 14),2) + ':00') AS DATETIME)
	END

	RETURN @result
END