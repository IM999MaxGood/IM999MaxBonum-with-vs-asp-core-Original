IF OBJECT_ID('CorrectUserOnline', 'P') IS NOT NULL
	DROP PROC CorrectUserOnline
GO


create procedure [dbo].[CorrectUserOnline]   
    @userId int   
AS   
	declare  @count int
	set @count = 0
	
	--˜ÇÑÈÑ áÇ ÈÇÒ Ç˜ÓÇÑ äÔÏå ÏÇÔÊå ÈÇÔÏ
	select @count= ISNULL(count(*), 0)
	from dbo.LoginLog ll WITH (NOLOCK) 
	where ISNULL(ll.DisposeDate, 0) = 0
		and ISNULL(ll.LogoutDate, 0) = 0
		and DATEADD(mi, ll.ExpireMin, ll.LastRefresh) > GETDATE()
		and ll.UserId =  @userId
	
	print '@count: '+cast(@count as nvarchar(10))
	
	UPDATE dbo.[User] 
	SET  IsOnline =   
		  CASE 
			 WHEN @count = 0 THEN 0
			 ELSE 1
		  END
	where UserId =  @userId


	if @@ERROR = 0
		return 1

	return -1
go