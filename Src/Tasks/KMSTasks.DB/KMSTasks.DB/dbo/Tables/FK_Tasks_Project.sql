ALTER TABLE [dbo].[Tasks]  WITH CHECK ADD  CONSTRAINT [FK_Tasks_Project] FOREIGN KEY([ProjectId])
REFERENCES [dbo].[Project] ([Id])
GO

ALTER TABLE [dbo].[Tasks] CHECK CONSTRAINT [FK_Tasks_Project]