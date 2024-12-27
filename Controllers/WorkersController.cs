using Microsoft.AspNetCore.Mvc;
using YaroffShop.Models;
using System.Data.SqlClient;

namespace YaroffShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkersController : Controller
    {
        private readonly string connectionString = "Data Source=YAROFF\\SQLEXPRESS;Initial Catalog=Shop;Integrated Security=True";

        public List<Workers> workersList = new List<Workers>();

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM Workers";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                workersList.Add(new Workers
                                {
                                    WORKERS_ID = reader.GetInt32(0),
                                    NAME = reader.GetString(1),
                                    ADRESS = reader.GetString(2),
                                    DEPT_ID = reader.GetDecimal(3),
                                    INFORMATION = reader.GetString(4)
                                });
                            }
                        }
                    }
                }

                return Ok(workersList);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}