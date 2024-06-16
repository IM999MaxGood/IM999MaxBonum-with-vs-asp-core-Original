select * from [dbo].[User]

exec LogoutUser 1

select * from [dbo].[User]


/*
INSERT INTO [dbo].[User]
           ([UserName] ,[Password] ,[Email] ,[IsOnline] ,[LastLoginDate] ,[IsPersonel] ,[RegisterDate] ,[IsManager] ,[IsConfirm]
           ,[ConfirmCode] ,[IsActive])
     VALUES
           ('IM999Max' , '123' , 'iounes.manoochehri@outlook.com' , 1 , GETDATE() , 1 , GETDATE(), 1, 1 , '', 1)
GO

*/

