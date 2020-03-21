CREATE TABLE [dbo].[VideosData] (
    [ID]           INT           IDENTITY (1, 1) NOT NULL,
    [Title]        VARCHAR (MAX) NOT NULL,
    [VideoID]      VARCHAR (MAX) NOT NULL,
    [Playlist_ID]  INT           NOT NULL,
    [PreferenceID] INT           NULL,
    CONSTRAINT [PK_table_19] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_24] FOREIGN KEY ([Playlist_ID]) REFERENCES [dbo].[PlaylistData] ([ID])
);


GO
CREATE NONCLUSTERED INDEX [fkIdx_24]
    ON [dbo].[VideosData]([Playlist_ID] ASC);