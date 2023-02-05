using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using pnb.api.easybox.domain.Interface;
using pnb.api.easybox.domain.Model;
using Serilog;
using static Slapper.AutoMapper;

namespace pnb.api.easybox.web.Controllers
{
    [ApiController]
    [Route("api/v1/user-profile")]
    public class UserProfileController : Controller
    {   
        private readonly IPerfilUsuarioService _perfilUsuarioService;

        private readonly ILogger<UserProfileController> _logger;

        private EventId correlationId;
        private Guid correlationGuid;

        public UserProfileController(IPerfilUsuarioService perfilUsuarioService,
            ILogger<UserProfileController> logger)
        {
            _perfilUsuarioService = perfilUsuarioService;
            _logger = logger;
             correlationId = new EventId();
             correlationGuid = Guid.NewGuid();
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllUserProfiles()
        {          
            _logger.LogInformation(correlationId, null, "Obtendo todos os perfis de usuarios", null);
            var result = await _perfilUsuarioService.GetAllUserProfiles(correlationGuid);
            if (result.didResult)
            {
                _logger.LogInformation(correlationId, null, $"{correlationGuid} - {result.data?.Count()} Perfis obtidos com sucesso ", result);
                result.correlatioId = correlationGuid;
                return Ok(result);
            }
            else 
            {
                _logger.LogError(correlationId, result.message, "Erro nos perfis obtidos");
                return BadRequest(result);
            }           
        }

        /// <summary>
        /// Insere a empresa e o usuário que está cadastrar
        /// </summary>
        /// <returns>200</returns>
       // [HttpPost("insert")]
        //public async Task<IActionResult> InsertEnteprise([FromBody] InsertEnteprise request,CancellationToken cancellationToken)
        //{
            
        //}
        /// <summary>
        /// Serve para fazer o login
        /// </summary>
        /// <returns></returns>
       // [HttpGet("login")]
       // public async Task<IActionResult> FindUserAsLogin() { }
        /// <summary>
        /// Insere a forma de recuperação da conta
        /// </summary>
        /// <returns>200</returns>
       // [HttpPost("insert-recover")]
    //    public async Task<IActionResult> InsertRecover() { }
        /// <summary>
        /// Obtem a forma de recuperar Senha
        /// </summary>
        /// <returns></returns>
      ///  [HttpPost("get-recover")]
    //    public async Task<IActionResult> GetRecover() { }
    }
}
