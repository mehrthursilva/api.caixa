using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace pnb.api.easybox.domain.Model
{
    public class Perfil_Usuario
    {
        public int CodPerfilUsuario { get; set; }
        public int CodRede { get; set; }
        public string? NomePerfil { get; set; }
        public bool Ativo { get; set; }
        public bool PerfilRevendedor { get; set; }
        public DateTime dateCreation { get; set; }
    }
}
