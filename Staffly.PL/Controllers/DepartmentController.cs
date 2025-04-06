using System.Threading.Tasks;
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
        public async Task<IActionResult> Index()
        {
            var departments = await _unitOfWork.DepartmentRepository.GetAllAsync();
            return View(departments);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateDepartmentDto departmentDto)
        {
            if (ModelState.IsValid)
            {
                // Mapping CreateDepartmentDto to Department
                var department = _mapper.Map<Department>(departmentDto);
                await _unitOfWork.DepartmentRepository.AddAsync(department);
                // Save changes to the database
                await _unitOfWork.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(departmentDto);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if(id is null)
            {
                return BadRequest("Invalid Id!");
            }

            var department = await _unitOfWork.DepartmentRepository.GetByIdAsync(id.Value);

            if(department is null)
            {
                return NotFound(new {statusCode = 404, message = $"Department With Id:{id.Value} Is Not Found!"});
            }

            return View(department);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int? id)
        {
            if (id is null)
            {
                return BadRequest("Invalid Id!");
            }

            var department = await _unitOfWork.DepartmentRepository.GetByIdAsync(id.Value);

            if (department is null)
            {
                return NotFound(new { statusCode = 404, message = $"Department With Id:{id.Value} Is Not Found!" });
            }

            var departmentDto = _mapper.Map<UpdateDepartmentDto>(department);
            return View(departmentDto);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update([FromRoute]int id,UpdateDepartmentDto departmentDto)
        {
            if (ModelState.IsValid)
            {
                var department = _mapper.Map<Department>(departmentDto);
                _unitOfWork.DepartmentRepository.Update(department);
                // Save changes to the database
                await _unitOfWork.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(departmentDto);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var department = await _unitOfWork.DepartmentRepository.GetByIdAsync(id);
            if (department is null)
            {
                return NotFound(new { statusCode = 404, message = $"Department With Id:{id} Is Not Found!" });
            }
            _unitOfWork.DepartmentRepository.Delete(department);
            // Save changes to the database
            await _unitOfWork.SaveChangesAsync();
            return RedirectToAction("Index");
        }

    }
}
