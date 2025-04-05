using AutoMapper;
using Staffly.DAL.Dtos;
using Staffly.DAL.Models;

namespace Staffly.PL.Mapping
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<CreateEmployeeDto,Employee>().ReverseMap();
            CreateMap<UpdateEmployeeDto, Employee>().ReverseMap();

            CreateMap<CreateDepartmentDto,Department>().ReverseMap();
            CreateMap<UpdateDepartmentDto, Department>().ReverseMap();
        }
    }
}
