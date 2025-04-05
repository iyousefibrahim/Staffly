using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Staffly.BLL.Interfaces;
using Staffly.DAL.Dtos;
using Staffly.DAL.Models;

namespace Staffly.PL.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DepartmentController(IUnitOfWork unitOfWork,IMapper mapper)
        {
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var departments = _unitOfWork.DepartmentRepository.GetAll();
            return View(departments);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateDepartmentDto departmentDto)
        {
            if (ModelState.IsValid)
            {
                // Mapping CreateDepartmentDto to Department
                var department = _mapper.Map<Department>(departmentDto);
                _unitOfWork.DepartmentRepository.Add(department);
                // Save changes to the database
                _unitOfWork.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(departmentDto);
        }

        [HttpGet]
        public IActionResult Details(int? id)
        {
            if(id is null)
            {
                return BadRequest("Invalid Id!");
            }

            var department = _unitOfWork.DepartmentRepository.GetById(id.Value);

            if(department is null)
            {
                return NotFound(new {statusCode = 404, message = $"Department With Id:{id.Value} Is Not Found!"});
            }

            return View(department);
        }

        [HttpGet]
        public IActionResult Update(int? id)
        {
            if (id is null)
            {
                return BadRequest("Invalid Id!");
            }

            var department = _unitOfWork.DepartmentRepository.GetById(id.Value);

            if (department is null)
            {
                return NotFound(new { statusCode = 404, message = $"Department With Id:{id.Value} Is Not Found!" });
            }

            var departmentDto = _mapper.Map<UpdateDepartmentDto>(department);
            return View(departmentDto);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update([FromRoute]int id,UpdateDepartmentDto departmentDto)
        {
            if (ModelState.IsValid)
            {
                var department = _mapper.Map<Department>(departmentDto);
                _unitOfWork.DepartmentRepository.Update(department);
                // Save changes to the database
                _unitOfWork.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(departmentDto);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var department = _unitOfWork.DepartmentRepository.GetById(id);
            if (department is null)
            {
                return NotFound(new { statusCode = 404, message = $"Department With Id:{id} Is Not Found!" });
            }
            _unitOfWork.DepartmentRepository.Delete(department);
            // Save changes to the database
            _unitOfWork.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
