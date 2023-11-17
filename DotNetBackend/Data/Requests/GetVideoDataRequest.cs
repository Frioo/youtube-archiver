using FluentValidation;
using System;

namespace DotNetBackend.Data.Requests
{
    public class GetVideoDataRequest
    {
        public string Id { get; set; }
        public string Url { get; set; }
    }

    public class GetVideoDataRequestValidator : AbstractValidator<GetVideoDataRequest>
    {
        public GetVideoDataRequestValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Stop;

            RuleFor(r => r.Id)
                .NotEmpty().When(r => r.Url == null)
                .WithMessage("Video URL or ID is required.")
                .Length(11)
                .WithMessage("Video ID is invalid.");

            RuleFor(r => r.Url)
                .NotEmpty()
                .WithMessage("Video URL or ID is required.")
                .Must(url => Uri.TryCreate(url, UriKind.Absolute, out _))
                .When(r => r.Id == null);
        }
    }
}
