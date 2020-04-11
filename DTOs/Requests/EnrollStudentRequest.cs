using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace tut3.DTOs.Requests
{
    public class EnrollStudentRequest
    {
        [Required]
        [MaxLength(100)]
        [RegularExpression("^s[0-9]+$")]
        public string IndexNumber { get; set; }
        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(100)]
        public string LastName { get; set; }
        [Required]
        public string BirthDate { get; set; }
        [Required]
        public string Studies { get; set; }
    }
}
