using GestionDeActivos.Compañias;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDeActivos.Personas
{
    class ExternalAssembler : IPerson, INotifyPropertyChanged
    {
        private string name;
        private string surname;
        private Company company;
        private string telephone;
        public string NameCompany => company?.Name;
        public string TypePerson => "Montador Externo";

        public ExternalAssembler(string name, string surname, Company company, string telephone)
        {
            if (company.Name == "Monbake")
            {
                throw new PersonException($"Un {TypePerson} no puede ser de la empresa {company.Name}");
            }

            Name = name;
            Surname = surname;
            Company = company;
            Telephone = telephone;
        }

        public ExternalAssembler() { }

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

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}