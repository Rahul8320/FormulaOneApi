using System.Net;
using AutoMapper;
using FormulaOne.Api.Models.Dtos;
using FormulaOne.Api.Models.Responses;
using FormulaOne.DataService.Repositories.Interfaces;
using MediatR;

namespace FormulaOne.Api.Queries.Handlers;

public class GetDriverHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetDriverQuery, HandlerResult<GetDriverResponse>>
{
    public async Task<HandlerResult<GetDriverResponse>> Handle(GetDriverQuery request, CancellationToken cancellationToken)
    {
        var handlerResult = new HandlerResult<GetDriverResponse>();

        var driver = await unitOfWork.DriverRepository.GetById(request.DriverId, cancellationToken);

        if (driver is null)
        {
            handlerResult.StatusCode = HttpStatusCode.NotFound;
            handlerResult.ErrorMessage = $"Driver with id: {request.DriverId} is not found in our database!";
            return handlerResult;
        }

        var resultData = mapper.Map<GetDriverResponse>(driver);

        if (resultData is null)
        {
            handlerResult.StatusCode = HttpStatusCode.BadRequest;
            handlerResult.ErrorMessage = "Failed to map driver details!";
            return handlerResult;
        }

        handlerResult.Data = resultData;

        return handlerResult;
    }
}
