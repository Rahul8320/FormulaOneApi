using FormulaOne.Api.Models.Dtos;
using FormulaOne.Api.Models.Requests;
using MediatR;

namespace FormulaOne.Api.Commands;

public record UpdateAchievementCommand(UpdateDriverAchievementRequest AchievementRequest) : IRequest<HandlerResult>;