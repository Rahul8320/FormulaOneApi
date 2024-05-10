using System.Net;
using AutoMapper;
using FormulaOne.Api.Models.Dtos;
using FormulaOne.Api.Models.Responses;
using FormulaOne.DataService.Repositories.Interfaces;
using MediatR;

namespace FormulaOne.Api.Queries.Handlers;

public class GetDriverAchievementHandler(
    IUnitOfWork unitOfWork,
    IMapper mapper) : IRequestHandler<GetDriverAchievementQuery, HandlerResult<List<DriverAchievementResponse>>>
{
    public async Task<HandlerResult<List<DriverAchievementResponse>>> Handle(GetDriverAchievementQuery request, CancellationToken cancellationToken)
    {
        var handlerResult = new HandlerResult<List<DriverAchievementResponse>>();

        var driverAchievements = await unitOfWork.AchievementRepository.GetDriverAchievement(request.DriverId, cancellationToken);

        if (driverAchievements is null || driverAchievements.Count == 0)
        {
            handlerResult.StatusCode = HttpStatusCode.NotFound;
            handlerResult.ErrorMessage = $"Achievement with driver id: {request.DriverId} is not found in our database!";

            return handlerResult;
        }

        var resultData = mapper.Map<List<DriverAchievementResponse>>(driverAchievements);

        if (resultData is null)
        {
            handlerResult.StatusCode = HttpStatusCode.BadRequest;
            handlerResult.ErrorMessage = "Failed to map entity to response model!";

            return handlerResult;
        }

        handlerResult.Data = resultData;

        return handlerResult;
    }
}
