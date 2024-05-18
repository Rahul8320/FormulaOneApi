using System.Net;
using AutoMapper;
using FormulaOne.Api.Models.Dtos;
using FormulaOne.Api.Models.Responses;
using FormulaOne.DataService.Repositories.Interfaces;
using FormulaOne.Services.Caching.Interface;
using MediatR;

namespace FormulaOne.Api.Queries.Handlers;

public class GetDriverHandler(
    IUnitOfWork unitOfWork,
    ICachingService cachingService,
    IMapper mapper) : IRequestHandler<GetDriverQuery, HandlerResult<GetDriverResponse>>
{
    public async Task<HandlerResult<GetDriverResponse>> Handle(GetDriverQuery request, CancellationToken cancellationToken)
    {
        // Create handler result object
        var handlerResult = new HandlerResult<GetDriverResponse>();

        // Fetch drivers from cached 
        var cachedDriver = cachingService.GetData<GetDriverResponse>($"driver-{request.DriverId}");

        if (cachedDriver != null)
        {
            handlerResult.Data = cachedDriver;
            return handlerResult;
        }

        // Fetch driver from repository
        var driver = await unitOfWork.DriverRepository.GetById(request.DriverId, cancellationToken);

        if (driver is null)
        {
            handlerResult.StatusCode = HttpStatusCode.NotFound;
            handlerResult.ErrorMessage = $"Driver with id: {request.DriverId} is not found in our database!";
            return handlerResult;
        }

        // Mapped driver into response model
        var mappedDriver = mapper.Map<GetDriverResponse>(driver);

        if (mappedDriver is null)
        {
            handlerResult.StatusCode = HttpStatusCode.BadRequest;
            handlerResult.ErrorMessage = "Failed to map driver details!";
            return handlerResult;
        }

        // Set driver data into cached
        cachingService.SetData<GetDriverResponse>($"driver-{request.DriverId}", mappedDriver);

        handlerResult.Data = mappedDriver;

        return handlerResult;
    }
}
