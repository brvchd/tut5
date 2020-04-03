using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using tut3.Models;

namespace tut3.Controllers
{
    [Route("api/enrollment")]
    [ApiController]
    public class EnrollmentController : Controller
    {
        [HttpPost]
        public IActionResult AddStudent(Student student)
        {
            return Ok();
        }
    }
}