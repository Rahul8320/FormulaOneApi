using System.Net;
using FormulaOne.Api.Models.Dtos;
using FormulaOne.DataService.Repositories.Interfaces;
using FormulaOne.Services.Caching.Interface;
using MediatR;

namespace FormulaOne.Api.Commands.Handlers;

public class DeleteDriverInfoHandler(
    IUnitOfWork unitOfWork,
    ICachingService cachingService) : IRequestHandler<DeleteDriverInfoCommand, HandlerResult>
{
    public async Task<HandlerResult> Handle(DeleteDriverInfoCommand request, CancellationToken cancellationToken)
    {
        var handlerResult = new HandlerResult();

        // Fetch driver from database
        var driver = await unitOfWork.DriverRepository.GetById(request.DriverId, cancellationToken);

        if (driver is null)
        {
            handlerResult.StatusCode = HttpStatusCode.NotFound;
            handlerResult.ErrorMessage = $"Driver with id: {request.DriverId} is not found in our database!";
            return handlerResult;
        }

        // Delete driver from database
        await unitOfWork.DriverRepository.Delete(request.DriverId, cancellationToken);
        await unitOfWork.Complete();

        // Removed from cached
        cachingService.RemoveData("drivers");
        cachingService.RemoveData($"driver-{request.DriverId}");

        return handlerResult;
    }
}
