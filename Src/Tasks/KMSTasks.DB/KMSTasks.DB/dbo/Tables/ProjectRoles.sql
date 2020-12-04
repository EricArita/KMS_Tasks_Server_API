CREATE TABLE [dbo].[ProjectRoles] (
    [Id]          TINYINT        IDENTITY (1, 1) NOT NULL,
    [Name]        NVARCHAR (50)  NOT NULL,
    [Description] NVARCHAR (200) NULL,
    CONSTRAINT [PK_ProjectRoles] PRIMARY KEY CLUSTERED ([Id] ASC)
);

