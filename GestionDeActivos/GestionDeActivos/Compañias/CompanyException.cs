using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDeActivos.Compañias
{
    public class CompanyException : Exception
    {
        public CompanyException(string message) : base(message) { }
    }
}
