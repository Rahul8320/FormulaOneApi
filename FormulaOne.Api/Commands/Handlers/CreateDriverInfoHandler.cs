using System.Net;
using AutoMapper;
using FormulaOne.Api.Models.Dtos;
using FormulaOne.Api.Models.Responses;
using FormulaOne.DataService.Repositories.Interfaces;
using FormulaOne.Entities.DbSet;
using MediatR;

namespace FormulaOne.Api.Commands.Handlers;

public class CreateDriverInfoHandler(
    IUnitOfWork unitOfWork,
    IMapper mapper) : IRequestHandler<CreateDriverInfoCommand, HandlerResult<GetDriverResponse>>
{
    public async Task<HandlerResult<GetDriverResponse>> Handle(CreateDriverInfoCommand request, CancellationToken cancellationToken)
    {
        var handlerResult = new HandlerResult<GetDriverResponse>();

        var driver = mapper.Map<Driver>(request.DriverRequest);

        if (driver is null)
        {
            handlerResult.StatusCode = HttpStatusCode.BadRequest;
            handlerResult.ErrorMessage = "Failed to map driver entity!";
            return handlerResult;
        }

        await unitOfWork.DriverRepository.Add(driver, cancellationToken);
        var isComplete = await unitOfWork.Complete();

        if (isComplete is false)
        {
            handlerResult.StatusCode = HttpStatusCode.BadRequest;
            handlerResult.ErrorMessage = "Failed create new driver!";
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
        handlerResult.StatusCode = HttpStatusCode.Created;

        return handlerResult;
    }
}
