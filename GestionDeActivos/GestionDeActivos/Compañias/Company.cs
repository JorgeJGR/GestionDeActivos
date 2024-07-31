using System;
using System.ComponentModel;

namespace GestionDeActivos.Compañias
{
    public class Company : INotifyPropertyChanged
    {
        public enum TypeCompany { Interna, Externa }

        private int idCompany;
        private string name;
        private TypeCompany type;
        private string telephone;
        private string email;

        private static int nextIdCompany = 1;

        public int IdCompany
        {
            get => idCompany;
            internal set 
            {
                if (idCompany != value)
                {
                    idCompany = value;
                    OnPropertyChanged(nameof(IdCompany));
                }
            }
        }

        public string Name
        {
            get => name;
            private set
            {
                if (name != value)
                {
                    name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        public TypeCompany Type
        {
            get => type;
            internal set 
            {
                if (type != value)
                {
                    type = value;
                    OnPropertyChanged(nameof(Type));
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

        public Company(string name, string telephone, string email)
        {
            IdCompany = nextIdCompany;
            nextIdCompany++;

            if (IdCompany == 1)
            {
                Name = "Monbake";
                Type = TypeCompany.Interna;
            }
            else
            {
                Name = name;
                Type = TypeCompany.Externa;
            }

            if (IdCompany != 1 && Type == TypeCompany.Interna)
            {
                throw new CompanyException("Si IdCompany es distinto de 1, el tipo de Compañia debe ser 'Externa'.");
            }

            Telephone = telephone;
            Email = email;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public override bool Equals(object obj)
        {
            if (obj is Company other)
            {
                return IdCompany == other.IdCompany;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return IdCompany.GetHashCode();
        }

        public override string ToString() => $"{IdCompany}: {Name} ({Type})\n"
                                            + $"Teléfono: {Telephone}\n"
                                            + $"Email: {Email}\n";
    }
}