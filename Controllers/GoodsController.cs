using Microsoft.AspNetCore.Mvc;
using YaroffShop.Models;
using System.Data.SqlClient;

namespace YaroffShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GoodsController : Controller
    {
        private readonly string connectionString = "Data Source=YAROFF\\SQLEXPRESS;Initial Catalog=Shop;Integrated Security=True";

        public List<Goods> goodsList = new List<Goods>();

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM Goods";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                goodsList.Add(new Goods
                                {
                                    GOOD_ID = reader.GetInt32(0),
                                    NAME = reader.GetString(1),
                                    PRICE = reader.GetDouble(2),
                                    QUANTITY = reader.GetInt32(3),
                                    PRODUCER = reader.GetString(4),
                                    DEPT_ID = reader.GetDecimal(5),
                                    DESCRIPTION = reader.GetString(6)
                                });
                            }
                        }
                    }
                }

                return Ok(goodsList);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("LessThanAvg")]
        public IActionResult GetRowsLessThanAvg()
        {
            try
            {
                int rowCount;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("SELECT dbo.LESS_THAN_AVG()", connection))
                    {
                        rowCount = (int)command.ExecuteScalar();
                    }
                }

                return Ok(new { RowsLessThanAvg = rowCount });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("LessThanAvg_v2")]
        public IActionResult GetRowsLessThanAvg_v2()
        {
            try
            {
                int rowCount;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("GetRowsLessThanAvg_v2", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                rowCount = reader.GetInt32(0);
                            }
                            else
                            {
                                throw new Exception("Unexpected result from the procedure.");
                            }
                        }
                    }
                }

                return Ok(new { RowsLessThanAvg = rowCount });
            }
            catch (SqlException sqlEx)
            {
                if (sqlEx.Number == 50001) // Код помилки, заданий у THROW
                {
                    return BadRequest(new { Error = "Table Goods is empty or does not exist." });
                }

                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Error = "Database error occurred.",
                    Details = sqlEx.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Error = "An unexpected error occurred.",
                    Details = ex.Message
                });
            }
        }

        [HttpGet("GoodsLessThanAvg")]
        public IActionResult GetGoodsLessThanAvgPrice()
        {
            try
            {
                List<dynamic> goods = new List<dynamic>();

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("SELECT * FROM dbo.GetGoodsLessThanAvgPrice()", connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                goods.Add(new 
                                {
                                    GOOD_ID = reader["GOOD_ID"],
                                    NAME = reader["NAME"],
                                    PRICE = reader["PRICE"],
                                    DESCRIPTION = reader["DESCRIPTION"]
                                });
                            }
                        }
                    }
                }

                return Ok(goods);
            }
            catch (SqlException sqlEx)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Error = "Database error occurred.",
                    Details = sqlEx.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Error = "An unexpected error occurred.",
                    Details = ex.Message
                });
            }
        }


        [HttpGet("CountGoods")]
        public IActionResult GetCountGoodsCheaperThan([FromQuery] decimal price)
        {
            try
            {
                int goodsCount;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("SELECT dbo.count_goods(@new_price)", connection))
                    {
                        command.Parameters.AddWithValue("@new_price", price);

                        goodsCount = (int)command.ExecuteScalar();
                    }
                }

                return Ok(new { GoodsCheaperThanPrice = goodsCount });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

    }
}