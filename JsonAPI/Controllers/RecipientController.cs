using BaseAPI.Model;
using JsonApiDotNetCore.Configuration;
using JsonApiDotNetCore.Controllers;
using JsonApiDotNetCore.Services;

namespace JsonAPI.Controllers
{
    public sealed class RecipientController : JsonApiQueryController<Recipient, int>
    {
        public RecipientController(IJsonApiOptions options,
            IResourceGraph resourceGraph,
            ILoggerFactory loggerFactory,
            IResourceService<Recipient, int> resourceService
            ) : base(options, resourceGraph, loggerFactory, resourceService) { }
    }
}
