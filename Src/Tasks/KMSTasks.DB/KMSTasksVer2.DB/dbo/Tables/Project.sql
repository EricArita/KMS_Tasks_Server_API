CREATE TABLE [dbo].[Project] (
    [Id]          BIGINT         IDENTITY (1, 1) NOT NULL,
    [Name]        NVARCHAR (100) NOT NULL,
    [Description] NVARCHAR (250) NULL,
    [ParentId]    BIGINT         NULL,
    [CreatedDate] DATETIME       NULL,
    [CreatedBy]   BIGINT         NULL,
    [UpdatedDate] DATETIME       NULL,
    [UpdatedBy]   BIGINT         NULL,
    [Deleted]     BIT            NOT NULL,
    CONSTRAINT [PK_Project] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Project_CreatedBy_User] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[AspNetUsers] ([UserId]),
    CONSTRAINT [FK_Project_Project] FOREIGN KEY ([ParentId]) REFERENCES [dbo].[Project] ([Id]),
    CONSTRAINT [FK_Project_UpdatedBy_User] FOREIGN KEY ([UpdatedBy]) REFERENCES [dbo].[AspNetUsers] ([UserId])
);


GO
CREATE NONCLUSTERED INDEX [IX_Project_CreatedBy]
    ON [dbo].[Project]([CreatedBy] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Project_ParentId]
    ON [dbo].[Project]([ParentId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Project_UpdatedBy]
    ON [dbo].[Project]([UpdatedBy] ASC);

