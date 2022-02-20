using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCore.Tips.Domain
{
    public class Departamento
    {
        public int Id { get; set; }
        public string Descricao { get; set; }

        public List<Colaborador> Colaboradores { get; set; }
    }
}
