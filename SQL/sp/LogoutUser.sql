IF OBJECT_ID('LogoutUser', 'P') IS NOT NULL
DROP PROC LogoutUser
GO

CREATE PROCEDURE LogoutUser   
    @userId int   
AS   
	if not exists (select * from dbo.[User] u where u.UserId = @userId) begin
		print 'user does not exist'
		return -1
    end
	 
	declare @count int
	--	, @userId int
	--set @userId = -1

    SELECT @count = Isnull( count(*), 0)   
    FROM dbo.[User] u inner join dbo.LoginLog ll 
		on u.UserId = ll.UserId
    WHERE u.UserId = @userId  
		and not(isnull(ll.LoginDate, 0) = 0)
		and isnull(ll.LogoutDate, 0) = 0
		and isnull(ll.DisposeDate, 0)  = 0
	--print @count

	if (@count = 0) begin
		UPDATE dbo.[User]
		SET IsOnline = 0
		WHERE UserId = @userId
	end else begin
		UPDATE dbo.[User]
		SET IsOnline = 1
		WHERE UserId = @userId
	end

	if @@ERROR = 0
		return 1

	return -2
GO  