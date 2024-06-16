IF OBJECT_ID('DisposeLoginsAndSetOnline', 'P') IS NOT NULL
	DROP PROC DisposeLoginsAndSetOnline
GO

CREATE PROCEDURE DisposeLoginsAndSetOnline 
 
AS   
	IF OBJECT_ID('tempdb..#TempTable') IS NOT NULL DROP TABLE #TempTable
	
	--انتخاب کاربرانی که در بانک لاگی دارند که زمان ابطالش گذشته و آنلاین ثبت شده اند 
	select u.UserId
	into #TempTable  
	from dbo.[User] u
	where u.UserId in (
		select ll.UserId 
		from dbo.LoginLog ll
		where ISNULL(ll.DisposeDate, 0) = 0
			and DATEADD(mi, ll.ExpireMin, ll.LastRefresh) < GETDATE()
			and not(isnull(ll.UserId, 0) = 0)
	) and (u.IsOnline = 1)

	select * from #TempTable

	--مقداردهی زمان ابطال تمامی لاگهای باز
	UPDATE dbo.LoginLog 
	SET DisposeDate = GETDATE()
	--	,LogoutDate= GETDATE()
	from dbo.LoginLog ll
	where ISNULL(ll.DisposeDate, 0) = 0
		and DATEADD(mi, ll.ExpireMin, ll.LastRefresh) < GETDATE()

	--+Cur
	--کرسر برای درست کردن وضعیت آنلاین بودن کاربر
	declare @userId int
	declare cur CURSOR LOCAL for
		select UserId  from #TempTable 

	open cur

	fetch next from cur into @userId

	while @@FETCH_STATUS = 0 BEGIN
		--print 'exec LogoutUser '+CAST(@userId as varchar(10)) 
		--exec LogoutUser @userId
		print 'exec CorrectUserOnline '+CAST(@userId as varchar(10)) 
		exec CorrectUserOnline @userId

		fetch next from cur into @userId
	END

	close cur
	deallocate cur
	---Cur

	IF OBJECT_ID('tempdb..#TempTable') IS NOT NULL DROP TABLE #TempTable

	if @@ERROR = 0
		return 1

	return -1

	

go
