using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Staffly.BLL.Interfaces;
using Staffly.DAL.Dtos;
using Staffly.DAL.Models;

namespace Staffly.PL.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public EmployeeController(IMapper mapper,IUnitOfWork unitOfWork)
        {
            this._mapper = mapper;
            this._unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult Index(string? SearchInput)
        {
            IEnumerable<Employee> employees;
            if (string.IsNullOrEmpty(SearchInput))
            {
                employees = _unitOfWork.EmployeeRepository.GetAll();
            }
            else
            {
                employees = _unitOfWork.EmployeeRepository.FindByName(SearchInput);
            }
            return View(employees);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var departments = _unitOfWork.DepartmentRepository.GetAll();
            ViewData["Departments"] = departments;
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateEmployeeDto employeeDto)
        {
            if (ModelState.IsValid)
            {
                // Map DTO to Entity
                var employee = _mapper.Map<Employee>(employeeDto);
                _unitOfWork.EmployeeRepository.Add(employee);
                // Save changes to the database
                _unitOfWork.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(employeeDto);
        }

        [HttpGet]
        public IActionResult Details(int? id)
        {
            if (id is null)
            {
                return BadRequest("Invalid Id!");
            }

            var employee = _unitOfWork.EmployeeRepository.GetById(id.Value);

            if (employee is null)
            {
                return NotFound(new { statusCode = 404, message = $"Employee With Id:{id.Value} Is Not Found!" });
            }
            var departments = _unitOfWork.DepartmentRepository.GetAll();
            ViewData["Departments"] = departments;

            return View(employee);
        }

        [HttpGet]
        public IActionResult Update(int? id)
        {
            if (id is null)
            {
                return BadRequest("Invalid Id!");
            }

            var employee = _unitOfWork.EmployeeRepository.GetById(id.Value);

            if (employee is null)
            {
                return NotFound(new { statusCode = 404, message = $"Employee With Id:{id.Value} Is Not Found!" });
            }
            var employeeDto = _mapper.Map<UpdateEmployeeDto>(employee);
            var departments = _unitOfWork.DepartmentRepository.GetAll();
            ViewData["Departments"] = departments;

            return View(employeeDto);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update([FromRoute] int id, UpdateEmployeeDto employeeDto)
        {
            if (ModelState.IsValid)
            {
                // Map DTO to Entity
                 var employee = _mapper.Map<Employee>(employeeDto);

                _unitOfWork.EmployeeRepository.Update(employee);
                // Save changes to the database
                _unitOfWork.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(employeeDto);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var employee = _unitOfWork.EmployeeRepository.GetById(id);
            if (employee is null)
            {
                return NotFound(new { statusCode = 404, message = $"Employee With Id:{id} Is Not Found!" });
            }
            _unitOfWork.EmployeeRepository.Delete(employee);
            // Save changes to the database
            _unitOfWork.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
