using GestionDeActivos.Compañias;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDeActivos.Personas
{
    class OwnAssembler : IPerson, INotifyPropertyChanged
    {
        private string name;
        private string surname;
        private Company company;
        public string NameCompany => company?.Name;
        public string TypePerson => "Montador Externo";
        public string FullName => $"{Surname}, {Name}";

        public OwnAssembler(string name, string surname, Company company)
        {
            if (company.Name != "Monbake")
            {
                throw new PersonException($"Un {TypePerson} no puede ser de la empresa {company.Name}");
            }

            Name = name;
            Surname = surname;
            Company = company;
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
                    OnPropertyChanged(nameof(FullName));
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
                    OnPropertyChanged(nameof(FullName));
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

        public override string ToString() => $"{FullName} ({TypePerson})\n";

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

