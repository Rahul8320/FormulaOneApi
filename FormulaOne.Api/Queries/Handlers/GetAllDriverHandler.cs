using System.Net;
using AutoMapper;
using FormulaOne.Api.Models.Dtos;
using FormulaOne.Api.Models.Responses;
using FormulaOne.DataService.Repositories.Interfaces;
using FormulaOne.Services.Caching.Interface;
using MediatR;

namespace FormulaOne.Api.Queries.Handlers;

public class GetAllDriverHandler(
    IUnitOfWork unitOfWork,
    ICachingService cachingService,
    IMapper mapper) : IRequestHandler<GetAllDriverQuery, HandlerResult<IEnumerable<GetDriverResponse>>>
{
    public async Task<HandlerResult<IEnumerable<GetDriverResponse>>> Handle(GetAllDriverQuery request, CancellationToken cancellationToken)
    {
        // create handler result object 
        var handlerResult = new HandlerResult<IEnumerable<GetDriverResponse>>();

        // Fetch drivers from cached 
        var cachedDrivers = cachingService.GetData<IEnumerable<GetDriverResponse>>("drivers");

        if (cachedDrivers != null && cachedDrivers.Any())
        {
            handlerResult.Data = cachedDrivers;
            return handlerResult;
        }

        // fetch drivers from repository
        var drivers = await unitOfWork.DriverRepository.GetAll(cancellationToken);

        if (drivers.Any() is false)
        {
            handlerResult.StatusCode = HttpStatusCode.NotFound;
            handlerResult.ErrorMessage = "Does not found any driver in our database!";

            return handlerResult;
        }

        // Mapped drivers into response model
        var mappedDrivers = mapper.Map<IEnumerable<GetDriverResponse>>(drivers);

        if (mappedDrivers is null)
        {
            handlerResult.StatusCode = HttpStatusCode.BadRequest;
            handlerResult.ErrorMessage = "Failed to map driver list!";

            return handlerResult;
        }

        // set data into cached
        cachingService.SetData<IEnumerable<GetDriverResponse>>("drivers", mappedDrivers);

        handlerResult.Data = mappedDrivers;

        return handlerResult;
    }
}
