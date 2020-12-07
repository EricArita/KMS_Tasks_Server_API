CREATE TABLE [dbo].[SysLogs] (
    [Id]        INT            IDENTITY (1, 1) NOT NULL,
    [When]      DATETIME       NOT NULL,
    [Message]   NVARCHAR (MAX) NOT NULL,
    [Level]     NVARCHAR (10)  NOT NULL,
    [Exception] NVARCHAR (MAX) NOT NULL,
    [Trace]     NVARCHAR (MAX) NOT NULL,
    [Logger]    NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_SysLogs] PRIMARY KEY CLUSTERED ([Id] ASC)
);

