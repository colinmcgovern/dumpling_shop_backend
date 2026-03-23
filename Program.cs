using Azure.Core;
using Azure.Identity;
using Microsoft.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
var app = builder.Build();

// Establish database connection at startup using Azure Managed Identity
var connectionString = "Server=tcp:colinmcgovern2.database.windows.net;Database=dumplings;Encrypt=True;";
SqlConnection? conn = null;
SqlDataReader? rdr = null;

try
{
    var credential = new DefaultAzureCredential();
    var tokenRequestContext = new TokenRequestContext(new[] { "https://database.windows.net/.default" });
    var accessToken = await credential.GetTokenAsync(tokenRequestContext);

    conn = new SqlConnection(connectionString);
    conn.AccessToken = accessToken.Token;
    await conn.OpenAsync();

    var cmd = new SqlCommand("SELECT TOP 1 1", conn);
    rdr = await cmd.ExecuteReaderAsync();

    Console.WriteLine("Database connection established successfully.");
}
catch (Exception ex)
{
    Console.WriteLine($"Failed to connect to the database: {ex.Message}");
}
finally
{
    if (rdr != null) await rdr.CloseAsync();
    if (conn != null) await conn.CloseAsync();
}

app.MapControllers();
app.Run();
