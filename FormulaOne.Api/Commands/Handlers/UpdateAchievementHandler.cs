using System.Net;
using AutoMapper;
using FormulaOne.Api.Models.Dtos;
using FormulaOne.DataService.Repositories.Interfaces;
using FormulaOne.Entities.DbSet;
using MediatR;

namespace FormulaOne.Api.Commands.Handlers;

public class UpdateAchievementHandler(
    IUnitOfWork unitOfWork,
    IMapper mapper) : IRequestHandler<UpdateAchievementCommand, HandlerResult>
{
    public async Task<HandlerResult> Handle(UpdateAchievementCommand request, CancellationToken cancellationToken)
    {
        var handlerResult = new HandlerResult();

        var achievement = mapper.Map<Achievement>(request.AchievementRequest);

        if (achievement is null)
        {
            handlerResult.StatusCode = HttpStatusCode.BadRequest;
            handlerResult.ErrorMessage = "Failed to map request to entity!";

            return handlerResult;
        }

        await unitOfWork.AchievementRepository.Update(achievement, cancellationToken);
        var isComplete = await unitOfWork.Complete();

        if (isComplete is false)
        {
            handlerResult.StatusCode = HttpStatusCode.BadRequest;
            handlerResult.ErrorMessage = "Failed to create driver achievement!";
        }

        return handlerResult;
    }
}
