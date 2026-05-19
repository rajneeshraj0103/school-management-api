using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using School_Management.DTOs;
using School_Management.Helpers;
using School_Management.Interfaces;

namespace School_Management.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService _studentService;
        public StudentsController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllStudents(
            [FromQuery] PaginationParams paginationParams)
        {
            var students = await _studentService.GetAllStudentsAsync(paginationParams);

            return Ok(new ApiResponse<PagedResponse<StudentResponseDto>>
            {
                Success = true,

                Message = "Students fetched successfully",

                Data = students
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetStudentById(int id)
        {
            if (id <= 0)
            {
                return BadRequest(
                    new ApiResponse<object>
                    {
                        Success = false,

                        Message = "Invalid student id",

                        Data = null
                    });
            }
            var student = await _studentService.GetStudentByIdAsync(id);

            if (student == null)
            {
                return NotFound(
                    new ApiResponse<StudentResponseDto>
                    {
                        Success = false,

                        Message = "Student not found",

                        Data = null
                    });
            }

            return Ok(
                new ApiResponse<StudentResponseDto>
                {
                    Success = true,

                    Message = "Student fetched successfully",

                    Data = student
                });
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateStudent(CreateStudentDto dto)
        {
            var createdStudent = await _studentService.CreateStudentAsync(dto);

            return CreatedAtAction(
                nameof(GetStudentById),
                new { id = createdStudent.Id },
                new ApiResponse<StudentResponseDto>
                {
                    Success = true,

                    Message = "Student created successfully",

                    Data = createdStudent
                });
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateStudent(int id, UpdateStudentDto student)
        {
            var updatedStudent = 
                await _studentService.UpdateStudentAsync(id, student);

            if (updatedStudent == null)
            {
                return NotFound(
                    new ApiResponse<StudentResponseDto>
                    {
                        Success = false,

                        Message = "Student not found",

                        Data = null
                    });
            }
            return Ok(
                new ApiResponse<StudentResponseDto>
                {
                    Success = true,

                    Message = "Student updated successfully",

                    Data = updatedStudent
                });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            if (id <= 0)
            {
                return BadRequest(
                    new ApiResponse<object>
                    {
                        Success = false,

                        Message = "Invalid student id",

                        Data = null
                    });
            }
            var deletedStudent = await _studentService.DeleteStudentAsync(id);
            if (!deletedStudent)
            {
                return NotFound(
                    new ApiResponse<object>
                    {
                        Success = false,

                        Message = "Student not found",

                        Data = null
                    });
            }
            return Ok(
                new ApiResponse<object>
                {
                    Success = true,

                    Message = "Student deleted successfully",

                    Data = null
                });
        }
    }
}
