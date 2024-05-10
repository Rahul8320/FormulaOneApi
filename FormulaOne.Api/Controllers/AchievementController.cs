using System.Net;
using FormulaOne.Api.Commands;
using FormulaOne.Api.Models.Requests;
using FormulaOne.Api.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FormulaOne.Api.Controllers;

public class AchievementController(IMediator mediator, Serilog.ILogger logger) : BaseController
{

    [HttpGet]
    [Route("{driverId:guid}")]
    public async Task<IActionResult> GetDriverAchievement(Guid driverId)
    {
        try
        {
            var query = new GetDriverAchievementQuery(driverId);
            var result = await mediator.Send(query);

            if (result.StatusCode is HttpStatusCode.NotFound)
            {
                return NotFound(result.ErrorMessage);
            }

            if (result.StatusCode is HttpStatusCode.BadRequest)
            {
                return BadRequest(result.ErrorMessage);
            }

            return Ok(result.Data);
        }
        catch (Exception ex)
        {
            logger.Error(ex, $"[Controller] GetDriverAchievements Function Error. {ex.Message}");
            return Problem(statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message, title: "Server Error");
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateAchievement([FromBody] CreateDriverAchievementRequest request)
    {
        try
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }

            var command = new CreateAchievementCommand(request);

            var result = await mediator.Send(command);

            if (result.StatusCode is HttpStatusCode.BadRequest)
            {
                return BadRequest(result.ErrorMessage);
            }

            return CreatedAtAction(nameof(GetDriverAchievement), new { driverId = result.Data.DriverId }, result.Data);
        }
        catch (Exception ex)
        {
            logger.Error(ex, $"[Controller] CreateAchievement Function Error. {ex.Message}");
            return Problem(statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message, title: "Server Error");
        }
    }

    [HttpPut]
    public async Task<IActionResult> UpdateAchievement([FromBody] UpdateDriverAchievementRequest request)
    {
        try
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }

            var command = new UpdateAchievementCommand(request);

            var result = await mediator.Send(command);

            if (result.StatusCode is HttpStatusCode.BadRequest)
            {
                return BadRequest(result.ErrorMessage);
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            logger.Error(ex, $"[Controller] UpdateAchievement Function Error. {ex.Message}");
            return Problem(statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message, title: "Server Error");
        }
    }
}
