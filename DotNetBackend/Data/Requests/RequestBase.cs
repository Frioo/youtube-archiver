using FluentValidation;

namespace DotNetBackend.Data.Requests
{
    public class RequestBase : IPagingParameters
    {
        public int Page { get; set; } = 0;
        public int PageSize { get; set; } = 50;
        public int Offset => Page * PageSize;

        public RequestBase()
        {

        }

        public RequestBase(IPagingParameters pagingParameters)
        {
            Page = pagingParameters.Page;
            PageSize = pagingParameters.PageSize;
        }
    }

    public class RequestBaseValidator : AbstractValidator<RequestBase>
    {
        public RequestBaseValidator()
        {
            RuleFor(r => r.Page)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Page number cannot be negative.");

            RuleFor(r => r.PageSize)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Page size cannot be negative.");
        }
    }
}
