using System.Net;
using AutoMapper;
using FormulaOne.Api.Models.Dtos;
using FormulaOne.Api.Models.Responses;
using FormulaOne.DataService.Repositories.Interfaces;
using FormulaOne.Entities.DbSet;
using FormulaOne.Services.Caching.Interface;
using MediatR;

namespace FormulaOne.Api.Commands.Handlers;

public class CreateDriverInfoHandler(
    IUnitOfWork unitOfWork,
    ICachingService cachingService,
    IMapper mapper) : IRequestHandler<CreateDriverInfoCommand, HandlerResult<GetDriverResponse>>
{
    public async Task<HandlerResult<GetDriverResponse>> Handle(CreateDriverInfoCommand request, CancellationToken cancellationToken)
    {
        var handlerResult = new HandlerResult<GetDriverResponse>();

        // Mapped request to driver
        var driver = mapper.Map<Driver>(request.DriverRequest);

        if (driver is null)
        {
            handlerResult.StatusCode = HttpStatusCode.BadRequest;
            handlerResult.ErrorMessage = "Failed to map driver entity!";
            return handlerResult;
        }

        // Add driver to database
        await unitOfWork.DriverRepository.Add(driver, cancellationToken);
        var isComplete = await unitOfWork.Complete();

        if (isComplete is false)
        {
            handlerResult.StatusCode = HttpStatusCode.BadRequest;
            handlerResult.ErrorMessage = "Failed create new driver!";
            return handlerResult;
        }

        // Mapped driver to response
        var mappedDriver = mapper.Map<GetDriverResponse>(driver);

        if (mappedDriver is null)
        {
            handlerResult.StatusCode = HttpStatusCode.BadRequest;
            handlerResult.ErrorMessage = "Failed to map driver details!";
            return handlerResult;
        }

        // Removed cached for all drivers
        cachingService.RemoveData("drivers");

        handlerResult.Data = mappedDriver;
        handlerResult.StatusCode = HttpStatusCode.Created;

        return handlerResult;
    }
}
