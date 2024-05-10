using System.Net;
using FormulaOne.Api.Models.Dtos;
using FormulaOne.DataService.Repositories.Interfaces;
using MediatR;

namespace FormulaOne.Api.Commands.Handlers;

public class DeleteDriverInfoHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteDriverInfoCommand, HandlerResult>
{
    public async Task<HandlerResult> Handle(DeleteDriverInfoCommand request, CancellationToken cancellationToken)
    {
        var handlerResult = new HandlerResult();

        var driver = await unitOfWork.DriverRepository.GetById(request.DriverId, cancellationToken);

        if (driver is null)
        {
            handlerResult.StatusCode = HttpStatusCode.NotFound;
            handlerResult.ErrorMessage = $"Driver with id: {request.DriverId} is not found in our database!";
            return handlerResult;
        }

        await unitOfWork.DriverRepository.Delete(request.DriverId, cancellationToken);
        await unitOfWork.Complete();

        return handlerResult;
    }
}
