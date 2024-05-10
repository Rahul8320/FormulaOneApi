using FormulaOne.Api.Models.Dtos;
using FormulaOne.Api.Models.Requests;
using MediatR;

namespace FormulaOne.Api.Commands;

public record UpdateDriverInfoCommand(UpdateDriverRequest DriverRequest) : IRequest<HandlerResult>;
