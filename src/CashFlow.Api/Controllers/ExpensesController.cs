using CashFlow.Aplication.UseCases.Expenses.Register;
using CashFlow.Communication.Requests;
using Microsoft.AspNetCore.Mvc;

namespace CashFlow.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpensesController : ControllerBase
    {
        [HttpPost]
        public IActionResult Regiter([FromBody] RequestRegisterExpenseDto request)
        {
            var registerExpenseUseCase = new RegisterExpenseUseCase();

            var response = registerExpenseUseCase.Execute(request);

            return Created(string.Empty, response);
        }
    }
}
