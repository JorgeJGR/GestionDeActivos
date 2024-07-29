using System;
using System.Runtime.Serialization;

namespace GestionDeActivos.Personas
{
    public class PersonException : Exception
    {
        public PersonException(string message) : base(message) {; }
    }
}