using pnb.api.easybox.domain.Interface;
using pnb.api.easybox.domain.Model;
using pnb.api.easybox.repository.Configuration;
using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Reflection;
using static Slapper.AutoMapper;

namespace pnb.api.easybox.repository.Repository
{
    public class PerfilUsuarioRepository: IPerfilUsuarioRepository
    {
        private readonly ConfigurationRepoitory? _configurationRepoitory;
        public PerfilUsuarioRepository(ConfigurationRepoitory? configurationRepoitory)
        {
            _configurationRepoitory = configurationRepoitory;
        }

        public async Task<IEnumerable<Perfil_Usuario>> GetAllUserProfiles()
        {
            IEnumerable<Perfil_Usuario>? userProfiles = null;
            await using (var conn = new SqlConnection(_configurationRepoitory?.SQLDbConfiguration))
            {
                conn.Open();
                const string? query = @"select 
                                         Cod_Perfil_Usuario_int as CodPerfilUsuario
                                        ,Cod_Rede_int as CodRede                      
                                        ,Nome_Perfil_Var as NomePerfil                
                                        ,Perfil_Ativo_bit as Ativo                    
                                        ,Perfil_Default_Revendedor_bit as PerfilRevendedor
                                        from [dbo].[tbl_Perfil_Usuario]";
                userProfiles = conn.Query<Perfil_Usuario>(query);
                return userProfiles;
            }
        }
        //  ponto 1 - Login primeira vez
        public async Task<int?> InsertLogin(Login login)
        {
            int? result = 0;
            var sqlCommand = @"INSERT INTO [dbo].[login]
               (
               ,[userName]
               ,[document] 
               ,[address]
               ,[zip]
               ,[email]
               ,[country]
               ,[numberPhone]
               ,[password]
               ,[dateCreation]
               ,[webhookUrl])

             OUTPUT INSERTED.idUser VALUES

               ( @userName
                ,@document
                ,@address
                ,@zip
                ,@email
                ,@country
                ,@numberPhone              
                ,@password
                ,@dateCreation
                ,@webhookUrl
                ,@hookTeams)";
            //hookTeams
            await using (var conn = new SqlConnection(_configurationRepoitory?.SQLDbConfiguration))
            {
                conn.Open();
                var parametros = new DynamicParameters();
                parametros.Add("@userName", login.userName);
                parametros.Add("@document", login.document);
                parametros.Add("@address", login.address);
                parametros.Add("@zip", login.zip);
                parametros.Add("@email", login.email);
                parametros.Add("@country", login.country);
                parametros.Add("@numberPhone", login.numberPhone);
                parametros.Add("@password", login.password);
                parametros.Add("@dateCreation", login.dateCreation);
                parametros.Add("@webhookUrl", login.webhook);
                parametros.Add("@hookTeams", login.hookTeams);
                result = conn.QuerySingle<int>(sqlCommand, parametros);
            };
            return result;
        }
        public async Task<int?> InsertUsers(Users user)
        {
            int? result = 0;
            var sqlCommand = @"INSERT INTO [dbo].[users]
               (
               ,[idUserEntreprise]
               ,[userName] 
               ,[email]
               ,[numberPhone]
               ,[profileType]
               ,[password]
               ,[numberPhone]
               ,[dateCreation]
               ,[lastLogin]
               ,[isAdministrator]
               ,[isActive]
               ,[permissions]
               ,[hookTeams])

             OUTPUT INSERTED.idUserProfile VALUES

               ( @idUserEntreprise
                ,@userName
                ,@email
                ,@numberPhone
                ,@profileType
                ,@password
                ,@numberPhone              
                ,@dateCreation
                ,@lastLogin
                ,@isAdministrator
                ,@isActive
                ,@permissions              
                ,@hookTeams)";
            
            await using (var conn = new SqlConnection(_configurationRepoitory?.SQLDbConfiguration))
            {
                conn.Open();
                var parametros = new DynamicParameters();
                parametros.Add("@idUserEntreprise", user.idUserEntreprise);
                parametros.Add("@userName", user.userName);
                parametros.Add("@email", user.email);
                parametros.Add("@numberPhone", user.numberPhone);
                parametros.Add("@profileType", user.profileType);
                parametros.Add("@password", user.password);
                parametros.Add("@numberPhone", user.numberPhone);
                parametros.Add("@dateCreation", user.dateCreation);
                parametros.Add("@lastLogin", user.numberPhone);
                parametros.Add("@isAdministrator", user.profileType);
                parametros.Add("@isActive", user.password);
                parametros.Add("@permissions", user.numberPhone);
                parametros.Add("@hookTeams", user.dateCreation);
                result = conn.QuerySingle<int>(sqlCommand, parametros);
            };
            return result;
        }
        public async Task<int?> InsertRecover(Recover recover)
        {
            int? result = 0;
            var sqlCommand = @"INSERT INTO [dbo].[recover]
               (
               ,[email]
               ,[defaultKey] 
               ,[shortQuestion]
               ,[shortanswer]
               ,[dateCreation]
               ,[codeForPhone]
               ,[codeForEmail])

             OUTPUT INSERTED.idUserProfile VALUES

               ( @email
                ,@defaultKey
                ,@shortQuestion
                ,@shortanswer
                ,@dateCreation
                ,@codeForPhone
                ,@codeForEmail)";

            await using (var conn = new SqlConnection(_configurationRepoitory?.SQLDbConfiguration))
            {
                conn.Open();
                var parametros = new DynamicParameters();
                parametros.Add("@email", recover.email);
                parametros.Add("@defaultKey", recover.defaultKey);
                parametros.Add("@shortQuestion", recover.shortQuestion);
                parametros.Add("@shortanswer", recover.shortanswer);
                parametros.Add("@dateCreation", recover.dateCreation);
                parametros.Add("@codeForPhone", recover.codeForPhone);
                parametros.Add("@codeForEmail", recover.codeForEmail);
                result = conn.QuerySingle<int>(sqlCommand, parametros);
            };
            return result;
        }

