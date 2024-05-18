using System.Net;
using AutoMapper;
using FormulaOne.Api.Models.Dtos;
using FormulaOne.Api.Models.Responses;
using FormulaOne.DataService.Repositories.Interfaces;
using FormulaOne.Services.Caching.Interface;
using MediatR;

namespace FormulaOne.Api.Queries.Handlers;

public class GetDriverAchievementHandler(
    IUnitOfWork unitOfWork,
    ICachingService cachingService,
    IMapper mapper) : IRequestHandler<GetDriverAchievementQuery, HandlerResult<List<DriverAchievementResponse>>>
{
    public async Task<HandlerResult<List<DriverAchievementResponse>>> Handle(GetDriverAchievementQuery request, CancellationToken cancellationToken)
    {
        // Create handler result object
        var handlerResult = new HandlerResult<List<DriverAchievementResponse>>();

        // Fetch driver achievments from cached
        var cachedDriverAchievments = cachingService.GetData<List<DriverAchievementResponse>>($"driver-achievments-{request.DriverId}");

        if(cachedDriverAchievments != null && cachedDriverAchievments.Count != 0)
        {
            handlerResult.Data = cachedDriverAchievments;
            return handlerResult;
        }

        // Fetch driver achievments from repository
        var driverAchievements = await unitOfWork.AchievementRepository.GetDriverAchievement(request.DriverId, cancellationToken);

        if (driverAchievements is null || driverAchievements.Count == 0)
        {
            handlerResult.StatusCode = HttpStatusCode.NotFound;
            handlerResult.ErrorMessage = $"Achievement with driver id: {request.DriverId} is not found in our database!";

            return handlerResult;
        }

        // Mapped driver achievements into response model
        var mappedDriverAchievements = mapper.Map<List<DriverAchievementResponse>>(driverAchievements);

        if (mappedDriverAchievements is null)
        {
            handlerResult.StatusCode = HttpStatusCode.BadRequest;
            handlerResult.ErrorMessage = "Failed to map entity to response model!";

            return handlerResult;
        }

        // set driver achievments into cached
        cachingService.SetData<List<DriverAchievementResponse>>($"driver-achievments-{request.DriverId}", mappedDriverAchievements);

        handlerResult.Data = mappedDriverAchievements;

        return handlerResult;
    }
}
