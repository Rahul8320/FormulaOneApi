using System.Net;
using AutoMapper;
using FormulaOne.Api.Models.Dtos;
using FormulaOne.Api.Models.Responses;
using FormulaOne.DataService.Repositories.Interfaces;
using FormulaOne.Entities.DbSet;
using MediatR;

namespace FormulaOne.Api.Commands.Handlers;

public class CreateAchievementHandler(
    IUnitOfWork unitOfWork,
    IMapper mapper) : IRequestHandler<CreateAchievementCommand, HandlerResult<DriverAchievementResponse>>
{
    public async Task<HandlerResult<DriverAchievementResponse>> Handle(CreateAchievementCommand request, CancellationToken cancellationToken)
    {
        var handlerResult = new HandlerResult<DriverAchievementResponse>();

        var achievement = mapper.Map<Achievement>(request.AchievementRequest);

        if (achievement is null)
        {
            handlerResult.StatusCode = HttpStatusCode.BadRequest;
            handlerResult.ErrorMessage = "Failed to map request to entity!";

            return handlerResult;
        }

        await unitOfWork.AchievementRepository.Add(achievement, cancellationToken);
        var isComplete = await unitOfWork.Complete();

        if (isComplete is false)
        {
            handlerResult.StatusCode = HttpStatusCode.BadRequest;
            handlerResult.ErrorMessage = "Failed to create driver achievement!";

            return handlerResult;
        }

        var resultData = mapper.Map<DriverAchievementResponse>(achievement);

        if (resultData is null)
        {
            handlerResult.StatusCode = HttpStatusCode.BadRequest;
            handlerResult.ErrorMessage = "Failed to map entity to response!";

            return handlerResult;
        }

        handlerResult.Data = resultData;
        return handlerResult;
    }
}