        //  ponto 2 - Login primeira vez
        public async Task<bool?> FindVerifyUserWithRecovery(string defaultKey, string email, int flagTypeWarning)
        {
            await using (var conn = new SqlConnection(_configurationRepoitory?.SQLDbConfiguration))
            {
                bool sistema;
                string name = "";
                string query = "";
                conn.Open();
                if (flagTypeWarning == 1) 
                {
                    name = "@defaultKey";
                    query = @" select defaultKey from recover where defaultKey = @defaultKey and email = @email";
                }
                if (flagTypeWarning == 2) 
                {
                    name = "@codeForPhone";
                    query = @" select codeForPhone from recover where codeForPhone = @codeForPhone and email = @email";
                }
                if (flagTypeWarning == 3) 
                {
                    name = "@codeForEmail";
                    query = @" select codeForEmail from recover where codeForEmail = @codeForEmail and email = @email";
                }         
                var parametros = new DynamicParameters();
                parametros.Add(name,defaultKey,System.Data.DbType.String,System.Data.ParameterDirection.Input);
                parametros.Add("@email",email,System.Data.DbType.String,System.Data.ParameterDirection.Input);
                sistema = conn.Query(query, parametros).Any();
                return sistema;
            }
        }
        public async Task<int?> InsertValueForVerificationUser(string defaultKey, string email)
        {
            int? result = 0;
            var sqlCommand = @"update recover 
                                      set defaultKey = @defaultKey,
                                          dateCreation = @dateCreation
                                    where email =  @email";
            await using (var conn = new SqlConnection(_configurationRepoitory?.SQLDbConfiguration))
            {
                conn.Open();
                var parametros = new DynamicParameters();
                parametros.Add("@defaultKey", defaultKey);
                parametros.Add("@email", email);
                parametros.Add("@dateCreation", DateTime.Now);
                result = conn.Execute(sqlCommand, parametros);
            };
            return result;
        }

