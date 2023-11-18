using DotNetBackend.Data.DTO;
using DotNetBackend.Data.Responses;
using FluentValidation;

namespace DotNetBackend.Data.Requests
{
    public class GetPlaylistVideosRequest : RequestBase, IGetPlaylistDataRequest
    {
        public string Id { get; set; }
        public string Url { get; set; }
    }

    public class GetPlaylistVideosRequestValidator : AbstractValidator<GetPlaylistVideosRequest>
    {
        public GetPlaylistVideosRequestValidator()
        {
            Include(new GetPlaylistDataRequestValidator());
        }
    }
}
