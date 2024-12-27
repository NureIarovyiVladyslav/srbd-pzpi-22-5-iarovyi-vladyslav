using Microsoft.AspNetCore.Mvc;
using YaroffShop.Models;
using System.Data.SqlClient;
using System.Security.Cryptography.X509Certificates;

namespace YaroffShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : Controller
    {
        private readonly string connectionString = "Data Source=YAROFF\\SQLEXPRESS;Initial Catalog=Shop;Integrated Security=True";

        public List<Departments> departmentsList = new List<Departments>();

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM Departments";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                departmentsList.Add(new Departments
                                {
                                    DEPT_ID = reader.GetDecimal(0),
                                    NAME = reader.GetString(1),
                                    INFO = reader.GetString(2)
                                });
                            }
                        }
                    }
                }

                return Ok(departmentsList);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("AddDepartment")]
        public IActionResult Post([FromForm] Departments department)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Call PostBody method to handle the actual insertion logic
            return PostBody(department);
        }

        [HttpPost("AddDepartmentBody")]
        public IActionResult PostBody([FromBody] Departments department)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("usp_insert_depd", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;

                        // Add parameters
                        command.Parameters.AddWithValue("@new_id", department.DEPT_ID);
                        command.Parameters.AddWithValue("@new_name", department.NAME);
                        command.Parameters.AddWithValue("@new_info", department.INFO);

                        // Execute the stored procedure
                        command.ExecuteNonQuery();
                    }
                }

                return CreatedAtAction(nameof(Get), new { id = department.DEPT_ID }, department);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        
    }
}
