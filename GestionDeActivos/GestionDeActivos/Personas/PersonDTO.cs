
using System;
using System.ComponentModel;

namespace GestionDeActivos.Personas
{
    class PersonDTO : INotifyPropertyChanged
    {
        private int idPerson;
        private string name;
        private string surname;
        private string nameCompany;
        private string typePerson;
        private string fullName;
        private string telephone;
        private string email;

        private static int nextIdPerson = 1;

        public int IdPerson
        {
            get => idPerson;
            internal set
            {
                if (idPerson != value)
                {
                    idPerson = value;
                    OnPropertyChanged(nameof(IdPerson));
                }
            }
        }

        public string Name
        {
            get => name;
            set
            {
                if (name != value)
                {
                    name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        public string Surname
        {
            get => surname;
            set
            {
                if (surname != value)
                {
                    surname = value;
                    OnPropertyChanged(nameof(Surname));
                }
            }
        }

        public string NameCompany
        {
            get => nameCompany;
            set
            {
                if (nameCompany != value)
                {
                    nameCompany = value;
                    OnPropertyChanged(nameof(NameCompany));
                }
            }
        }

        public string TypePerson
        {
            get => typePerson;
            set
            {
                if (typePerson != value)
                {
                    typePerson = value;
                    OnPropertyChanged(nameof(TypePerson));
                }
            }
        }

        public string FullName
        {
            get => fullName;
            set
            {
                if (fullName != value)
                {
                    fullName = value;
                    OnPropertyChanged(nameof(FullName));
                }
            }
        }

        public string Telephone
        {
            get => telephone;
            set
            {
                if (telephone != value)
                {
                    telephone = value;
                    OnPropertyChanged(nameof(Telephone));
                }
            }
        }

        public string Email
        {
            get => email;
            set
            {
                if (email != value)
                {
                    email = value;
                    OnPropertyChanged(nameof(Email));
                }
            }
        }

        public PersonDTO(IPerson person)
        {
            IdPerson = nextIdPerson;
            nextIdPerson++;

            Name = person.Name;
            Surname = person.Surname;
            NameCompany = person.NameCompany;
            TypePerson = person.TypePerson;
            FullName = $"{person.Name} {person.Surname}";

            if (person is Commercial commercial)
            {
                Telephone = commercial.Telephone;
                Email = commercial.Email;
            }
            else if (person is ExternalAssembler externalAssembler)
            {
                Telephone = externalAssembler.Telephone;
                Email = null;
            }
            else if (person is OwnAssembler)
            {
                Telephone = null;
                Email = null;
            }
        }

        public override bool Equals(object obj)
        {
            if (obj is PersonDTO other)
            {
                return FullName == other.FullName;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return FullName?.GetHashCode() ?? 0;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}