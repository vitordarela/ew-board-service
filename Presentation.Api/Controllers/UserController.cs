using Application.Services;
using Domain.Model.DTO.User;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsersAsync()
        {
            var projects = await this.userService.GetAllUsersAsync();
            return Ok(projects);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUserAsync([FromBody] UserRequest userRequest)
        {
            var createdUser = await this.userService.AddUserAsync(userRequest);
            return Ok(new { id = createdUser.Id });
        }
    }
}

