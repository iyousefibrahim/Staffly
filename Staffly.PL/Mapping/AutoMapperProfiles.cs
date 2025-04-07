using AutoMapper;
using Microsoft.AspNetCore.Identity;
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

            CreateMap<SignUpDto, ApplicationUser>().ReverseMap();
            CreateMap<SignInDto, ApplicationUser>().ReverseMap();

            CreateMap<UpdateUserDto, ApplicationUser>().ReverseMap();
            CreateMap<UserToReturnDto, ApplicationUser>().ReverseMap();

            CreateMap<RoleToRetrurnDto, IdentityRole>().ReverseMap();
        }
    }
}
