using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Mailer.Tests;
internal static class Utilities
{
    /// <summary>
    /// Creates a localdb connection string for SQL Server based on the entry
    /// assembly's GUID and <typeparamref name="T"/>'s namespace-qualified name.
    /// </summary>
    /// <typeparam name="T">
    /// The type to use a namespace-qualified name from.
    /// This should be the test class that the connection string is used in.
    /// </typeparam>
    /// <returns>The connection string for the given class.</returns>
    public static string GetTestConnectionString<T>()
    {
        var type = typeof(T);
        if (type.Assembly != Assembly.GetExecutingAssembly())
        {
            // This check is to discourage non-test types from being passed.
            throw new ArgumentException(
                "Type argument T should be in the same assembly as TestConnectionString.",
                nameof(T));
        }

        string projectGuid = Assembly
            .GetEntryAssembly()
            !.GetCustomAttribute<GuidAttribute>()
            !.Value;
        return $"Server=(localdb)\\MSSQLLocalDB;Database=Mailer.Tests-{projectGuid}-{type.FullName}";
    }
}
