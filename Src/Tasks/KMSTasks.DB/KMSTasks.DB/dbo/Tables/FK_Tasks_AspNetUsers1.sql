ALTER TABLE [dbo].[Tasks]  WITH CHECK ADD  CONSTRAINT [FK_Tasks_AspNetUsers1] FOREIGN KEY([AssignedFor])
REFERENCES [dbo].[AspNetUsers] ([UserId])
GO

ALTER TABLE [dbo].[Tasks] CHECK CONSTRAINT [FK_Tasks_AspNetUsers1]