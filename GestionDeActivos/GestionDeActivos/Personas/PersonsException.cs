using System;
using System.Runtime.Serialization;

namespace GestionDeActivos.Personas
{
    public class PersonsException : Exception
    {
        public PersonsException(string message) : base(message) {; }
    }
}