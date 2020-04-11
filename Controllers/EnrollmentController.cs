using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using tut3.DAL;
using tut3.DTOs.Requests;
using tut3.Models;

namespace tut3.Controllers
{
    [Route("api/enrollment")]
    [ApiController]
    public class EnrollmentController : ControllerBase
    {
        private readonly IDbService _dbService;

        public EnrollmentController(IDbService dbService)
        {
            _dbService = dbService;
        }

        [HttpPost]
        public IActionResult EnrollStudent(EnrollStudentRequest request)
        {
            using (var con = new SqlConnection("Data Source=db-mssql;Initial Catalog=s18963;Integrated Security=True"))
            {
                using (var com = new SqlCommand())
                {
                    com.Connection = con;
                    com.CommandText = "Select * From Studies Where Name = @Name";
                    com.Parameters.AddWithValue("Name", request.Studies);
                    con.Open();

                    var trans = con.BeginTransaction();
                    com.Transaction = trans;
                    var dr = com.ExecuteReader();

                    if (!dr.Read())
                    {
                        dr.Close();
                        trans.Rollback();
                        return BadRequest("Specified studies does not exist");
                    }

                    int idStudy = (int)dr["IdStudy"];

                }
            }
            return Ok();
        }
    }
}