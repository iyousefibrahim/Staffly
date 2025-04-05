using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Staffly.DAL.Dtos
{
    public class UpdateDepartmentDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Code Is Required!")]
        public string Code { get; set; }
        [Required(ErrorMessage = "Name Is Required!")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Date Is Required!")]
        public DateTime CreateAt { get; set; }
    }
}
