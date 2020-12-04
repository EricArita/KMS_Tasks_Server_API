ALTER TABLE [dbo].[Tasks]  WITH CHECK ADD  CONSTRAINT [FK_Tasks_Tasks] FOREIGN KEY([ParentId])
REFERENCES [dbo].[Tasks] ([Id])
GO

ALTER TABLE [dbo].[Tasks] CHECK CONSTRAINT [FK_Tasks_Tasks]