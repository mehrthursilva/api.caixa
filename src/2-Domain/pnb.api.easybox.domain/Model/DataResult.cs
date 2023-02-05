using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pnb.api.easybox.domain.Model
{
    public class DataResults<T>
    {
      public bool didResult { get; set; } = false;
      public string? message { get; set; }
      public T? data { get; set; }
      public Guid correlatioId { get; set; }
      public DateTime dateCreation { get; set; }
    }
}
