using CashFlow.Application.UseCases.Expenses.Register;
using CashFlow.Application.UseCases.Users.Register;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CashFlow.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType(typeof(ResponseRegisteredUser), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Regiter(
            [FromServices] IRegisterUserUseCase useCase,
            [FromBody] RequestRegisterUser request
        )
        {
            var response = await useCase.Execute(request);

            return Created(string.Empty, response);
        }

        [HttpGet]
        [ProducesResponseType(typeof(ResponseUserProfile), StatusCodes.Status200OK)]
        [Authorize]
        public async Task<IActionResult> GetProfile(
            [FromServices] IGetUserProfileUseCase useCase
        )
        {
            var response = await useCase.Execute();
            return Ok(response);
        }
    }
}
