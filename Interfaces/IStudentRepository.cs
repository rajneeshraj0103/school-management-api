using School_Management.Entities;
using School_Management.Helpers;

namespace School_Management.Interfaces
{
    public interface IStudentRepository
    {
        Task<PagedResponse<Student>> GetAllStudentsAsync(PaginationParams paginationParams);
        Task<Student?> GetStudentByIdAsync(int id);
        Task<Student> CreateStudentAsync(Student studnet);
        Task<Student?> UpdateStudentAsync(Student student);
        Task<bool> DeleteStudentAsync(int id);
        Task<Student?> GetStudentByEmailAsync(string email);
    }
}
