using AutoMapper;
using School_Management.DTOs;
using School_Management.Entities;
using School_Management.Helpers;
using School_Management.Interfaces;

namespace School_Management.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;

        private readonly IMapper _mapper;
        public StudentService( IStudentRepository studentRepository,
            IMapper mapper)
        {
            _studentRepository = studentRepository;   
            _mapper = mapper;
        }

        public async Task<PagedResponse<StudentResponseDto>> GetAllStudentsAsync( PaginationParams paginationParams)
        {
            //throw new Exception("Test Exception");
            var pagedStudents = await _studentRepository
                .GetAllStudentsAsync(paginationParams);

            var studentDtos = _mapper.Map<IEnumerable<StudentResponseDto>>(pagedStudents.Data);

            return new PagedResponse<StudentResponseDto>
            {
                PageNumber = pagedStudents.PageNumber,

                PageSize = pagedStudents.PageSize,

                TotalRecords = pagedStudents.TotalRecords,

                TotalPages = pagedStudents.TotalPages,

                Data = studentDtos
            };
        }

        public async Task<StudentResponseDto?> GetStudentByIdAsync(int id)
        {
            var student = await _studentRepository.GetStudentByIdAsync(id);

            if (student == null)
            {
                return null;
            }

            return _mapper.Map<StudentResponseDto>(student);
        }

        public async Task<StudentResponseDto> CreateStudentAsync(CreateStudentDto dto)
        {
            var student = _mapper.Map<Student>(dto);

            student.Password = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            student.CreatedDate = DateTime.UtcNow;

            var createdStudent = await _studentRepository.CreateStudentAsync(student);

            return _mapper.Map<StudentResponseDto> (createdStudent);
        }

        public async Task<StudentResponseDto?> UpdateStudentAsync(int id, UpdateStudentDto dto)
        {
            var student = _mapper.Map<Student>(dto);

            student.Id = id;

            var updatedStudent = 
                await _studentRepository.UpdateStudentAsync(student);

            if (updatedStudent == null)
            {
                return null;
            }

            return _mapper.Map<StudentResponseDto?>(updatedStudent);
        }

        public async Task<bool> DeleteStudentAsync(int id)
        {
            return await _studentRepository.DeleteStudentAsync(id);
        }
    }
}
