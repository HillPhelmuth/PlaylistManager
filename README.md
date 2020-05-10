# Playlist Manager
### Search for videos using the YouTube Data v3 API and organize the videos into simple, easy to organize playlists. 
- Business layer uses Blazor Server (.net core 3.1)
- Data layer currently uses Azure SQL Databases with Entity Framework for both Identity and Playlist data, but is also set up to use liteDb for Playlist data. Supports download and upload of playlists excel files to share playlists.
- Front end uses razor components. Blazor Server with Signal R creates a responsive front end without much Javascript. A little Javascript is required for YouTube iframe api, but is handled using JSInterop.
- Try the [PlaylistManager Demo](https://playlistmanager20200328074053.azurewebsites.net)
