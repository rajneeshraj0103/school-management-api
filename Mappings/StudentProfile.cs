using AutoMapper;
using School_Management.DTOs;
using School_Management.Entities;

namespace School_Management.Mappings
{
    public class StudentProfile : Profile
    {
        public StudentProfile()
        {
            CreateMap<CreateStudentDto, Student>();

            CreateMap<Student, StudentResponseDto>();

            CreateMap<UpdateStudentDto, Student>();
        }
    }
}
