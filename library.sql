USE [library]
GO
/****** Object:  Table [dbo].[authors]    Script Date: 12/15/2016 4:51:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[authors](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](50) NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[authors_books]    Script Date: 12/15/2016 4:51:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[authors_books](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[author_id] [int] NULL,
	[book_id] [int] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[books]    Script Date: 12/15/2016 4:51:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[books](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[title] [varchar](50) NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[checkouts]    Script Date: 12/15/2016 4:51:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[checkouts](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[copy_id] [int] NULL,
	[patron_id] [int] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[copies]    Script Date: 12/15/2016 4:51:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[copies](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[book_id] [int] NULL,
	[number_of] [int] NULL,
	[due_date] [date] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[patrons]    Script Date: 12/15/2016 4:51:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[patrons](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](50) NULL
) ON [PRIMARY]

GO
