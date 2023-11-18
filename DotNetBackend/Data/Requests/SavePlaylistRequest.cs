using FluentValidation;

namespace DotNetBackend.Data.Requests
{
    public class SavePlaylistRequest : GetPlaylistDataRequest
    {
        public bool SaveVideos { get; set; } = true;
    }

    public class SavePlaylistRequestValidator : AbstractValidator<SavePlaylistRequest>
    {
        public SavePlaylistRequestValidator()
        {
            Include(new GetPlaylistDataRequestValidator());
        }
    }
}
