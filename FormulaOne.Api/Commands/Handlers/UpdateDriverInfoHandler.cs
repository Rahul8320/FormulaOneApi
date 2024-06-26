using System.Net;
using AutoMapper;
using FormulaOne.Api.Models.Dtos;
using FormulaOne.DataService.Repositories.Interfaces;
using FormulaOne.Entities.DbSet;
using FormulaOne.Services.Caching.Interface;
using MediatR;

namespace FormulaOne.Api.Commands.Handlers;

public class UpdateDriverInfoHandler(
    IUnitOfWork unitOfWork,
    ICachingService cachingService,
    IMapper mapper) : IRequestHandler<UpdateDriverInfoCommand, HandlerResult>
{
    public async Task<HandlerResult> Handle(UpdateDriverInfoCommand request, CancellationToken cancellationToken)
    {
        var handlerResult = new HandlerResult();

        // Mapped request to driver
        var driver = mapper.Map<Driver>(request.DriverRequest);

        if (driver is null)
        {
            handlerResult.StatusCode = HttpStatusCode.BadRequest;
            handlerResult.ErrorMessage = "Failed to mapped driver entity!";

            return handlerResult;
        }

        // Update database
        var isSuccess = await unitOfWork.DriverRepository.Update(driver, cancellationToken);

        if (isSuccess == false)
        {
            handlerResult.StatusCode = HttpStatusCode.NotFound;
            handlerResult.ErrorMessage = $"Driver with id: {driver.Id} is not found in our database!";
            return handlerResult;
        }

        var isComplete = await unitOfWork.Complete();

        if (isComplete == false)
        {
            handlerResult.StatusCode = HttpStatusCode.BadRequest;
            handlerResult.ErrorMessage = $"Failed to update Driver with id: {request.DriverRequest.DriverId}!";
        }

        // Removed from cached
        cachingService.RemoveData("drivers");
        cachingService.RemoveData($"driver-{request.DriverRequest.DriverId}");

        return handlerResult;
    }
}
