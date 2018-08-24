-- =============================================
-- Create date: 2018 April 06
-- Description:	Manage the history of the executions of each interfaces
-- Example:	execute axf_usp_InterfaceGetPendingUsers
-- =============================================
CREATE PROCEDURE [dbo].[axf_usp_InterfaceHistoryUpdate]
	@Action VARCHAR(50),
	@InterfaceId VARCHAR(80),
	@ErrorMessage VARCHAR(MAX)
AS
BEGIN
	IF @Action = 'NEWRECORD'
	BEGIN
		INSERT INTO axf_Interfaces_HistoryCalls(InterfaceId, StartExecutionDate) VALUES (@InterfaceId, GETDATE())
	END
	ELSE IF @Action = 'SUCCESSPROCESS'
	BEGIN
		UPDATE axf_Interfaces_HistoryCalls SET 
			EndExecutionDate = GETDATE(), 
			IsSuccess = 1 
		WHERE InterfaceId = @InterfaceId AND EndExecutionDate IS NULL

		UPDATE axf_Interfaces SET LastExecutionDate = GETDATE() WHERE InterfaceId = @InterfaceId
	END
	ELSE IF @Action = 'FAILPROCESS'
	BEGIN
		UPDATE axf_Interfaces_HistoryCalls SET 
			EndExecutionDate = GETDATE(), 
			IsSuccess = 0, 
			ErrorMessage = @ErrorMessage 
		WHERE InterfaceId = @InterfaceId AND EndExecutionDate IS NULL
	END
	ELSE IF @Action = 'CANCELPROCESS'
	BEGIN
		DELETE axf_Interfaces_HistoryCalls WHERE InterfaceId = @InterfaceId AND EndExecutionDate IS NULL
	END
END