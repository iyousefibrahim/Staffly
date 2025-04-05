using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Staffly.DAL.Dtos
{
    public class UpdateEmployeeDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string Name { get; set; }

        [Range(18, 100, ErrorMessage = "Age must be between 18 and 100.")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }

        [StringLength(250, ErrorMessage = "Address cannot exceed 250 characters.")]
        public string Address { get; set; }

        [Phone(ErrorMessage = "Invalid phone number.")]
        public string Phone { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Salary must be greater than 0.")]
        public decimal Salary { get; set; }

        public bool isActive { get; set; }

        public bool isDeleted { get; set; }

        [Required(ErrorMessage = "Hiring Date is required.")]
        [DataType(DataType.Date, ErrorMessage = "Invalid Hiring Date format.")]
        public DateTime HiringDate { get; set; }

        [Required(ErrorMessage = "Creation Date is required.")]
        public DateTime CreateAt { get; set; }
        [Required(ErrorMessage = "Department ID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Invalid Department ID.")]
        public int DepartmentId { get; set; }
    }
}
