using FormulaOne.Api.Models.Dtos;
using FormulaOne.Api.Models.Responses;
using MediatR;

namespace FormulaOne.Api.Queries;

public record GetDriverAchievementQuery(Guid DriverId) : IRequest<HandlerResult<List<DriverAchievementResponse>>>;