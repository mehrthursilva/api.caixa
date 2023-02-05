using pnb.api.easybox.domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pnb.api.easybox.domain.Interface
{
    public interface IPerfilUsuarioService
    {
        Task<DataResults<IEnumerable<Perfil_Usuario>>> GetAllUserProfiles(Guid correlationId);
    }
}
