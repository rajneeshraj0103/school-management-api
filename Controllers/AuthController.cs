using Microsoft.AspNetCore.Mvc;
using School_Management.DTOs;
using School_Management.Helpers;
using School_Management.Interfaces;

namespace School_Management.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IJwtService _jwtService;

        private readonly IStudentRepository _studentRepository;
        public AuthController(IJwtService jwtService,
            IStudentRepository studentRepository)
        {
            _jwtService = jwtService;
            _studentRepository = studentRepository;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var student = await _studentRepository
                .GetStudentByEmailAsync(dto.Email);

            if (student == null)
            {
                return Unauthorized("Invalid Credentials");
            }

            var isPasswordValid =
                BCrypt.Net.BCrypt.Verify(
                    dto.Password,
                    student.Password);

            if (!isPasswordValid)
            {
                return Unauthorized(
                    new ApiResponse<object>
                    {
                        Success = false,

                        Message = "Invalid credentials",

                        Data = null
                    });
            }

            var token = _jwtService.GenerateToken(
                student.Email,
                student.Role);

            return Ok(new ApiResponse<AuthResponseDto>
            {
                Success = true,

                Message = "Login successful",

                Data = new AuthResponseDto
                {
                    Token = token
                }
            });
        }
    }
}
