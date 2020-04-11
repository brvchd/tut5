using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using tut3.DAL;
using tut3.DTOs.Requests;
using tut3.DTOs.Responces;
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
            var response = new EnrollStudentResponce();
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

                    dr.Close();

                    com.CommandText = "Select * From Enrollment Where Semester = 1 And IdStudy = @idStudy";
                    int IdEnrollment = (int)dr["IdEnrollemnt"] + 1;
                    com.Parameters.AddWithValue("IdStudy", idStudy);
                    dr = com.ExecuteReader();
                    
                    if(dr.Read())
                    {
                        dr.Close();
                        com.CommandText = "Select MAX(idEnrollment) as 'idEnrollment' From Enrollment";
                        dr = com.ExecuteReader();
                        dr.Close();
                        DateTime StartDate = DateTime.Now;
                        com.CommandText = "Insert Into Enrollment(IdEnrollment, Semester, IdStudy, StartDate) Values (@IdEnrollemnt, 1, @IdStudy, @StartDate)";
                        com.Parameters.AddWithValue("IdEnrollemnt", IdEnrollment);
                        com.Parameters.AddWithValue("StartDate", StartDate);
                        com.ExecuteNonQuery();
                    }

                    dr.Close();

                    com.CommandText = "Select * From Student Where IndexNumber=@IndexNumber";
                    com.Parameters.AddWithValue("IndexNumber", request.IndexNumber);
                    dr = com.ExecuteReader();
                    
                    if (dr.Read())
                    {
                        dr.Close();
                        trans.Rollback();
                        return BadRequest("You can't add student with the same index number");
                    }
                    else
                    {
                        dr.Close();
                        com.CommandText = "Insert Into Student(IndexNumber, FirstName, LastName, Birthdate, IdEnrollment) Value (@IndexNumber, @FirstName, @LastName, @BirthDate, @IdEnrollment)";
                        com.Parameters.AddWithValue("FirstName", request.FirstName);
                        com.Parameters.AddWithValue("LastName", request.LastName);
                        com.Parameters.AddWithValue("BirthDate", request.BirthDate);
                        com.Parameters.AddWithValue("IdEnrollment", IdEnrollment);
                        com.ExecuteNonQuery();
                        dr.Close();

                        response.FirstName = request.FirstName;
                        response.LastName = request.LastName;
                        response.BirthDate = request.LastName;
                        response.Studies = request.Studies;
                        response.Semester = 1;
                    }

                    trans.Commit();
                }
            }
            
            return Ok(response);
        }
    }
}