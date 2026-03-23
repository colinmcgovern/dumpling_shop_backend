namespace Lab01.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Azure.Core;
    using Azure.Identity;
    using Lab01.Models;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Data.SqlClient;

    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly DefaultAzureCredential _credential;
        private const string ConnectionString = "Server=tcp:colinmcgovern2.database.windows.net;Database=dumplings;Encrypt=True;";

        public OrdersController(DefaultAzureCredential credential)
        {
            _credential = credential;
        }

        [HttpGet("list_orders")]
        public async Task<ActionResult<IEnumerable<Order>>> ListOrders()
        {
            var orders = new List<Order>();
            var tokenRequestContext = new TokenRequestContext(new[] { "https://database.windows.net/.default" });
            var accessToken = await _credential.GetTokenAsync(tokenRequestContext);

            using var conn = new SqlConnection(ConnectionString);
            conn.AccessToken = accessToken.Token;
            await conn.OpenAsync();

            using var cmd = new SqlCommand("SELECT * FROM [dbo].[Orders]", conn);
            using var rdr = await cmd.ExecuteReaderAsync();

            while (await rdr.ReadAsync())
            {
                orders.Add(new Order
                {
                    OrderId = (int)rdr["orderId"],
                    OrderName = (string)rdr["orderName"],
                    OrderPlaced = (DateTime)rdr["orderPlaced"],
                    EstimatedCompleted = (DateTime)rdr["estimatedCompleted"],
                    IsCompleted = (bool)rdr["isCompleted"],
                    IsPickedUp = (bool)rdr["isPickedUp"],
                    NumDumplings = (int)rdr["num_dumplings"],
                    NumTeas = (int)rdr["num_teas"]
                });
            }

            return Ok(orders);
        }

        [HttpPost("insert_order")]
        public async Task<ActionResult> InsertOrder([FromBody] NewOrder order)
        {
            var tokenRequestContext = new TokenRequestContext(new[] { "https://database.windows.net/.default" });
            var accessToken = await _credential.GetTokenAsync(tokenRequestContext);

            using var conn = new SqlConnection(ConnectionString);
            conn.AccessToken = accessToken.Token;
            await conn.OpenAsync();

            using var cmd = new SqlCommand(
                "INSERT INTO [dbo].[Orders] (orderName, orderPlaced, estimatedCompleted, isCompleted, isPickedUp, num_dumplings, num_teas) " +
                "VALUES (@orderName, SYSDATETIME(), @estimatedCompleted, @isCompleted, @isPickedUp, @numDumplings, @numTeas)", conn);

            cmd.Parameters.AddWithValue("@orderName", order.OrderName);
            cmd.Parameters.AddWithValue("@estimatedCompleted", order.EstimatedCompleted);
            cmd.Parameters.AddWithValue("@isCompleted", order.IsCompleted);
            cmd.Parameters.AddWithValue("@isPickedUp", order.IsPickedUp);
            cmd.Parameters.AddWithValue("@numDumplings", order.NumDumplings);
            cmd.Parameters.AddWithValue("@numTeas", order.NumTeas);

            await cmd.ExecuteNonQueryAsync();

            return Ok();
        }

        [HttpPost("update_order")]
        public async Task<ActionResult> UpdateOrder([FromBody] Order order)
        {
            var tokenRequestContext = new TokenRequestContext(new[] { "https://database.windows.net/.default" });
            var accessToken = await _credential.GetTokenAsync(tokenRequestContext);

            using var conn = new SqlConnection(ConnectionString);
            conn.AccessToken = accessToken.Token;
            await conn.OpenAsync();

            using var cmd = new SqlCommand(
                "UPDATE [dbo].[Orders] " +
                "SET orderName = @orderName, estimatedCompleted = @estimatedCompleted, " +
                "    isCompleted = @isCompleted, isPickedUp = @isPickedUp, " +
                "    num_dumplings = @numDumplings, num_teas = @numTeas " +
                "WHERE orderId = @orderId", conn);

            cmd.Parameters.AddWithValue("@orderId", order.OrderId);
            cmd.Parameters.AddWithValue("@orderName", order.OrderName);
            cmd.Parameters.AddWithValue("@estimatedCompleted", order.EstimatedCompleted);
            cmd.Parameters.AddWithValue("@isCompleted", order.IsCompleted);
            cmd.Parameters.AddWithValue("@isPickedUp", order.IsPickedUp);
            cmd.Parameters.AddWithValue("@numDumplings", order.NumDumplings);
            cmd.Parameters.AddWithValue("@numTeas", order.NumTeas);

            await cmd.ExecuteNonQueryAsync();

            return Ok();
        }
    }
}
