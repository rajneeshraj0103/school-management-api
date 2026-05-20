using Microsoft.AspNetCore.Mvc;
using School_Management.DTOs;
using School_Management.Entities;
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

            var refreshToken = _jwtService.GenerateRefreshToken();

            student.RefreshToken = refreshToken;

            student.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

            await _studentRepository.UpdateStudentAsync(student);

            return Ok(new ApiResponse<AuthResponseDto>
            {
                Success = true,

                Message = "Login successful",

                Data = new AuthResponseDto
                {
                    Token = token,
                    RefreshToken = refreshToken
                }
            });
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(RefreshTokenDto dto)
        {
            var students = await _studentRepository
                .GetAllStudentsAsync(
                new PaginationParams())
                .ContinueWith(t =>
                t.Result.Data.FirstOrDefault(x =>
                x.RefreshToken == dto.RefreshToken));

            if(students == null)
            {
                return Unauthorized(
                    new ApiResponse<object>
                    {
                        Success = false,

                        Message = "Invalid refresh token",

                        Data = null
                    });
            }

            if (students.RefreshTokenExpiryTime
        <= DateTime.UtcNow)
            {
                return Unauthorized(
                    new ApiResponse<object>
                    {
                        Success = false,

                        Message = "Refresh token expired",

                        Data = null
                    });
            }

            var newAccessToken =
                _jwtService.GenerateToken(
                    students.Email,
                    students.Role);

            return Ok(
                new ApiResponse<AuthResponseDto>
                {
                    Success = true,

                    Message = "Token refreshed successfully",

                    Data = new AuthResponseDto
                    {
                        Token = newAccessToken,

                        RefreshToken = students.RefreshToken!
                    }
                });
        }
    }
}
