using FluentValidation;
using System;

namespace DotNetBackend.Data.Requests
{
    public class GetPlaylistDataRequest
    {
        public string Id { get; set; }
        public string Url { get; set; }
    }

    public class GetPlaylistDataRequestValidator : AbstractValidator<GetPlaylistDataRequest>
    {
        public GetPlaylistDataRequestValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Stop;

            RuleFor(r => r.Id)
                .NotEmpty().When(r => r.Url == null)
                .WithMessage("Playlist URL or ID is required.");

            RuleFor(r => r.Url)
                .NotEmpty()
                .WithMessage("Playlist URL or ID is required.")
                .Must(url => Uri.TryCreate(url, UriKind.Absolute, out _))
                .When(r => r.Id == null);
        }
    }
}
