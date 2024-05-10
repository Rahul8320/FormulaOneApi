using FormulaOne.Api.Models.Dtos;
using FormulaOne.Api.Models.Requests;
using FormulaOne.Api.Models.Responses;
using MediatR;

namespace FormulaOne.Api.Commands;

public record CreateDriverInfoCommand(CreateDriverRequest DriverRequest) : IRequest<HandlerResult<GetDriverResponse>>;
