using ASK.Shared.Interfaces;
using FastEndpoints;

namespace ASK.Api.Endpoints
{
    public class FirstEndPoint : EndpointWithoutRequest
    {
        readonly IAppService _service;

        public FirstEndPoint(IAppService service)
        {
            _service = service;
        }

        public override void Configure()
        {
            Verbs(Http.GET);
            Routes("test/first");
            DontThrowIfValidationFails();
            AllowAnonymous();
        }

        public override async Task HandleAsync(CancellationToken cancellationToken)
        {
            await SendErrorsAsync();
        }
    }
}
