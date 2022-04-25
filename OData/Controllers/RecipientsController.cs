using Mailer.Core.Data;
using Mailer.Core.Model;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace Mailer.OData.Controllers;

public class RecipientsController : ODataController
{
    private readonly EmailDbContext _db;
    public RecipientsController(EmailDbContext db)
    {
        _db = db;
    }

    [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.Supported & ~AllowedQueryOptions.Expand)]
    public IQueryable<EmailRecipient> Get() => _db.Recipients;

    [EnableQuery]
    public SingleResult<EmailRecipient> GetEmailRecipient([FromODataUri] int key)
        => SingleResult.Create(_db.Recipients.Where(r => r.Id == key));
}
