﻿ALTER TABLE dbo.VideosData ADD RateScore INT NULL

ALTER TABLE dbo.VideosData ADD CONSTRAINT [FK_25] FOREIGN KEY ([User_ID]) REFERENCES [dbo].[UserData] ([ID])