        //  ponto 3 - Login primeira vez
        public async Task<int?> InsertTimeStampExpiration(TimeStampExpiration timeStampExpiration)
        {
            int? result = 0;
            var sqlCommand = @"INSERT INTO [dbo].[timeStampExpiration]
               (
               ,[idUser]
               ,[idUserProfile] 
               ,[dateCreation])
                VALUES
               ( @idUser
                ,@idUserProfile
                ,@dateCreation)";

            await using (var conn = new SqlConnection(_configurationRepoitory?.SQLDbConfiguration))
            {
                conn.Open();
                var parametros = new DynamicParameters();
                parametros.Add("@idUser", timeStampExpiration.idUser);
                parametros.Add("@idUserProfile", timeStampExpiration.idUserProfile);
                parametros.Add("@dateCreation", timeStampExpiration.dateCreation);
                result = conn.Execute(sqlCommand, parametros);
            };
            return result;
        }


        //  ponto 4 - Login Segunda vez

        public async Task<Users> FindUserAsLogin(string userName, string password)
        {
            await using (var conn = new SqlConnection(_configurationRepoitory?.SQLDbConfiguration))
            {
                Users sistema = null;
                conn.Open();
                const string query = @" select
                                        idUserEntreprise as idUserEntreprise
                                        ,idUserProfile  as idUserProfile
                                        ,userName   as userName 
                                        ,email   as email
                                        ,numberPhone   as numberPhone  
                                        ,profileType   as profileType
                                        ,password  as password
                                        ,dateCreation  as dateCreation 
                                        ,lastLogin  as lastLogin 
                                        ,isAdministrator   as isAdministrator
                                        ,isActive   as isActive
                                        ,permissions   as permission
                                        ,hookTeams  as hookTeams FROM users  where email = @email and password = @password";
                var parametros = new DynamicParameters();
                parametros.Add("@email", userName, System.Data.DbType.String, System.Data.ParameterDirection.Input);
                parametros.Add("@password", password, System.Data.DbType.String, System.Data.ParameterDirection.Input);
                sistema = conn.Query<Users>(query, parametros).AsList()[0];
                return sistema;
            }
        }
           
        public async Task<int?> InsertValueForVerifyUserFlag(string defaultKey, string email,int flagVerification)
        {
            int? result = 0;
            string sqlCommand = "";
            string typeOf = "";

            switch (flagVerification) 
            {
                case 1 : sqlCommand = @"update recover 
                                      set defaultKey = @defaultKey,
                                          dateCreation = @dateCreation
                                    where email =  @email";
                    typeOf = "@defaultKey";
                    break;
                case 2: sqlCommand = @"update recover 
                                      set codeForPhone = @defaultKey,
                                          dateCreation = @dateCreation
                                    where email =  @email";
                    typeOf = "@codeForPhone";
                    break;
                case 3: sqlCommand = @"update recover 
                                      set codeForEmail = @defaultKey,
                                          dateCreation = @dateCreation
                                    where email =  @email";
                    typeOf = "@codeForEmail";
                    break;

            }
            await using (var conn = new SqlConnection(_configurationRepoitory?.SQLDbConfiguration))
            {
                conn.Open();
                var parametros = new DynamicParameters();
                parametros.Add(typeOf, defaultKey);
                parametros.Add("@email", email);
                parametros.Add("@dateCreation", DateTime.Now);
                result = conn.Execute(sqlCommand, parametros);
            };
            return result;
        }

        // public async Task<bool> FindVerifyUserWithRecovery(string defaultKey, string email)

        // ponto 5 - adicionar User

        // public async Task<int?> InsertLogin(Login login)

        // public async Task<int?> InsertRecover(Recover recover)

        // public async Task<int?> InsertTimeStampExpiration(TimeStampExpiration timeStampExpiration)

        // ponto 2 - Login primeira vez

    }
}