ALTER TABLE [dbo].[Tasks]  WITH CHECK ADD  CONSTRAINT [FK_Tasks_PriorityLevel] FOREIGN KEY([PriorityId])
REFERENCES [dbo].[PriorityLevel] ([Id])
GO

ALTER TABLE [dbo].[Tasks] CHECK CONSTRAINT [FK_Tasks_PriorityLevel]