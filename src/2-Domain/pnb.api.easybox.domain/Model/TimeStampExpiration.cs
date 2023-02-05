using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pnb.api.easybox.domain.Model
{
    public class TimeStampExpiration
    {
        public int idUser { get; set; }
        public int idUserProfile { get; set; }
        public DateTime dateCreation { get; set; }
    }
}
