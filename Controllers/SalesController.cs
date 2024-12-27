using Microsoft.AspNetCore.Mvc;
using YaroffShop.Models;
using System.Data.SqlClient;

namespace YaroffShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesController : Controller
    {
        private readonly string connectionString = "Data Source=YAROFF\\SQLEXPRESS;Initial Catalog=Shop;Integrated Security=True";

        public List<Sales> salesList = new List<Sales>();

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM Sales";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                salesList.Add(new Sales
                                {
                                    SALE_ID = reader.GetInt32(0),
                                    CHECK_NO = reader.GetInt32(1),
                                    GOOD_ID = reader.GetInt32(2),
                                    DATE_SALE = reader.GetDateTime(3),
                                    QUANTITY = reader.GetInt32(4)
                                });
                            }
                        }
                    }
                }

                return Ok(salesList);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}