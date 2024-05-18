using System.Net;
using AutoMapper;
using FormulaOne.Api.Models.Dtos;
using FormulaOne.DataService.Repositories.Interfaces;
using FormulaOne.Entities.DbSet;
using FormulaOne.Services.Caching.Interface;
using MediatR;

namespace FormulaOne.Api.Commands.Handlers;

public class UpdateAchievementHandler(
    IUnitOfWork unitOfWork,
    ICachingService cachingService,
    IMapper mapper) : IRequestHandler<UpdateAchievementCommand, HandlerResult>
{
    public async Task<HandlerResult> Handle(UpdateAchievementCommand request, CancellationToken cancellationToken)
    {
        var handlerResult = new HandlerResult();

        // Mapped request to achievment
        var achievement = mapper.Map<Achievement>(request.AchievementRequest);

        if (achievement is null)
        {
            handlerResult.StatusCode = HttpStatusCode.BadRequest;
            handlerResult.ErrorMessage = "Failed to map request to entity!";

            return handlerResult;
        }

        // Update achievment data into database
        await unitOfWork.AchievementRepository.Update(achievement, cancellationToken);
        var isComplete = await unitOfWork.Complete();

        if (isComplete is false)
        {
            handlerResult.StatusCode = HttpStatusCode.BadRequest;
            handlerResult.ErrorMessage = "Failed to create driver achievement!";
        }

        // Removed cached for achievment 
        cachingService.RemoveData($"driver-achievments-{request.AchievementRequest.DriverId}");

        return handlerResult;
    }
}
