CREATE TABLE [dbo].[Tasks] (
    [Id]               INT            IDENTITY (1, 1) NOT NULL,
    [Name]             NVARCHAR (100) NOT NULL,
    [CreatedDate]      DATETIME       NOT NULL,
    [Schedule]         DATETIME       NULL,
    [ScheduleString]   VARCHAR (50)   NULL,
    [PriorityId]       INT            NULL,
    [Deleted]          BIT            ALTER TABLE [dbo].[Tasks] ADD  CONSTRAINT [DF_Tasks_Deleted]  DEFAULT ((0)) FOR [Deleted] NOT NULL,
    [UpdatedDate]      DATETIME       NULL,
    [ProjectId]        INT            NULL,
    [SectionId]        INT            NULL,
    [ParentId]         INT            NULL,
    [ReminderSchedule] DATETIME       NULL,
    [Reminder]         BIT            ALTER TABLE [dbo].[Tasks] ADD  CONSTRAINT [DF_Tasks_Reminder]  DEFAULT ((0)) FOR [Reminder] NOT NULL,
    [AssignedBy]       INT            NULL,
    [AssignedFor]      INT            NULL,
    [CreatedBy]        INT            NULL,
    CONSTRAINT [PK_Tasks] PRIMARY KEY CLUSTERED ([Id] ASC),
    ALTER TABLE [dbo].[Tasks]  WITH CHECK ADD  CONSTRAINT [FK_Tasks_AspNetUsers] FOREIGN KEY([AssignedBy])
REFERENCES [dbo].[AspNetUsers] ([UserId])
GO

ALTER TABLE [dbo].[Tasks] CHECK CONSTRAINT [FK_Tasks_AspNetUsers],
    ALTER TABLE [dbo].[Tasks]  WITH CHECK ADD  CONSTRAINT [FK_Tasks_AspNetUsers1] FOREIGN KEY([AssignedFor])
REFERENCES [dbo].[AspNetUsers] ([UserId])
GO

ALTER TABLE [dbo].[Tasks] CHECK CONSTRAINT [FK_Tasks_AspNetUsers1],
    ALTER TABLE [dbo].[Tasks]  WITH CHECK ADD  CONSTRAINT [FK_Tasks_AspNetUsers2] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[AspNetUsers] ([UserId])
GO

ALTER TABLE [dbo].[Tasks] CHECK CONSTRAINT [FK_Tasks_AspNetUsers2],
    ALTER TABLE [dbo].[Tasks]  WITH CHECK ADD  CONSTRAINT [FK_Tasks_PriorityLevel] FOREIGN KEY([PriorityId])
REFERENCES [dbo].[PriorityLevel] ([Id])
GO

ALTER TABLE [dbo].[Tasks] CHECK CONSTRAINT [FK_Tasks_PriorityLevel],
    ALTER TABLE [dbo].[Tasks]  WITH CHECK ADD  CONSTRAINT [FK_Tasks_Project] FOREIGN KEY([ProjectId])
REFERENCES [dbo].[Project] ([Id])
GO

ALTER TABLE [dbo].[Tasks] CHECK CONSTRAINT [FK_Tasks_Project],
    ALTER TABLE [dbo].[Tasks]  WITH CHECK ADD  CONSTRAINT [FK_Tasks_Tasks] FOREIGN KEY([ParentId])
REFERENCES [dbo].[Tasks] ([Id])
GO

ALTER TABLE [dbo].[Tasks] CHECK CONSTRAINT [FK_Tasks_Tasks]
);


GO
ALTER TABLE [dbo].[Tasks] ADD  CONSTRAINT [DF_Tasks_Reminder]  DEFAULT ((0)) FOR [Reminder]
GO
ALTER TABLE [dbo].[Tasks] ADD  CONSTRAINT [DF_Tasks_Deleted]  DEFAULT ((0)) FOR [Deleted]
GO
ALTER TABLE [dbo].[Tasks]  WITH CHECK ADD  CONSTRAINT [FK_Tasks_Tasks] FOREIGN KEY([ParentId])
REFERENCES [dbo].[Tasks] ([Id])
GO

ALTER TABLE [dbo].[Tasks] CHECK CONSTRAINT [FK_Tasks_Tasks]
GO
ALTER TABLE [dbo].[Tasks]  WITH CHECK ADD  CONSTRAINT [FK_Tasks_Project] FOREIGN KEY([ProjectId])
REFERENCES [dbo].[Project] ([Id])
GO

ALTER TABLE [dbo].[Tasks] CHECK CONSTRAINT [FK_Tasks_Project]
GO
ALTER TABLE [dbo].[Tasks]  WITH CHECK ADD  CONSTRAINT [FK_Tasks_PriorityLevel] FOREIGN KEY([PriorityId])
REFERENCES [dbo].[PriorityLevel] ([Id])
GO

ALTER TABLE [dbo].[Tasks] CHECK CONSTRAINT [FK_Tasks_PriorityLevel]
GO
ALTER TABLE [dbo].[Tasks]  WITH CHECK ADD  CONSTRAINT [FK_Tasks_AspNetUsers2] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[AspNetUsers] ([UserId])
GO

ALTER TABLE [dbo].[Tasks] CHECK CONSTRAINT [FK_Tasks_AspNetUsers2]
GO
ALTER TABLE [dbo].[Tasks]  WITH CHECK ADD  CONSTRAINT [FK_Tasks_AspNetUsers1] FOREIGN KEY([AssignedFor])
REFERENCES [dbo].[AspNetUsers] ([UserId])
GO

ALTER TABLE [dbo].[Tasks] CHECK CONSTRAINT [FK_Tasks_AspNetUsers1]
GO
ALTER TABLE [dbo].[Tasks]  WITH CHECK ADD  CONSTRAINT [FK_Tasks_AspNetUsers] FOREIGN KEY([AssignedBy])
REFERENCES [dbo].[AspNetUsers] ([UserId])
GO

ALTER TABLE [dbo].[Tasks] CHECK CONSTRAINT [FK_Tasks_AspNetUsers]