
ALTER TABLE Tasks
ADD ScheduleString varchar(50);
GO

INSERT INTO [KMS_Tasks].[dbo].[PriorityLevel](Id, DisplayName, Description) VALUES(1, 'Emergency', '');
GO

INSERT INTO [KMS_Tasks].[dbo].[PriorityLevel](Id, DisplayName, Description) VALUES(2, 'High', '');
GO

INSERT INTO [KMS_Tasks].[dbo].[PriorityLevel](Id, DisplayName, Description) VALUES(3, 'Medium', '');
GO

INSERT INTO [KMS_Tasks].[dbo].[PriorityLevel](Id, DisplayName, Description) VALUES(4, 'Low', '');
GO

INSERT INTO [KMS_Tasks].[dbo].[PriorityLevel](Id, DisplayName, Description) VALUES(5, 'Free', 'Finish task in anytime');
GO

INSERT INTO [KMS_Tasks].[dbo].[ProjectRoles](Name) Values('Owner')
GO

INSERT INTO [KMS_Tasks].[dbo].[ProjectRoles](Name, Description) Values('PM', 'Project Manager')
GO

INSERT INTO [KMS_Tasks].[dbo].[ProjectRoles](Name) Values('Leader')
GO

INSERT INTO [KMS_Tasks].[dbo].[ProjectRoles](Name, Description) Values('QA', 'Quality Assurance')
GO

INSERT INTO [KMS_Tasks].[dbo].[ProjectRoles](Name, Description) Values('Dev', 'Developer')
GO

INSERT INTO [KMS_Tasks].[dbo].[ProjectRoles](Name, Description) Values('BA', 'Business Analyst')
GO

INSERT INTO [KMS_Tasks].[dbo].[ProjectRoles](Name) Values('Member')
GO

INSERT INTO [KMS_Tasks].[dbo].[PriorityLevel](DisplayName) VALUES('Emergency')
GO

INSERT INTO [KMS_Tasks].[dbo].[PriorityLevel](DisplayName) VALUES('High')
GO

INSERT INTO [KMS_Tasks].[dbo].[PriorityLevel](DisplayName) VALUES('Medium')
GO

INSERT INTO [KMS_Tasks].[dbo].[PriorityLevel](DisplayName) VALUES('Low')
GO

INSERT INTO [KMS_Tasks].[dbo].[PriorityLevel](DisplayName) VALUES('Anytime')
GO

USE [KMS_Tasks]
GO

