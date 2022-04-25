using Mailer.Core.Data;
using Mailer.Core.Model;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace Mailer.OData.Controllers;

public class MessagesController : ODataController
{
    private readonly EmailDbContext _db;
    public MessagesController(EmailDbContext db)
    {
        _db = db;
    }

    [EnableQuery]
    public IQueryable<EmailMessage> Get() => _db.Messages;

    [EnableQuery]
    public SingleResult<EmailMessage> GetEmailMessage([FromODataUri] int key)
        => SingleResult.Create(_db.Messages.Where(m => m.Id == key));
}
