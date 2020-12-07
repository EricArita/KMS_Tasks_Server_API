CREATE TABLE [dbo].[Tasks] (
    [Id]               BIGINT         IDENTITY (1, 1) NOT NULL,
    [Name]             NVARCHAR (100) NOT NULL,
    [CreatedDate]      DATETIME       NOT NULL,
    [Schedule]         DATETIME       NULL,
    [ScheduleString]   NVARCHAR (MAX) NULL,
    [PriorityId]       INT            NULL,
    [Deleted]          BIT            NOT NULL,
    [UpdatedDate]      DATETIME2 (7)  NULL,
    [ProjectId]        BIGINT         NULL,
    [ParentId]         BIGINT         NULL,
    [ReminderSchedule] DATETIME       NULL,
    [Reminder]         BIT            NOT NULL,
    [AssignedBy]       BIGINT         NULL,
    [AssignedFor]      BIGINT         NULL,
    [CreatedBy]        BIGINT         NULL,
    CONSTRAINT [PK_Tasks] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Task_AssignedBy_User] FOREIGN KEY ([AssignedBy]) REFERENCES [dbo].[AspNetUsers] ([UserId]),
    CONSTRAINT [FK_Task_AssignedFor_User] FOREIGN KEY ([AssignedFor]) REFERENCES [dbo].[AspNetUsers] ([UserId]),
    CONSTRAINT [FK_Task_CreatedBy_User] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[AspNetUsers] ([UserId]),
    CONSTRAINT [FK_Tasks_PriorityLevel] FOREIGN KEY ([PriorityId]) REFERENCES [dbo].[PriorityLevel] ([Id]),
    CONSTRAINT [FK_Tasks_Project] FOREIGN KEY ([ProjectId]) REFERENCES [dbo].[Project] ([Id]),
    CONSTRAINT [FK_Tasks_Tasks] FOREIGN KEY ([ParentId]) REFERENCES [dbo].[Tasks] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_Tasks_AssignedBy]
    ON [dbo].[Tasks]([AssignedBy] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Tasks_AssignedFor]
    ON [dbo].[Tasks]([AssignedFor] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Tasks_CreatedBy]
    ON [dbo].[Tasks]([CreatedBy] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Tasks_ParentId]
    ON [dbo].[Tasks]([ParentId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Tasks_PriorityId]
    ON [dbo].[Tasks]([PriorityId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Tasks_ProjectId]
    ON [dbo].[Tasks]([ProjectId] ASC);

