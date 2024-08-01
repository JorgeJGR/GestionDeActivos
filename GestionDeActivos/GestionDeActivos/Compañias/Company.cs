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

        public int IdCompany
        {
            get => idCompany;
            set 
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
            Name = name;
            if (Name != "Monbake")
            {
                Type = TypeCompany.Externa;

            }
            else
            {
                Type = TypeCompany.Interna;
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