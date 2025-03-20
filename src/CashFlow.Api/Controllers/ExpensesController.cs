using CashFlow.Aplication.UseCases.Expenses.Register;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using Microsoft.AspNetCore.Mvc;

namespace CashFlow.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpensesController : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType(typeof(ResponseRegisteredExpense), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Regiter(
            [FromServices] IRegisterExpenseUseCase registerExpenseUseCase,
            [FromBody] RequestRegisterExpenseDto request
        )
        {
            var response = await registerExpenseUseCase.Execute(request);

            return Created(string.Empty, response);
        }
    }
}
