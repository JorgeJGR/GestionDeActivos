using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDeActivos.Personas
{
    class PersonService
    {
        public ObservableCollection<PersonDTO> Persons { get; }
        public PersonService() => Persons = new ObservableCollection<PersonDTO>();

        public void AddPerson(PersonDTO p)
        {
            if (Persons.Contains(p))
                throw new PersonsException($"{p.FullName} ya se encuentra en la lista.");
            Persons.Add(p);
        }

        public void UpdatePersonData(PersonDTO p)
        {
            if (!Persons.Contains(p))
                throw new PersonsException($"{p.FullName} no se encuentra en la lista para poder ser actualizada");

            var existingPerson = Persons.FirstOrDefault(comp => comp.Equals(p));
            if (existingPerson != null)
            {
                existingPerson.Name = p.Name;
                existingPerson.Surname = p.Surname;
                existingPerson.TypePerson = p.TypePerson;

                if (p.TypePerson == "Comercial")
                {
                    existingPerson.Telephone = p.Telephone;
                    existingPerson.Email = p.Email;
                }
                else if (p.TypePerson == "Montador Externo")
                {
                    existingPerson.Telephone = p.Telephone;
                    existingPerson.Email = null;
                }
                else if (p.TypePerson == "Montador Propio")
                {
                    existingPerson.Telephone = null;
                    existingPerson.Email = null;
                }
            }
        }

        public void RemovePersonData(PersonDTO p)
        {
            if (!Persons.Contains(p))
                throw new PersonsException($"{p.FullName} no se encuentra en la lista para poder ser eliminada");

            Persons.Remove(p);
        }

        public PersonDTO SeePerson(PersonDTO p)
        {
            if (!Persons.Contains(p))
            {
                throw new PersonsException($"{p.FullName} no se encuentra en la lista.");
            }

            return Persons.FirstOrDefault(comp => comp.Equals(p));
        }
    }
}