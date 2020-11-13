CREATE TABLE [dbo].[PriorityLevel] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [DisplayName] NVARCHAR (50)  NOT NULL,
    [Description] NVARCHAR (200) NULL,
    CONSTRAINT [PK_PriorityLevel] PRIMARY KEY CLUSTERED ([Id] ASC)
);

