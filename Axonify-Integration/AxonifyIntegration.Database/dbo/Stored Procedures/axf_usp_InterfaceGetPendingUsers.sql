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
	--or t.talentid = 107

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
	--or t.talentid = 107
END