using School_Management.DTOs;
using School_Management.Helpers;

namespace School_Management.Interfaces
{
    public interface IStudentService
    {
        Task<PagedResponse<StudentResponseDto>> GetAllStudentsAsync(PaginationParams paginationParams);
        Task<StudentResponseDto?> GetStudentByIdAsync(int id);
        Task<StudentResponseDto> CreateStudentAsync(CreateStudentDto dto);
        Task<StudentResponseDto?> UpdateStudentAsync(int id, UpdateStudentDto dto);
        Task<bool> DeleteStudentAsync(int id);
    }
}
