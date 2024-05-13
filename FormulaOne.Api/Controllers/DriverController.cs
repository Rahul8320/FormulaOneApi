using System.Net;
using FormulaOne.Api.Commands;
using FormulaOne.Api.Models.Requests;
using FormulaOne.Api.Queries;
using FormulaOne.Services.Common;
using FormulaOne.Services.Notification.Interface;
using Hangfire;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FormulaOne.Api.Controllers;

public class DriverController(IMediator mediator, Serilog.ILogger logger) : BaseController
{
    [HttpGet]
    public async Task<IActionResult> GetAllDriver(CancellationToken cancellationToken)
    {
        try
        {
            // Specifying the query
            var query = new GetAllDriverQuery();

            var result = await mediator.Send(query, cancellationToken);

            if (result.StatusCode == HttpStatusCode.NotFound)
            {
                return NotFound(result.ErrorMessage);
            }

            if (result.StatusCode == HttpStatusCode.BadRequest)
            {
                return BadRequest(result.ErrorMessage);
            }

            return Ok(result.Data);
        }
        catch (Exception ex)
        {
            logger.Error(ex, $"[Controller] GetAllDriver Function Error. {ex.Message}");
            return Problem(statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message, title: "Server Error");
        }
    }

    [HttpGet]
    [Route("{driverId:guid}")]
    public async Task<IActionResult> GetDriver(Guid driverId, CancellationToken cancellationToken)
    {
        try
        {
            var query = new GetDriverQuery(driverId);

            var result = await mediator.Send(query, cancellationToken);

            if (result.StatusCode == HttpStatusCode.NotFound)
            {
                return NotFound(result.ErrorMessage);
            }

            if (result.StatusCode == HttpStatusCode.BadRequest)
            {
                return BadRequest(result.ErrorMessage);
            }

            return Ok(result.Data);
        }
        catch (Exception ex)
        {
            logger.Error(ex, $"[Controller] GetDriver Function Error. {ex.Message}");
            return Problem(statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message, title: "Server Error");
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateDriver([FromBody] CreateDriverRequest request, CancellationToken cancellationToken)
    {
        try
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }

            var command = new CreateDriverInfoCommand(request);
            var result = await mediator.Send(command, cancellationToken);

            if (result.StatusCode == HttpStatusCode.BadRequest)
            {
                return BadRequest(result.ErrorMessage);
            }

            // Create and forget type of background jobs
            var notificationDto = new NotificationDto()
            {
                DriverId = result.Data.DriverId,
                Title = "Driver Created",
                Message = $"Hello {result.Data.FullName}, Welcome to our website."
            };
            var jobId = BackgroundJob.Enqueue<INotificationService>(x => x.SendNotification(notificationDto));
            logger.Information($"Background job called with id: {jobId}.");

            return CreatedAtAction(nameof(GetDriver), new { driverId = result.Data.DriverId }, result.Data);
        }
        catch (Exception ex)
        {
            logger.Error(ex, $"[Controller] CreateDriver Function Error. {ex.Message}");
            return Problem(statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message, title: "Server Error");
        }
    }

    [HttpPut]
    public async Task<IActionResult> UpdateDriver([FromBody] UpdateDriverRequest request, CancellationToken cancellationToken)
    {
        try
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }

            var command = new UpdateDriverInfoCommand(request);
            var result = await mediator.Send(command, cancellationToken);

            if (result.StatusCode == HttpStatusCode.NotFound)
            {
                return NotFound(result.ErrorMessage);
            }

            if (result.StatusCode == HttpStatusCode.BadRequest)
            {
                return BadRequest(result.ErrorMessage);
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            logger.Error(ex, $"[Controller] UpdateDriver Function Error. {ex.Message}");
            return Problem(statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message, title: "Server Error");
        }
    }

    [HttpDelete]
    [Route("{driverId:guid}")]
    public async Task<IActionResult> DeleteDriver(Guid driverId, CancellationToken cancellationToken)
    {
        try
        {
            var command = new DeleteDriverInfoCommand(driverId);
            var result = await mediator.Send(command, cancellationToken);

            if (result.StatusCode == HttpStatusCode.NotFound)
            {
                return NotFound(result.ErrorMessage);
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            logger.Error(ex, $"[Controller] DeleteDriver Function Error. {ex.Message}");
            return Problem(statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message, title: "Server Error");
        }
    }
}
