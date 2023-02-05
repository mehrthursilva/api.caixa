using AutoMapper;
using Microsoft.Extensions.Logging;
using pnb.api.easybox.domain.Interface;
using pnb.api.easybox.domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace pnb.api.easybox.aplication.Services
{
    public class PerfilUsuarioService : IPerfilUsuarioService
    {
        private readonly IPerfilUsuarioRepository _perfilUsuarioRepository;
        private readonly ILogger<PerfilUsuarioService> _logger;
        private readonly IMapper _mapper;

        public PerfilUsuarioService(IPerfilUsuarioRepository perfilUsuarioRepository,
            ILogger<PerfilUsuarioService> logger,
            IMapper mapper)
        {
            _perfilUsuarioRepository = perfilUsuarioRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<DataResults<IEnumerable<Perfil_Usuario>>> GetAllUserProfiles(Guid correlationId)
        {
            var dataResult = new DataResults<IEnumerable<Perfil_Usuario>> { };
            dataResult.correlatioId = correlationId;
            try
            {
                _logger.LogInformation($"{correlationId} - [PerfilUsuarioService-GetAllUserProfiles] - Obtendo todos os perfis de usuários ...");
                dataResult.data = await _perfilUsuarioRepository.GetAllUserProfiles();
                dataResult.message = $"{correlationId} - [PerfilUsuarioService-GetAllUserProfiles] - Perfis de Usuarios Retornados com Sucesso!";
                dataResult.didResult = true;
            }
            catch (Exception ex) {
                _logger.LogError($"Bug - {correlationId}");
                dataResult.message = $"[PerfilUsuarioService-GetAllUserProfiles] - {ex.Message} \n {ex.StackTrace}";
                _logger.LogError(ex, $"{correlationId} - [PerfilUsuarioService-GetAllUserProfiles] - Erro ao obter os perfis de usuários.");
            }
            return dataResult;
        }

        //Gravar os dados (1)
        public async Task<DataResults<InsertEnteprise>> InsertEntepriseFirstTime(InsertEnteprise entreprise, Guid correlationId)
        {
            Random randNum = new Random();
            var dataResult = new DataResults<InsertEnteprise> { };
            dataResult.data = entreprise;
            try
            {
                _logger.LogInformation($"{correlationId} - [PerfilUsuarioService-InsertEntepriseFirstTime] - Inserindo na tabela login...");
                var login = _mapper.Map<InsertEnteprise, Login>(entreprise);
                var user = _mapper.Map<InsertEnteprise, Users>(entreprise);
                var recover = new Recover(); // _mapper.Map<InsertEnteprise,Recover>(entreprise);
                var isLoginInserted = await _perfilUsuarioRepository.InsertLogin(login);
                if (isLoginInserted > 0)
                {
                    _logger.LogInformation($"{correlationId} - [PerfilUsuarioService-InsertEntepriseFirstTime-InsertLogin] - Inserido na tabela login com Sucesso! ID: {isLoginInserted}...");
                    _logger.LogInformation($"{correlationId} - [PerfilUsuarioService-InsertEntepriseFirstTime-InsertLogin] - Inserindo na tabela User...");
                }
                user.idUserEntreprise = (int)isLoginInserted;
                var isUserInserted = await _perfilUsuarioRepository.InsertUsers(user);
                if (isUserInserted > 0)
                {
                    _logger.LogInformation($"{correlationId} - [PerfilUsuarioService-InsertEntepriseFirstTime-InsertUsers] - Inserido na tabela User com Sucesso! ID: {isUserInserted}...");
                    _logger.LogInformation($"{correlationId} - [PerfilUsuarioService-InsertEntepriseFirstTime-InsertUsers] - Inserindo na tabela Recover...");
                }
                recover.defaultKey = (randNum.Next(0, 9) + randNum.Next(0, 9) + randNum.Next(0, 9) + randNum.Next(0, 9)).ToString();
                recover.codeForEmail = (randNum.Next(0, 9) + randNum.Next(0, 9) + randNum.Next(0, 9) + randNum.Next(0, 9)).ToString();
                recover.codeForPhone = (randNum.Next(0, 9) + randNum.Next(0, 9) + randNum.Next(0, 9) + randNum.Next(0, 9)).ToString();
                recover.idUserProfile = (int)isLoginInserted;
                recover.email = entreprise.emailEp;
                var isInsertRecover = await _perfilUsuarioRepository.InsertRecover(recover);
                if (isInsertRecover > 0)
                {
                    _logger.LogInformation($"{correlationId} - [PerfilUsuarioService-InsertEntepriseFirstTime-InsertRecover] - Inserido na tabela Recover com Sucesso! ID: {isInsertRecover}...");
                    _logger.LogInformation($"{correlationId} - [PerfilUsuarioService-InsertEntepriseFirstTime-InsertRecover] - Inserindo na tabela Recover...");
                    Reporting.Reporting.ReportingToEmail(entreprise.emailEp, "[CHAOS] - CRIAÇÃO DE CONTA", "Hollá, seu código é :" + recover.codeForEmail);
                    Reporting.Reporting.ReportToTwilio(entreprise.numberPhoneEp, "[CHAOS] - CRIAÇÃO DE CONTA, Hollá, seu código é :" + recover.codeForPhone);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Bug - {correlationId}");
                dataResult.message = $"[PerfilUsuarioService-InsertEntepriseFirstTime] - {ex.Message} \n {ex.StackTrace}";
                _logger.LogError(ex, $"{correlationId} - [PerfilUsuarioService-InsertEntepriseFirstTime] - Erro ao obter os perfis de usuários.");
            }
            return dataResult;
        }

        //Validação dos dados (1)
        public async Task<DataResults<bool?>> CheckTelephoneOrEmail(string defaultKey,string email,int flagTypeWarning,Guid correlationId) 
        {
            var dataResult = new DataResults<bool?> { };
            _logger.LogInformation($"{correlationId} - [PerfilUsuarioService-CheckTelephoneOrEmail] - Verificando se a chave bate certo com a chave que caiu no dispositivo...");
            dataResult.data = await _perfilUsuarioRepository.FindVerifyUserWithRecovery(defaultKey,email,flagTypeWarning);
            _logger.LogInformation($"{correlationId} - [PerfilUsuarioService-CheckTelephoneOrEmail] - Resultado: {dataResult.data}...");
            if ((bool)!dataResult.data) 
            {
                dataResult.data = false;
                dataResult.didResult = false;
            }
            _logger.LogInformation($"{correlationId} - [PerfilUsuarioService-CheckTelephoneOrEmail] - Retornando...");
            return dataResult;
        }

        //Login (2)
        public async Task<DataResults<bool?>> InsertValueForVerifyUserFlag(string defaultKey, string email, int flagVerification, Guid correlationId) 
        {
            var dataResult = new DataResults<bool?> { };
            _logger.LogInformation($"{correlationId} - [PerfilUsuarioService-InsertValueForVerifyUserFlag] - Vinculando os códigos ao usuário...");
            if (await _perfilUsuarioRepository.InsertValueForVerifyUserFlag(defaultKey, email, flagVerification) > 0) 
            {
                dataResult.data = true;
                dataResult.didResult = true;
            }
            return dataResult;
        }

        // find username and email

        public async Task<DataResults<bool?>> CheckEmailandPassword(string email,string password, Guid correlationId) 
        {
            Random randNum = new Random();
            var dataResult = new DataResults<bool?> { };
            _logger.LogInformation($"{correlationId} - [PerfilUsuarioService-CheckEmailandPassword] - Vinculando os códigos ao usuário...");
            var isHim = await _perfilUsuarioRepository.FindUserAsLogin(email,password);
            if (isHim.idUserEntreprise>0) 
            {
                _logger.LogInformation($"{correlationId} - [PerfilUsuarioService-CheckEmailandPassword] - Usuário existente...");
                var defaultKey = (randNum.Next(0, 9) + randNum.Next(0, 9) + randNum.Next(0, 9) + randNum.Next(0, 9)).ToString();
                await _perfilUsuarioRepository.InsertValueForVerifyUserFlag(defaultKey, email, 2);     
                Reporting.Reporting.ReportToTwilio(isHim.numberPhone, "[CHAOS] - CRIAÇÃO DE CONTA, Hollá, seu código é :" + defaultKey);
                dataResult.data = true;
                dataResult.didResult= true;
            }
            return dataResult;
        }
    }
}
