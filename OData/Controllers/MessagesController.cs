using Mailer.Core.Data;
using Mailer.Core.Model;
using Mailer.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace Mailer.OData.Controllers;

public class MessagesController : ODataController
{
    private readonly EmailDbContext _db;
    private readonly IEmailQueue _emailQueue;

    public MessagesController(EmailDbContext db, IEmailQueue emailQueue)
    {
        _db = db;
        _emailQueue = emailQueue;
    }

    [EnableQuery]
    public IQueryable<EmailMessage> Get() => _db.Messages;

    [EnableQuery]
    public SingleResult<EmailMessage> GetEmailMessage([FromODataUri] int key)
        => SingleResult.Create(_db.Messages.Where(m => m.Id == key));

    public async Task<IActionResult> Post([FromBody] NewEmailMessage newEmail)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var email = newEmail.ToEmailMessage();
        await _db.Messages.AddAsync(email);
        await _db.SaveChangesAsync();
        _db.ChangeTracker.Clear();

        await _emailQueue.EnqueueAsync(email);

        return Created(email);
    }
}
