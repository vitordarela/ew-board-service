using Application.Services;
using Domain.Model.DTO.Report;
using Domain.Model.Enum;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportController : Controller
    {
        private readonly ITaskProjectService taskProjectService;
        private readonly IUserService userService;

        public ReportController(ITaskProjectService projectService, IUserService userService)
        {
            this.taskProjectService = projectService;
            this.userService = userService;
        }

        [HttpGet("task/general")]
        public async Task<ActionResult<GeneralReportResult>> GetGeneralStatisticAsync(
            [FromQuery] string userId,
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate,
            [FromQuery] TaskProjectStatus? taskProjectStatus = null,
            [FromQuery] TaskProjectPriority? taskProjectPriority = null)
        {
            var isManager = await this.CheckRoleIsManager(userId);

            if(!isManager)
            {
                return StatusCode(403);
            }

            int count = await this.taskProjectService.GetGeneralStatisticAsync(startDate, endDate, taskProjectStatus, taskProjectPriority);

            return Ok(new GeneralReportResult { Count = count });
        }

        [HttpGet("task/average/bystatus")]
        public async Task<ActionResult<AverageReportResult>> GetAverageTasksByStatusAsync(
            [FromQuery] string userId,
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate,
            [FromQuery] TaskProjectStatus status)
        {
            var isManager = await this.CheckRoleIsManager(userId);

            if(!isManager)
            {
                return StatusCode(403);
            }

            var average = await this.taskProjectService.GetAverageTasksByStatusAsync(startDate, endDate, status);

            return Ok(average);
        }

        private async Task<bool> CheckRoleIsManager(string userId)
        {
            var user = await this.userService.GetUserByIdAsync(userId).ConfigureAwait(false);

            return user?.Role == Role.Manager;
        }
    }
}
