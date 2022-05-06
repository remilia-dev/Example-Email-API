# Example Email API
This is an example solution to send emails and store/log them in a database.

## Utilizing the Project
You will *need* the following software installed:
* SQL Server (preferably 2019)
* .NET 6 SDK

The following software is recommended:
* Visual Studio
* Postman Desktop

### Configuration
The provided appsettings does not contain the necessary configuration
information to connect to an SMTP server (which is needed to send emails).
Below is an example of what the configuration should look like and an
explaination of each option:
```json5
{
    "SmtpConnection": {
        // SMTP server host (required)
        "Host": "",
        // Port of the SMTP server (usually 25, 465, or 587)
        "Port": 465,
        // Authentication information (omit if the server doesn't require it)
        "Username": "",
        "Password": "",
        // Maximum number of times to attempt sending an email (defaults to 3)
        MaxRetries: 3,
        // Milliseconds to wait before retrying to send an email (defaults to 0)
        MillisecondDelayBetweenRetries: 0,
        // Whether sensitive data (like email contents) is logged.
        // This is *always* false in non-debug builds.
        LogSensitiveData: false
    }
}
```

If you are running a non-development build, you will also need to provide an
SQL Server connection string. Don't forget to migrate the database as
migrations do not occur automatically in production builds.

### Running

You can either use Visual Studio or the following command to run the Development
build:
```ps
dotnet run --project OData --launch-profile ODataAPI
```

### API Requests

You can import the example [postman collection](API%20Examples.postman_collection.json)
into postman to experiment with the API. Here is a quick summary of the API:

| HTTP Verb | API Path             | Action                                   |
|-----------|----------------------|------------------------------------------|
| GET       | `api/v1/Messages`    | Gets all email messages (paginated)      |
| GET       | `api/v1/Messages(1)` | Gets the email message with an Id of `1` |
| POST      | `api/v1/Messages`    | Queues an email to be sent               |

Note that posting only *queues* an email to be sent. The background service will
connect and send emails in the queue as they are added. The JSON that is returned
upon posting contains an Id that can be used to look up if an email has
successfully sent.

## License
This project is licensed under GPLv3 or later. See [LICENSE](LICENSE).
