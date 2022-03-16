using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SitioPersona.Models
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public string Usuario1 { get; set; }
        public string Pass { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public int? FkPersona { get; set; }

        public virtual Persona FkPersonaNavigation { get; set; }
    }
}
