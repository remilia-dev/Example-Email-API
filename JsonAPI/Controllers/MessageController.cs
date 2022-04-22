using BaseAPI.Model;
using JsonApiDotNetCore.Configuration;
using JsonApiDotNetCore.Controllers;
using JsonApiDotNetCore.Controllers.Annotations;
using JsonApiDotNetCore.Services;
using Microsoft.AspNetCore.Mvc;

namespace JsonAPI.Controllers
{
    public sealed class MessageController : JsonApiQueryController<Message, int>
    {
        public MessageController(IJsonApiOptions options,
            IResourceGraph resourceGraph,
            ILoggerFactory loggerFactory,
            IResourceService<Message, int> resourceService
            ) : base(options, resourceGraph, loggerFactory, resourceService) { }
    }
}
