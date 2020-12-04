ALTER TABLE [dbo].[Tasks]  WITH CHECK ADD  CONSTRAINT [FK_Tasks_AspNetUsers] FOREIGN KEY([AssignedBy])
REFERENCES [dbo].[AspNetUsers] ([UserId])
GO

ALTER TABLE [dbo].[Tasks] CHECK CONSTRAINT [FK_Tasks_AspNetUsers]