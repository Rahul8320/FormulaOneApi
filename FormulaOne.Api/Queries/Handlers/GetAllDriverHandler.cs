using System.Net;
using AutoMapper;
using FormulaOne.Api.Models.Dtos;
using FormulaOne.Api.Models.Responses;
using FormulaOne.DataService.Repositories.Interfaces;
using MediatR;

namespace FormulaOne.Api.Queries.Handlers;

public class GetAllDriverHandler(
    IUnitOfWork unitOfWork,
    IMapper mapper) : IRequestHandler<GetAllDriverQuery, HandlerResult<IEnumerable<GetDriverResponse>>>
{
    public async Task<HandlerResult<IEnumerable<GetDriverResponse>>> Handle(GetAllDriverQuery request, CancellationToken cancellationToken)
    {
        var handlerResult = new HandlerResult<IEnumerable<GetDriverResponse>>();

        var drivers = await unitOfWork.DriverRepository.GetAll(cancellationToken);

        if (drivers.Any() is false)
        {
            handlerResult.StatusCode = HttpStatusCode.NotFound;
            handlerResult.ErrorMessage = "Does not found any driver in our database!";

            return handlerResult;
        }

        var resultData = mapper.Map<IEnumerable<GetDriverResponse>>(drivers);

        if (resultData is null)
        {
            handlerResult.StatusCode = HttpStatusCode.BadRequest;
            handlerResult.ErrorMessage = "Failed to map driver list!";

            return handlerResult;
        }

        handlerResult.Data = resultData;

        return handlerResult;
    }
}
