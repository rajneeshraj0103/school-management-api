using Microsoft.EntityFrameworkCore;
using School_Management.Data;
using School_Management.Entities;
using School_Management.Helpers;
using School_Management.Interfaces;

namespace School_Management.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly ApplicationDbContext _context;
        public StudentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResponse<Student>> GetAllStudentsAsync(PaginationParams paginationParams)
        {
            var query = _context.Students.AsQueryable();

            if(!string.IsNullOrWhiteSpace(
                paginationParams.Search))
            {
                query = query.Where(x =>
                x.Name.Contains(paginationParams.Search)
                ||
                x.Email.Contains(paginationParams.Search));
            }

            var totalRecords = await query.CountAsync();

            query = paginationParams.SortBy?.ToLower() switch
            {
                "name" => query.OrderBy(x => x.Name),
                "createddate" => query.OrderBy(
                    x => x.CreatedDate),
                _ => query.OrderBy(x => x.Id)
            };

            var students =  await query
                .Skip((paginationParams.PageNumber - 1)
                * paginationParams.PageSize)
                .Take(paginationParams.PageSize)
                .ToListAsync();

            return new PagedResponse<Student>
            {
                PageNumber = paginationParams.PageNumber,

                PageSize = paginationParams.PageSize,

                TotalRecords = totalRecords,

                TotalPages = (int)Math.Ceiling(
                    totalRecords / (double)paginationParams.PageSize),

                Data = students
            };
        }

        public async Task<Student?> GetStudentByIdAsync(int id)
        {
            return await _context.Students
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Student> CreateStudentAsync(Student student)
        {
            await _context.Students.AddAsync(student);
            await _context.SaveChangesAsync();

            return student;
        }

        public async Task<Student?> UpdateStudentAsync(Student student)
        {
            var existingStudent = await _context.Students
                .FirstOrDefaultAsync(x => x.Id == student.Id);
            if (existingStudent == null)
            {
                return null;
            }

            existingStudent.Name = student.Name;
            existingStudent.Email = student.Email;
            existingStudent.Age = student.Age;

            await _context.SaveChangesAsync();

            return existingStudent;
        }

        public async Task<bool> DeleteStudentAsync(int id)
        {
            var student = await _context.Students
                .FirstOrDefaultAsync(x => x.Id == id);

            if (student == null)
            {
                return false;
            }

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Student?> GetStudentByEmailAsync(string email)
        {
            return await _context.Students
                .FirstOrDefaultAsync(X => X.Email == email);
        }
    }
}
