using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDeActivos.Personas
{
    interface IPerson
    {
        string Name { get; set; }
        string Surname { get; set; }
        string NameCompany { get; }
        string TypePerson { get; }
        string FullName { get; }
    }
}
