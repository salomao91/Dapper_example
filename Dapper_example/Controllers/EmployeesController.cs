using Dapper;
using Dapper_example.Entites;
using Dapper_example.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Dapper_example.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly string _connectionString;
        public EmployeesController(IConfiguration connectionString)
        {
            _connectionString = connectionString.GetConnectionString("Dapper_example");
        }

        // GET: api/<EmployeesController>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                const string sql = "SELECT * FROM EmployeeTb WHERE IsActive = 1";
                var employees = await sqlConnection.QueryAsync<EmployeeEntite>(sql);
                return Ok(employees);
            }
        }

        // GET api/<EmployeesController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var parameters = new
            {
                id
            };

            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                const string sql = "SELECT * FROM EmployeeTb WHERE Id = @Id";

                var employee = await sqlConnection.QuerySingleOrDefaultAsync<EmployeeEntite>(sql, parameters);

                if (employee == null)
                {
                    return NotFound();
                }
                return Ok(employee);
            }
        }

        // POST api/<EmployeesController>
        [HttpPost]
        public async Task<IActionResult> Post(EmployeeModel model)
        {
            var employee = new EmployeeEntite(model.Fullname, model.Birthdate, model.Salary, model.Position);

            var parameters = new
            {
                employee.Fullname,
                employee.Birthdate,
                employee.Salary,
                employee.Position,
                employee.IsActive
            };

            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                const string sql = "INSERT INTO EmployeeTb OUTPUT INSERTED.Id VALUES (@Fullname, @Birthdate, @Salary, @Position, @IsActive)";

                var id = await sqlConnection.ExecuteScalarAsync<int>(sql, parameters);

                return Ok(id);
            }
        }

        // PUT api/<EmployeesController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, EmployeeModel model) 
        {
            var parameters = new
            {
                id,
                model.Fullname,
                model.Birthdate,
                model.Salary,
                model.Position
            };

            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                const string sql = "UPDATE EmployeeTb SET Fullname = @Fullname, Birthdate = @Birthdate, Salary = @Salary, Position = @Position WHERE Id = @id";

                await sqlConnection.ExecuteAsync(sql, parameters);

                return NoContent();
            }

        }

        // DELETE api/<EmployeesController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var parameters = new
            {
                id
            };

            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                const string sql = "UPDATE EmployeeTb SET IsActive = 0 WHERE Id = @id ";

                await sqlConnection.ExecuteAsync(sql, parameters);
            }

            return NoContent();
        }
    }
}
