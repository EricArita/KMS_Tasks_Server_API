CREATE TABLE [dbo].[ProjectRoles] (
    [Id]          INT            NOT NULL,
    [Name]        NVARCHAR (50)  NOT NULL,
    [Description] NVARCHAR (200) NULL,
    CONSTRAINT [PK_ProjectRoles] PRIMARY KEY CLUSTERED ([Id] ASC)
);