/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 11/13/2020 09:45:38 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/****** Object:  Table [dbo].[AspNetRoleClaims]    Script Date: 11/13/2020 09:45:38 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/****** Object:  Table [dbo].[AspNetRoles]    Script Date: 11/13/2020 09:45:38 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/****** Object:  Table [dbo].[AspNetUserClaims]    Script Date: 11/13/2020 09:45:38 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/****** Object:  Table [dbo].[AspNetUserLogins]    Script Date: 11/13/2020 09:45:38 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/****** Object:  Table [dbo].[AspNetUserRoles]    Script Date: 11/13/2020 09:45:38 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/****** Object:  Table [dbo].[AspNetUsers]    Script Date: 11/13/2020 09:45:38 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/****** Object:  Table [dbo].[AspNetUserTokens]    Script Date: 11/13/2020 09:45:38 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/****** Object:  Table [dbo].[PriorityLevel]    Script Date: 11/13/2020 09:45:38 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/****** Object:  Table [dbo].[Project]    Script Date: 11/13/2020 09:45:38 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/****** Object:  Table [dbo].[ProjectRoles]    Script Date: 11/13/2020 09:45:38 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/****** Object:  Table [dbo].[SysLogs]    Script Date: 11/13/2020 09:45:38 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/****** Object:  Table [dbo].[Tasks]    Script Date: 11/13/2020 09:45:38 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/****** Object:  Table [dbo].[UserProjects]    Script Date: 11/13/2020 09:45:38 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO

--Syntax Error: Incorrect syntax near OPTIMIZE_FOR_SEQUENTIAL_KEY.
--CREATE TABLE [dbo].[__EFMigrationsHistory](
--	[MigrationId] [nvarchar](150) NOT NULL,
--	[ProductVersion] [nvarchar](32) NOT NULL,
-- CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
--(
--	[MigrationId] ASC
--)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
--) ON [PRIMARY]

GO

CREATE TABLE [dbo].[AspNetRoleClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RoleId] [nvarchar](450) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

--Syntax Error: Incorrect syntax near OPTIMIZE_FOR_SEQUENTIAL_KEY.
--CREATE TABLE [dbo].[AspNetRoleClaims](
--	[Id] [int] IDENTITY(1,1) NOT NULL,
--	[RoleId] [nvarchar](450) NOT NULL,
--	[ClaimType] [nvarchar](max) NULL,
--	[ClaimValue] [nvarchar](max) NULL,
-- CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY CLUSTERED 
--(
--	[Id] ASC
--)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
--) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

CREATE TABLE [dbo].[AspNetRoles](
	[Id] [nvarchar](450) NOT NULL,
	[RoleId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](256) NULL,
	[NormalizedName] [nvarchar](256) NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
	[CreatedTime] [datetime] NULL,
 CONSTRAINT [PK_AspNetRoles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

--Syntax Error: Incorrect syntax near OPTIMIZE_FOR_SEQUENTIAL_KEY.
--CREATE TABLE [dbo].[AspNetRoles](
--	[Id] [nvarchar](450) NOT NULL,
--	[RoleId] [int] IDENTITY(1,1) NOT NULL,
--	[Name] [nvarchar](256) NULL,
--	[NormalizedName] [nvarchar](256) NULL,
--	[ConcurrencyStamp] [nvarchar](max) NULL,
--	[CreatedTime] [datetime] NULL,
-- CONSTRAINT [PK_AspNetRoles] PRIMARY KEY CLUSTERED 
--(
--	[Id] ASC
--)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
--) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

CREATE TABLE [dbo].[AspNetUserClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [nvarchar](450) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

--Syntax Error: Incorrect syntax near OPTIMIZE_FOR_SEQUENTIAL_KEY.
--CREATE TABLE [dbo].[AspNetUserClaims](
--	[Id] [int] IDENTITY(1,1) NOT NULL,
--	[UserId] [nvarchar](450) NOT NULL,
--	[ClaimType] [nvarchar](max) NULL,
--	[ClaimValue] [nvarchar](max) NULL,
-- CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY CLUSTERED 
--(
--	[Id] ASC
--)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
--) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

CREATE TABLE [dbo].[AspNetUserLogins](
	[LoginProvider] [nvarchar](450) NOT NULL,
	[ProviderKey] [nvarchar](450) NOT NULL,
	[ProviderDisplayName] [nvarchar](max) NULL,
	[UserId] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY CLUSTERED 
(
	[LoginProvider] ASC,
	[ProviderKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

--Syntax Error: Incorrect syntax near OPTIMIZE_FOR_SEQUENTIAL_KEY.
--CREATE TABLE [dbo].[AspNetUserLogins](
--	[LoginProvider] [nvarchar](450) NOT NULL,
--	[ProviderKey] [nvarchar](450) NOT NULL,
--	[ProviderDisplayName] [nvarchar](max) NULL,
--	[UserId] [nvarchar](450) NOT NULL,
-- CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY CLUSTERED 
--(
--	[LoginProvider] ASC,
--	[ProviderKey] ASC
--)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
--) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

CREATE TABLE [dbo].[AspNetUserRoles](
	[UserId] [nvarchar](450) NOT NULL,
	[RoleId] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO

--Syntax Error: Incorrect syntax near OPTIMIZE_FOR_SEQUENTIAL_KEY.
--CREATE TABLE [dbo].[AspNetUserRoles](
--	[UserId] [nvarchar](450) NOT NULL,
--	[RoleId] [nvarchar](450) NOT NULL,
-- CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY CLUSTERED 
--(
--	[UserId] ASC,
--	[RoleId] ASC
--)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
--) ON [PRIMARY]

GO

CREATE TABLE [dbo].[AspNetUsers](
	[Id] [nvarchar](450) NOT NULL,
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [varchar](256) NULL,
	[NormalizedUserName] [nvarchar](256) NULL,
	[FirstName] [nvarchar](50) NULL,
	[MidName] [nvarchar](50) NULL,
	[LastName] [nvarchar](50) NULL,
	[Email] [nvarchar](256) NULL,
	[NormalizedEmail] [nvarchar](256) NULL,
	[EmailConfirmed] [bit] NOT NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[SecurityStamp] [nvarchar](max) NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
	[Avatar] [varchar](max) NULL,
	[PhoneNumber] [varchar](20) NULL,
	[DateOfBirth] [datetime] NULL,
	[PhoneNumberConfirmed] [bit] NOT NULL,
	[TwoFactorEnabled] [bit] NOT NULL,
	[LockoutEnd] [datetimeoffset](7) NULL,
	[LockoutEnabled] [bit] NOT NULL,
	[AccessFailedCount] [int] NOT NULL,
	[Status] [tinyint] NOT NULL,
	[CreateAt] [datetime] NOT NULL,
 CONSTRAINT [PK_AspNetUsers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [IX_AspNetUsers_1] UNIQUE NONCLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

--Syntax Error: Incorrect syntax near OPTIMIZE_FOR_SEQUENTIAL_KEY.
--CREATE TABLE [dbo].[AspNetUsers](
--	[Id] [nvarchar](450) NOT NULL,
--	[UserId] [int] IDENTITY(1,1) NOT NULL,
--	[UserName] [varchar](256) NULL,
--	[NormalizedUserName] [nvarchar](256) NULL,
--	[FirstName] [nvarchar](50) NULL,
--	[MidName] [nvarchar](50) NULL,
--	[LastName] [nvarchar](50) NULL,
--	[Email] [nvarchar](256) NULL,
--	[NormalizedEmail] [nvarchar](256) NULL,
--	[EmailConfirmed] [bit] NOT NULL,
--	[PasswordHash] [nvarchar](max) NULL,
--	[SecurityStamp] [nvarchar](max) NULL,
--	[ConcurrencyStamp] [nvarchar](max) NULL,
--	[Avatar] [varchar](max) NULL,
--	[PhoneNumber] [varchar](20) NULL,
--	[DateOfBirth] [datetime] NULL,
--	[PhoneNumberConfirmed] [bit] NOT NULL,
--	[TwoFactorEnabled] [bit] NOT NULL,
--	[LockoutEnd] [datetimeoffset](7) NULL,
--	[LockoutEnabled] [bit] NOT NULL,
--	[AccessFailedCount] [int] NOT NULL,
--	[Status] [tinyint] NOT NULL,
--	[CreateAt] [datetime] NOT NULL,
-- CONSTRAINT [PK_AspNetUsers] PRIMARY KEY CLUSTERED 
--(
--	[Id] ASC
--)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
-- CONSTRAINT [IX_AspNetUsers_1] UNIQUE NONCLUSTERED 
--(
--	[UserId] ASC
--)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
--) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

CREATE TABLE [dbo].[AspNetUserTokens](
	[UserId] [nvarchar](450) NOT NULL,
	[LoginProvider] [nvarchar](450) NOT NULL,
	[Name] [nvarchar](450) NOT NULL,
	[Value] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[LoginProvider] ASC,
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

--Syntax Error: Incorrect syntax near OPTIMIZE_FOR_SEQUENTIAL_KEY.
--CREATE TABLE [dbo].[AspNetUserTokens](
--	[UserId] [nvarchar](450) NOT NULL,
--	[LoginProvider] [nvarchar](450) NOT NULL,
--	[Name] [nvarchar](450) NOT NULL,
--	[Value] [nvarchar](max) NULL,
-- CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY CLUSTERED 
--(
--	[UserId] ASC,
--	[LoginProvider] ASC,
--	[Name] ASC
--)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
--) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

CREATE TABLE [dbo].[PriorityLevel](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DisplayName] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](200) NULL,
 CONSTRAINT [PK_PriorityLevel] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO

--Syntax Error: Incorrect syntax near OPTIMIZE_FOR_SEQUENTIAL_KEY.
--CREATE TABLE [dbo].[PriorityLevel](
--	[Id] [int] IDENTITY(1,1) NOT NULL,
--	[DisplayName] [nvarchar](50) NOT NULL,
--	[Description] [nvarchar](200) NULL,
-- CONSTRAINT [PK_PriorityLevel] PRIMARY KEY CLUSTERED 
--(
--	[Id] ASC
--)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
--) ON [PRIMARY]

GO

CREATE TABLE [dbo].[Project](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](250) NULL,
	[CreatedDate] [datetime] NULL,
	[UpdatedDate] [datetime] NULL,
	[Deleted] [bit] NOT NULL,
	[CreatedBy] [int] NULL,
	[ParentId] [int] NULL,
	[UpdatedBy] [int] NULL,
 CONSTRAINT [PK_Project] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO

--Syntax Error: Incorrect syntax near OPTIMIZE_FOR_SEQUENTIAL_KEY.
--CREATE TABLE [dbo].[Project](
--	[Id] [int] IDENTITY(1,1) NOT NULL,
--	[Name] [nvarchar](100) NOT NULL,
--	[Description] [nvarchar](250) NULL,
--	[CreatedDate] [datetime] NULL,
--	[UpdatedDate] [datetime] NULL,
--	[Deleted] [bit] NOT NULL,
--	[CreatedBy] [int] NULL,
--	[ParentId] [int] NULL,
--	[UpdatedBy] [int] NULL,
-- CONSTRAINT [PK_Project] PRIMARY KEY CLUSTERED 
--(
--	[Id] ASC
--)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
--) ON [PRIMARY]

GO

CREATE TABLE [dbo].[ProjectRoles](
	[Id] [tinyint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](200) NULL,
 CONSTRAINT [PK_ProjectRoles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO

--Syntax Error: Incorrect syntax near OPTIMIZE_FOR_SEQUENTIAL_KEY.
--CREATE TABLE [dbo].[ProjectRoles](
--	[Id] [tinyint] IDENTITY(1,1) NOT NULL,
--	[Name] [nvarchar](50) NOT NULL,
--	[Description] [nvarchar](200) NULL,
-- CONSTRAINT [PK_ProjectRoles] PRIMARY KEY CLUSTERED 
--(
--	[Id] ASC
--)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
--) ON [PRIMARY]

GO

CREATE TABLE [dbo].[SysLogs](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[When] [datetime] NOT NULL,
	[Message] [nvarchar](max) NOT NULL,
	[Level] [nvarchar](10) NOT NULL,
	[Exception] [nvarchar](max) NOT NULL,
	[Trace] [nvarchar](max) NOT NULL,
	[Logger] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_SysLogs] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

--Syntax Error: Incorrect syntax near OPTIMIZE_FOR_SEQUENTIAL_KEY.
--CREATE TABLE [dbo].[SysLogs](
--	[Id] [int] IDENTITY(1,1) NOT NULL,
--	[When] [datetime] NOT NULL,
--	[Message] [nvarchar](max) NOT NULL,
--	[Level] [nvarchar](10) NOT NULL,
--	[Exception] [nvarchar](max) NOT NULL,
--	[Trace] [nvarchar](max) NOT NULL,
--	[Logger] [nvarchar](max) NOT NULL,
-- CONSTRAINT [PK_SysLogs] PRIMARY KEY CLUSTERED 
--(
--	[Id] ASC
--)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
--) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

CREATE TABLE [dbo].[Tasks](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[Schedule] [datetime] NULL,
	[ScheduleString] [varchar](50) NULL,
	[PriorityId] [int] NULL,
	[Deleted] [bit] NOT NULL,
	[UpdatedDate] [datetime] NULL,
	[ProjectId] [int] NULL,
	[SectionId] [int] NULL,
	[ParentId] [int] NULL,
	[ReminderSchedule] [datetime] NULL,
	[Reminder] [bit] NOT NULL,
	[AssignedBy] [int] NULL,
	[AssignedFor] [int] NULL,
	[CreatedBy] [int] NULL,
 CONSTRAINT [PK_Tasks] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO

--Syntax Error: Incorrect syntax near OPTIMIZE_FOR_SEQUENTIAL_KEY.
--CREATE TABLE [dbo].[Tasks](
--	[Id] [int] IDENTITY(1,1) NOT NULL,
--	[Name] [nvarchar](100) NOT NULL,
--	[CreatedDate] [datetime] NOT NULL,
--	[Schedule] [datetime] NULL,
--	[ScheduleString] [varchar](50) NULL,
--	[PriorityId] [int] NULL,
--	[Deleted] [bit] NOT NULL,
--	[UpdatedDate] [datetime] NULL,
--	[ProjectId] [int] NULL,
--	[SectionId] [int] NULL,
--	[ParentId] [int] NULL,
--	[ReminderSchedule] [datetime] NULL,
--	[Reminder] [bit] NOT NULL,
--	[AssignedBy] [int] NULL,
--	[AssignedFor] [int] NULL,
--	[CreatedBy] [int] NULL,
-- CONSTRAINT [PK_Tasks] PRIMARY KEY CLUSTERED 
--(
--	[Id] ASC
--)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
--) ON [PRIMARY]



GO
