using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using ONE.DTOs;

namespace ONE.Services
{
    public class YouTubeServiceApi
    {
        private readonly string _apiKey = "AIzaSyCOIVuAlAc9ftp186d6_w5WZbJot0dgEQM"; 
        public async Task<YouTubeVideoData> GetVideoDetails(string videoId)
        {
            var youTubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = _apiKey
            });
            var videoReq = youTubeService.Videos.List("snippet");
            videoReq.Id = videoId;
            var response = await videoReq.ExecuteAsync();
            if (response.Items.Count > 0)
            {
                var video = response.Items[0];
                return new YouTubeVideoData
                {
                    Title = video.Snippet.Title,
                    ThumbnailUrl = video.Snippet.Thumbnails.High.Url,
                    Description = video.Snippet.Description
                };
            }

            return null;
        }
    }
}
