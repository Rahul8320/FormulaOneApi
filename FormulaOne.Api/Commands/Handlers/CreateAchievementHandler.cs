using System.Net;
using AutoMapper;
using FormulaOne.Api.Models.Dtos;
using FormulaOne.Api.Models.Responses;
using FormulaOne.DataService.Repositories.Interfaces;
using FormulaOne.Entities.DbSet;
using FormulaOne.Services.Caching.Interface;
using MediatR;

namespace FormulaOne.Api.Commands.Handlers;

public class CreateAchievementHandler(
    IUnitOfWork unitOfWork,
    ICachingService cachingService,
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

        // Add achievment into database
        await unitOfWork.AchievementRepository.Add(achievement, cancellationToken);
        var isComplete = await unitOfWork.Complete();

        if (isComplete is false)
        {
            handlerResult.StatusCode = HttpStatusCode.BadRequest;
            handlerResult.ErrorMessage = "Failed to create driver achievement!";

            return handlerResult;
        }

        // Mapped achievment into response model
        var mappedAchievment = mapper.Map<DriverAchievementResponse>(achievement);

        if (mappedAchievment is null)
        {
            handlerResult.StatusCode = HttpStatusCode.BadRequest;
            handlerResult.ErrorMessage = "Failed to map entity to response!";

            return handlerResult;
        }

        // Removed cached for achievment 
        cachingService.RemoveData($"driver-achievments-{request.AchievementRequest.DriverId}");

        handlerResult.Data = mappedAchievment;
        return handlerResult;
    }
}
