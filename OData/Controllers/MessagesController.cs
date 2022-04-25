using Mailer.Core.Data;
using Mailer.Core.Model;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace Mailer.OData.Controllers;

public class MessagesController : ODataController
{
    private readonly MessageDbContext _db;
    public MessagesController(MessageDbContext db)
    {
        _db = db;
    }

    [EnableQuery]
    public IQueryable<Message> Get() => _db.Messages;

    [EnableQuery]
    public SingleResult<Message> GetMessage([FromODataUri] int key)
        => SingleResult.Create(_db.Messages.Where(m => m.Id == key));
}
