using pnb.api.easybox.domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pnb.api.easybox.domain.Interface
{
    public interface IPerfilUsuarioRepository
    {
        Task<IEnumerable<Perfil_Usuario>> GetAllUserProfiles();
        Task<int?> InsertLogin(Login login);
        Task<int?> InsertUsers(Users user);
        Task<int?> InsertRecover(Recover recover);
        Task<bool> FindVerifyUserWithRecovery(string defaultKey, string email, int flagTypeWarning);
        Task<int?> InsertValueForVerificationUser(string defaultKey, string email);
        Task<int?> InsertTimeStampExpiration(TimeStampExpiration timeStampExpiration);
        Task<Users> FindUserAsLogin(string userName, string password);
        Task<int?> InsertValueForVerifyUserFlag(string defaultKey, string email, int flagVerification);
    }
}
