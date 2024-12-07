using Application.UserCQ.Commands;
using Application.UserCQ.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers {
    [Route("[controller]")]
    [ApiController]
    public class UserController(IMediator mediator) : ControllerBase {
        private readonly IMediator _mediator = mediator;

        /// <summary>
        /// Rota responsável pela criação de um usuário
        /// </summary>
        /// <param name="command">
        /// Um objeto CreateUserCommand
        /// </param>
        /// <returns> Os dados do usuário criado</returns>
        /// <remarks>
        /// Exemplo de request:
        /// ```
        /// POST /User/Create-User 
        /// {
        ///   "name": "Thiago",
        ///   "surname": "Moura",
        ///   "email": "meu.email@teste.com.br",
        ///   "password": "senha@123",
        ///   "username": "ThiMoura"
        /// }
        /// ```
        /// </remarks>
        /// <response code="200">Retorna os dados de um novo usuário</response>
        /// <response code="400">Se algum dado for digitado incorretamente</response>
        [HttpPost("Create-User")]
        public async Task<ActionResult<UserInfoViewModel>> CreateUser(CreateUserCommand command) {
            return Ok(await _mediator.Send(command));
        }
    }
}
