using FormulaOne.Api.Models.Dtos;
using FormulaOne.Api.Models.Requests;
using FormulaOne.Api.Models.Responses;
using MediatR;

namespace FormulaOne.Api.Commands;

public record CreateAchievementCommand(CreateDriverAchievementRequest AchievementRequest) : IRequest<HandlerResult<DriverAchievementResponse>>;
