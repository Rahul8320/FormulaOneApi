using FormulaOne.Api.Models.Dtos;
using MediatR;

namespace FormulaOne.Api.Commands;

public record DeleteDriverInfoCommand(Guid DriverId) : IRequest<HandlerResult>;

