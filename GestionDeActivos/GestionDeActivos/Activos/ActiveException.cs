using System;
using System.Runtime.Serialization;

namespace GestionDeActivos.Activos
{
    public class ActiveException : Exception
    {
        public ActiveException(string message) : base(message) {; }
    }
}