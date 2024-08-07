using GestionDeActivos.Compañias;
using System.ComponentModel;

namespace GestionDeActivos.Personas
{
    class Commercial : IPerson, INotifyPropertyChanged
    {
        private string name;
        private string surname;
        private Company company;
        private string telephone;
        private string email;
        public string NameCompany => company?.Name;
        public string TypePerson => "Comercial";

        public Commercial(string name, string surname, Company company, string telephone, string email)
        {
            if (company.Name == "Monbake")
            {
                throw new PersonException($"Un {TypePerson} no puede ser de la empresa {company.Name}");
            }

            Name = name;
            Surname = surname;
            Company = company;
            Telephone = telephone;
            Email = email;
        }

        public Commercial() { }

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

        public Company Company
        {
            get => company;
            set
            {
                if (company != value)
                {
                    company = value;
                    OnPropertyChanged(nameof(NameCompany));
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

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}