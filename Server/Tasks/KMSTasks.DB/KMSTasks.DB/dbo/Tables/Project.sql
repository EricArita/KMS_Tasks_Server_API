CREATE TABLE [dbo].[Project] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [Name]        NVARCHAR (100) NOT NULL,
    [Description] NVARCHAR (250) NULL,
    [CreatedDate] DATETIME       NULL,
    [UpdatedDate] DATETIME       NULL,
    [Deleted]     BIT            NOT NULL,
    [CreatedBy]   INT            NULL,
    [ParentId]    INT            NULL,
    [UpdatedBy]   INT            NULL,
    CONSTRAINT [PK_Project] PRIMARY KEY CLUSTERED ([Id] ASC),
    ALTER TABLE [dbo].[Project]  WITH CHECK ADD  CONSTRAINT [FK_Project_Project_ParentId] FOREIGN KEY([ParentId])
REFERENCES [dbo].[Project] ([Id])
GO

ALTER TABLE [dbo].[Project] CHECK CONSTRAINT [FK_Project_Project_ParentId]
);


GO
CREATE NONCLUSTERED INDEX [IX_Project_ParentId]
    ON [dbo].[Project]([ParentId] ASC);


GO
ALTER TABLE [dbo].[Project]  WITH CHECK ADD  CONSTRAINT [FK_Project_Project_ParentId] FOREIGN KEY([ParentId])
REFERENCES [dbo].[Project] ([Id])
GO

ALTER TABLE [dbo].[Project] CHECK CONSTRAINT [FK_Project_Project_ParentId]