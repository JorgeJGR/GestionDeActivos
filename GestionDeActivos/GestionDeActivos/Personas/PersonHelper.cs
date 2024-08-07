using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GestionDeActivos.Personas
{
    public static class PersonHelper
    {
        public static List<string> GetTypePersons()
        {
            var typePersons = new List<string>();

            var interfaceType = typeof(IPerson);
            var assembly = Assembly.GetExecutingAssembly();

            var implementations = assembly.GetTypes()
                                          .Where(t => interfaceType.IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

            foreach (var implementation in implementations)
            {
                var instance = (IPerson)Activator.CreateInstance(implementation);
                typePersons.Add(instance.TypePerson);
            }

            return typePersons;
        }
    }
}

