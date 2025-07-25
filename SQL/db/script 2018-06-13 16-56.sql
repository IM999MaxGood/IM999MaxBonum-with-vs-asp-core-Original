USE [IM999Max_DotNetCore]
GO
/****** Object:  StoredProcedure [dbo].[CorrectUserOnline]    Script Date: 13/06/2018 16:55:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


create procedure [dbo].[CorrectUserOnline]   
    @userId int   
AS   
	declare  @count int
	set @count = 0
	
	--کاربر لاگ باز اکسپار نشده داشته باشد
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

GO
/****** Object:  StoredProcedure [dbo].[DisposeLoginsAndSetOnline]    Script Date: 13/06/2018 16:55:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[DisposeLoginsAndSetOnline] 
 
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

	


GO
/****** Object:  StoredProcedure [dbo].[LogoutUser]    Script Date: 13/06/2018 16:55:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[LogoutUser]   
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
/****** Object:  Table [dbo].[Language]    Script Date: 13/06/2018 16:55:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Language](
	[LangId] [int] IDENTITY(1,1) NOT NULL,
	[LangMark] [nvarchar](10) NOT NULL,
	[Direction] [nvarchar](10) NOT NULL,
	[LangName] [nvarchar](100) NOT NULL,
	[LangNameInter] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_Language] PRIMARY KEY CLUSTERED 
(
	[LangId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[LoginLog]    Script Date: 13/06/2018 16:55:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LoginLog](
	[LLId] [bigint] IDENTITY(1,1) NOT NULL,
	[GUID] [nchar](100) NOT NULL,
	[UserId] [int] NULL,
	[ExpireMin] [int] NOT NULL,
	[LastRefresh] [datetime] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[LoginDate] [datetime] NULL,
	[LogoutDate] [datetime] NULL,
	[DisposeDate] [datetime] NULL,
	[Location] [nvarchar](1000) NULL,
	[IpAddress] [nvarchar](max) NOT NULL,
	[Browser] [nvarchar](max) NOT NULL,
	[OS] [nvarchar](max) NULL,
	[SD] [nvarchar](max) NULL,
 CONSTRAINT [PK_LoginLog] PRIMARY KEY CLUSTERED 
(
	[LLId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Menu]    Script Date: 13/06/2018 16:55:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Menu](
	[MenuId] [int] IDENTITY(1,1) NOT NULL,
	[MenuName] [nvarchar](100) NOT NULL,
	[LangId] [int] NOT NULL,
	[CanEdit] [bit] NOT NULL,
	[IsMainMenu] [bit] NOT NULL,
	[MenuFileName] [nvarchar](1000) NOT NULL,
 CONSTRAINT [PK_Menu] PRIMARY KEY CLUSTERED 
(
	[MenuId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Page]    Script Date: 13/06/2018 16:55:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Page](
	[PageId] [int] IDENTITY(1,1) NOT NULL,
	[LangId] [int] NOT NULL,
	[PageGroupId] [int] NOT NULL,
	[PageName] [nvarchar](1000) NOT NULL,
	[PageContentMini] [nvarchar](max) NOT NULL,
	[PageContentHTML] [nvarchar](max) NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[LastModifyDate] [datetime] NOT NULL,
	[CreateUser] [int] NOT NULL,
	[LastModifyUser] [int] NOT NULL,
	[PageTopCode] [nvarchar](max) NOT NULL,
	[CanDelete] [bit] NOT NULL,
	[UnicId] [varchar](100) NOT NULL,
	[UnderConstract] [bit] NOT NULL,
 CONSTRAINT [PK_Page] PRIMARY KEY CLUSTERED 
(
	[PageId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PageGroup]    Script Date: 13/06/2018 16:55:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PageGroup](
	[PageGroupId] [int] IDENTITY(1,1) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CanEdit] [bit] NOT NULL,
	[PageGroupName] [nvarchar](1000) NOT NULL,
	[LangId] [int] NOT NULL,
 CONSTRAINT [PK_PageGroup] PRIMARY KEY CLUSTERED 
(
	[PageGroupId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[User]    Script Date: 13/06/2018 16:55:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [nvarchar](100) NOT NULL,
	[Password] [nvarchar](1000) NOT NULL,
	[Email] [nvarchar](1000) NOT NULL,
	[IsOnline] [bit] NOT NULL,
	[LastLoginDate] [datetime] NOT NULL,
	[IsPersonel] [bit] NOT NULL,
	[RegisterDate] [datetime] NOT NULL,
	[IsManager] [bit] NOT NULL,
	[IsConfirm] [bit] NOT NULL,
	[ConfirmCode] [nvarchar](max) NOT NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  View [dbo].[vMenu]    Script Date: 13/06/2018 16:55:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vMenu]
AS
SELECT        dbo.Menu.MenuId, dbo.Menu.MenuName, dbo.Menu.LangId, dbo.Menu.CanEdit, dbo.Menu.IsMainMenu, dbo.Menu.MenuFileName, dbo.Language.LangMark, dbo.Language.Direction, dbo.Language.LangName, 
                         dbo.Language.LangNameInter
FROM            dbo.Menu INNER JOIN
                         dbo.Language ON dbo.Menu.LangId = dbo.Language.LangId

GO
/****** Object:  View [dbo].[vPage]    Script Date: 13/06/2018 16:55:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vPage]
AS
SELECT        dbo.Page.PageId, dbo.Page.LangId, dbo.Page.PageGroupId, dbo.Page.PageName, dbo.Page.PageContentMini, dbo.Page.PageContentHTML, dbo.Page.CreateDate, dbo.Page.LastModifyDate, dbo.Page.CreateUser, 
                         dbo.Page.LastModifyUser, dbo.Page.PageTopCode, dbo.Page.CanDelete, dbo.Page.UnicId, dbo.Page.UnderConstract, dbo.Language.LangMark, dbo.Language.Direction, dbo.Language.LangName, 
                         dbo.Language.LangNameInter
FROM            dbo.Page INNER JOIN
                         dbo.Language ON dbo.Page.LangId = dbo.Language.LangId

GO
/****** Object:  View [dbo].[vPageGroup]    Script Date: 13/06/2018 16:55:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vPageGroup]
AS
SELECT        dbo.PageGroup.PageGroupId, dbo.PageGroup.IsActive, dbo.PageGroup.CanEdit, dbo.PageGroup.PageGroupName, dbo.PageGroup.LangId, dbo.Language.LangMark, dbo.Language.Direction, dbo.Language.LangName, 
                         dbo.Language.LangNameInter
FROM            dbo.PageGroup INNER JOIN
                         dbo.Language ON dbo.PageGroup.LangId = dbo.Language.LangId

GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "Menu"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 136
               Right = 208
            End
            DisplayFlags = 280
            TopColumn = 2
         End
         Begin Table = "Language"
            Begin Extent = 
               Top = 6
               Left = 246
               Bottom = 136
               Right = 417
            End
            DisplayFlags = 280
            TopColumn = 1
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vMenu'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vMenu'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "Page"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 136
               Right = 229
            End
            DisplayFlags = 280
            TopColumn = 10
         End
         Begin Table = "Language"
            Begin Extent = 
               Top = 6
               Left = 267
               Bottom = 136
               Right = 438
            End
            DisplayFlags = 280
            TopColumn = 1
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vPage'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vPage'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "PageGroup"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 136
               Right = 218
            End
            DisplayFlags = 280
            TopColumn = 1
         End
         Begin Table = "Language"
            Begin Extent = 
               Top = 6
               Left = 256
               Bottom = 136
               Right = 427
            End
            DisplayFlags = 280
            TopColumn = 1
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vPageGroup'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vPageGroup'
GO
